using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quiz.WebUI.Context;
using Quiz.WebUI.Dtos;
using Quiz.WebUI.Entities;
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

        public async Task< IActionResult> ManageQuiz()
        {
            var oturum = new Oturum()
            {
                CompetitorsNumber = 0,
                Date = DateTime.Now,
                Token = Guid.NewGuid().ToString(),
                Status=1
            };
            _quizContext.Oturums.Add(oturum);
            _quizContext.SaveChanges();

            var session = await _quizContext.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                session = new Entities.QuizSession { QuizStartTime = DateTime.Now.AddSeconds(5) };
                _quizContext.QuizSessions.Add(session);
                await _quizContext.SaveChangesAsync();
            }
            else
            {
                session.QuizStartTime = DateTime.Now.AddSeconds(5);
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
}
