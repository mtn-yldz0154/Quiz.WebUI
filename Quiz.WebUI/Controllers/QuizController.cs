using Microsoft.AspNetCore.Mvc;
using Quiz.WebUI.Context;

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
       
            var oturum = _quizContext.Oturums
                .Where(i => i.Date > DateTime.Now)
                .OrderBy(i => i.Date)
                .FirstOrDefault();
            if(oturum == null)
            {
                ViewBag.State = 0;
            }
            else
            {
                ViewBag.State = 1;

                ViewBag.Oturum = oturum;
            }

            return View();
        }
    }
}
