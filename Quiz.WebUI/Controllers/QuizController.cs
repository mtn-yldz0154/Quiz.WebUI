using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz.WebUI.Context;
using Quiz.WebUI.Entities;
using Quiz.WebUI.Models;
using System.Reflection.Metadata;

namespace Quiz.WebUI.Controllers
{
    public class QuizController : Controller
    {
        private QuizContext _quizContext;

        public QuizController(QuizContext quizContext)
        {
            _quizContext = quizContext;
        }
        public IActionResult Index()
        {
       
            

            return View();
        }

        public IActionResult StartQuiz(string id)
        {
            var oturum=_quizContext.Oturums.Where(i=>i.Status==1).FirstOrDefault();

            if(oturum==null)
            {
               return RedirectToAction("Index");

            }

            ViewBag.Token = id;
            ViewBag.name = _quizContext.Contestants.Where(i => i.Token == id).FirstOrDefault().Name;
            ViewBag.puan = _quizContext.Contestants.Where(i => i.Token == id).FirstOrDefault().Skor;

            return PartialView("StartQuiz");

        }

        [HttpPost]
        public IActionResult FinishQuiz([FromBody] QuizResultModel model)
        {
            var oturum = _quizContext.Oturums
                .Where(i => i.Status == 1)
                .FirstOrDefault();

            if (oturum == null)
            {
                return BadRequest("Aktif oturum bulunamadı.");
            }

            if (model.Name=="")
            {
                Random random = new Random();
                int randomNumber = random.Next(100000, 999999); // Generates a 6-digit random number
                model.Name = "Guest_" + randomNumber;
            }

            var contestant = new Contestant()
            {
                Name = model.Name,
                OturumId = oturum.Id,
                Skor = model.Skor*10,
                Token = Guid.NewGuid().ToString()  // Token için benzersiz bir değer oluşturduk
            };
            oturum.CompetitorsNumber++;
            _quizContext.Oturums.Update(oturum);         
            _quizContext.Contestants.Add(contestant);
            _quizContext.SaveChanges();

            return Ok(contestant.Token);
        }


        [HttpPost]
        public IActionResult UpdateQuiz([FromBody] QuizResultModel model)
        {
            // Model'den gelen token ile contestant'ı bul
            var contestant = _quizContext.Contestants.FirstOrDefault(i => i.Token == model.Token);

            // Eğer contestant null ise, işlem yapılmadan hata dönülmeli
            if (contestant == null)
            {
                return BadRequest("Geçersiz kullanıcı.");
            }

            // Kullanıcının skorunu güncelle
            contestant.Skor += model.Skor;
            _quizContext.Contestants.Update(contestant);
            _quizContext.SaveChanges();

            var question = _quizContext.Questions.Where(i => i.Id == model.QuestionId).FirstOrDefault();
            if (model.QuestionType == 4)
            {
                var answer = new AnswerQuestion()
                {
                    UserToken = model.Token,
                    Answer = model.Answer,
                    OturumId = contestant.OturumId,
                    QuestionToken = model.QuestionId,
                    Munite=question.Second-model.Munite,
                };

                // Cevabı veritabanına ekle ve değişiklikleri kaydet
                _quizContext.AnswerQuestions.Add(answer);
                _quizContext.SaveChanges();

                return Ok("Cevap kaydedildi.");
            }

            return Ok("Cevap kaydedildi.");
        }



        public IActionResult ResultQuiz(string id)
        {
            var contestant = _quizContext.Contestants.Where(i => i.Token == id).FirstOrDefault();
            var contestantList = _quizContext.Contestants.Where(i => i.OturumId == contestant.OturumId).OrderByDescending(i=>i.Skor).ToList();
            var skor = contestant.Skor;

            return Json(new
            {
                Skor = skor,
                ContestantList = contestantList
            });
        }

       
        
