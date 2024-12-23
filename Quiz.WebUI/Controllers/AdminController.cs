using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quiz.WebUI.Context;
using Quiz.WebUI.Dtos;
using Quiz.WebUI.Entities;
using Quiz.WebUI.Helpers;
using Quiz.WebUI.Models;

namespace Quiz.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private QuizContext _quizContext;
        private readonly IHubContext<QuizHub> _hubContext;
        public AdminController(QuizContext quizContext, IHubContext<QuizHub> hubContext)
        {
            _hubContext = hubContext;
            _quizContext = quizContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddQuestion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddQuestion([FromForm] QuestionDto model, IFormFile? imageFile)
        {
            // QuestionType değerini varsayılan olarak 'choice' olarak ayarlıyoruz
            var questionType = model.QuestionType;
            string imageName = "";
            if (imageFile != null)
            {
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

                // Dosya uzantısını kontrol et
                if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                {
                    questionType = EnumQuestionType.photo;
                }
                else if (fileExtension == ".mp4" || fileExtension == ".avi" || fileExtension == ".mov")
                {
                    questionType = EnumQuestionType.video;
                }
                else if (fileExtension == ".mp3" || fileExtension == ".mkv" || fileExtension == ".wav" || fileExtension == ".aac" || fileExtension == ".ogg" || fileExtension == ".flac")
                {
                    questionType = EnumQuestionType.sound;
                }
                imageName = UploadFile.Upload(imageFile);
            }

            var question = new Questions()
            {
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                Option4 = model.Option4,
                CorrectOption = model.CorrectOption,
                Question = model.Question,
                Second = model.Second,
                Puan = model.Puan,
                QuestionType = questionType, // Dinamik olarak belirlenen QuestionType
                ImageUrl = imageName
            };

            _quizContext.Questions.Add(question);
            _quizContext.SaveChanges();
            return RedirectToAction("GetQuestions", "Admin");
        }
        public IActionResult DeleteQuestion(int id)
        {
            // Soruyu veritabanından bul
            var question = _quizContext.Questions.Find(id);

            if (question != null)
            {
                // Dosya yolunu belirleyin (örneğin, belirli bir dizinde saklıyorsanız)
                string filePath = Path.Combine("DosyaYolu", question.ImageUrl); // ImageFileName, sorunun dosya adını tutan bir alan olmalı

                // Dosyayı sil
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Soruyu veritabanından sil
                _quizContext.Questions.Remove(question);
                _quizContext.SaveChanges();
            }

            return RedirectToAction("GetQuestions", "Admin");
        }


        public IActionResult GetQuestions()
        {
            var questions = _quizContext.Questions.ToList();
            return View(questions);
        }

        public IActionResult GetQuestionList()
        {
            var questions = _quizContext.Questions.OrderBy(i=>i.QuestionType).ToList();
            return Ok(questions);
        }
        public IActionResult StartOturum()
        {
            var questions = _quizContext.Questions.ToList();

            return View(questions);
        }

        public async Task<IActionResult> StartQuiz()
        {
            var oturum = _quizContext.Oturums.Where(i => i.Status == 1).FirstOrDefault();
            if (oturum == null)
            {
                ViewBag.oturum = 0;
            }
            else
            {
                ViewBag.oturum = 1;
                // Admin quiz'i başlatırsa tüm katılımcılara mesaj gönderir

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ManageQuiz(OturumModel oturumModel)
        {


            var oturum = new Oturum()
            {
                CompetitorsNumber = 0,
                Date = DateTime.Now,
                Token = Guid.NewGuid().ToString(),
                Munite =oturumModel.minute,
                Status = 1,
                Mola1=oturumModel.Mola1*60,
                Mola2=oturumModel.Mola2*60,
            };
            _quizContext.Oturums.Add(oturum);
            _quizContext.SaveChanges();

            var session = await _quizContext.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                session = new Entities.QuizSession { QuizStartTime = DateTime.Now.AddMinutes(oturumModel.minute) };
                _quizContext.QuizSessions.Add(session);
                await _quizContext.SaveChangesAsync();
            }
            else
            {
                session.QuizStartTime = DateTime.Now.AddMinutes(oturumModel.minute);
                _quizContext.QuizSessions.Update(session);
                await _quizContext.SaveChangesAsync();
            }

            await _hubContext.Clients.All.SendAsync("ReceiveQuizStart");
            return RedirectToAction("StartQuiz");

        }

        public async Task<IActionResult> GetCountdown()
        {
            var session = await _quizContext.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            var remainingTime = (session.QuizStartTime - DateTime.Now).TotalSeconds;
            return Ok(new { remainingTime = Math.Max(remainingTime, 0) });
        }

        public IActionResult GetOturum()
        {
            var oturum = _quizContext.Oturums.OrderByDescending(i => i.Date).ToList();

            return View(oturum);
        }


        public IActionResult GetContestant(int id)
        {
            var contestants = _quizContext.Contestants
     .Where(i => i.OturumId == id)
     .OrderByDescending(i => i.Skor)
     .ToList();

            return View(contestants);
        }
    }
    public class OturumModel
    {
        public int minute { get; set; }
        public int Mola1 { get; set; }
        public int Mola2 { get; set; }
    }
}
