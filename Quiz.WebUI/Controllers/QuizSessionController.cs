using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz.WebUI.Context;

namespace Quiz.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizSessionController : ControllerBase
    {
        private readonly QuizContext _context;

        public QuizSessionController(QuizContext context)
        {
            _context = context;
        }

        [HttpGet("GetCountdown")]
        public async Task<IActionResult> GetCountdown()
        {
            var session = await _context.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            var remainingTime = (session.QuizStartTime - DateTime.Now).TotalSeconds;
            return Ok(new { remainingTime = Math.Max(remainingTime, 0) });
        }
    }
}
