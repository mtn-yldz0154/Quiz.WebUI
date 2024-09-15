using Microsoft.AspNetCore.Mvc;
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
           

            var contestant = _quizContext.Contestants.Where(i => i.Token == model.Token).FirstOrDefault();
            contestant.Skor += model.Skor;

            _quizContext.Contestants.Update(contestant);
            _quizContext.SaveChanges();

            return Ok();
        }


        public IActionResult ResultQuiz(string id)
        {
            var contestant = _quizContext.Contestants.Where(i => i.Token == id).FirstOrDefault();
            var contestantList = _quizContext.Contestants.Where(i => i.OturumId == contestant.OturumId).OrderByDescending(i=>i.Skor).ToList();
            return Json(contestantList);
        }
    }
}