      [HttpPost]
        public async Task<IActionResult> GetResultQuizAnswer([FromBody] QuizRequestModel model)
        {
            // Soruyu ve yarışmacıyı bul
            var question = await _quizContext.Questions.FirstOrDefaultAsync(i => i.Id == model.QuestionId);
            var contestant = await _quizContext.Contestants.FirstOrDefaultAsync(i => i.Token == model.Id);

            if (question == null)
            {
                return NotFound("Soru bulunamadı.");
            }

            // Bu soruya verilen cevapları getir ve her yarışmacının en hızlı cevabını dikkate al
            var answers = await _quizContext.AnswerQuestions
                                            .Where(i => i.QuestionToken == model.QuestionId && i.OturumId == contestant.OturumId)
                                            .GroupBy(i => i.OturumId)  // Aynı yarışmacının en hızlı cevabını al
                                            .Select(g => g.OrderBy(i => i.Munite).First())  // En hızlı cevabı al
                                            .ToListAsync();

            // Doğru cevabı al
            var correctAnswer = question.Answer;

            // En yakın cevabı ve süresini bulmak için değişkenler
            AnswerQuestion closestAnswer = null;
            int minDifference = int.MaxValue; // Cevaba olan farkı minimum tutmak için
            int bestTime = int.MaxValue;      // En iyi süreyi bulmak için
            var totalPoint = question.Puan;
            var totalTime = question.Second;
            double highScore = 0;
            // En iyi cevabı ve süresini bul
            foreach (var answer in answers)
            {
                // Doğru cevaba olan fark
                int accuracyPoints = Math.Max(0,totalPoint-Math.Abs(answer.Answer-correctAnswer));
                double timeMuliplier = (double)(totalTime - answer.Munite) / totalTime;
                double totalScore = accuracyPoints * (1 + timeMuliplier);

                // Eğer cevaba olan fark daha küçükse veya fark eşitse süreye bak
                //if (difference < minDifference || (difference == minDifference && answer.Munite < bestTime))
                //{
                //    closestAnswer = answer;
                //    minDifference = difference;
                //    bestTime = answer.Munite;
                //}

                if (totalScore >highScore)
                {
                    closestAnswer=answer;
                    highScore= totalScore;
                }
            }

            // Eğer en yakın ve en hızlı cevap varsa, tek seferde 100 puan ekleyelim
            if (closestAnswer != null)
            {
                var closestContestant = await _quizContext.Contestants.FirstOrDefaultAsync(i => i.Token == closestAnswer.UserToken);

                // Bu soruya daha önce puan verilip verilmediğini kontrol et
                var existingScore = await _quizContext.ContestantScores
                                                      .FirstOrDefaultAsync(cs => cs.ContestantId == closestContestant.Id && cs.QuestionId == model.QuestionId);

                if (closestContestant != null && existingScore == null)  // Eğer daha önce bu soruya puan eklenmemişse
                {
                    closestContestant.Skor += (int)highScore; // Puanı ekle

                    // Bu yarışmacıya ve bu soruya özel puan eklenme bilgisini kaydet
                    _quizContext.ContestantScores.Add(new ContestantScores
                    {
                        ContestantId = closestContestant.Id,
                        QuestionId = model.QuestionId,
                        Score = (int)highScore
                    });
                    await _quizContext.SaveChangesAsync(); // Veritabanına kaydet
                }

                // Tüm yarışmacıları listele ve skorlarına göre sırala
                var contestantList = await _quizContext.Contestants
                                                       .Where(i => i.OturumId == contestant.OturumId)
                                                       .OrderByDescending(i => i.Skor)
                                                       .ToListAsync();

           
                return Json(new
                {
                    Skor = contestant.Skor,
                    ContestantList = contestantList
                });
            }

            return BadRequest("En yakın cevap bulunamadı.");
        }



        // QuizRequestModel: Kullanıcıdan gelen veriyi modellemek için
        public class QuizRequestModel
        {
            public string Id { get; set; }         // Yarışmacı ID'si (Token)
            public int QuestionId { get; set; }    // Soru ID'si
        }



    }
}
