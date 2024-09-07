using Microsoft.AspNetCore.Mvc;
using Quiz.WebUI.Context;
using Quiz.WebUI.Entities;
using Quiz.WebUI.Models;

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


        public IActionResult StartQuiz()
        {


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

            return RedirectToAction("Index");
        }


    }
}
