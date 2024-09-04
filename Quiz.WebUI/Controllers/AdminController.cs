using Microsoft.AspNetCore.Mvc;
using Quiz.WebUI.Context;
using Quiz.WebUI.Dtos;
using Quiz.WebUI.Entities;
using Quiz.WebUI.Models;

namespace Quiz.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private QuizContext _quizContext;

        public AdminController(QuizContext quizContext)
        {
            _quizContext = quizContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  IActionResult AddQuestion()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult AddQuestion([FromBody] QuestionDto model)
        {
            var question = new Questions()
            {
                Option1 = model.Option1,
                Option2 = model.Option2,    
                Option3 = model.Option3,
                Option4 = model.Option4,
                CorrectOption = model.CorrectOption,
                Question = model.Question,
               QuestionType=EnumQuestionType.choice
            };

            _quizContext.Questions.Add(question);
            _quizContext.SaveChanges();
            return Ok(); 
        }


        public IActionResult GetQuestions()
        {
            var questions = _quizContext.Questions.ToList();
            return View(questions);
        }

        public IActionResult GetQuestionList()
        {
            var questions = _quizContext.Questions.ToList();
            return Ok(questions);
        }
        public IActionResult StartOturum()
        {
            var questions= _quizContext.Questions.ToList();

            return View(questions);
        }

      
    }
}
