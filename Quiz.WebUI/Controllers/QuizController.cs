using Microsoft.AspNetCore.Mvc;

namespace Quiz.WebUI.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
