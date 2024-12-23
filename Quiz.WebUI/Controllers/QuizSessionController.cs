using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Quiz.WebUI.Context;
using System;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Quiz.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizSessionController : ControllerBase
    {
        private readonly QuizContext _context;
        private readonly IMemoryCache _cache;

        public QuizSessionController(QuizContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
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
        [HttpGet("Mola1Getir")]
        public async Task<IActionResult> Mola1Getir()
        {

            var session = await _context.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            var lastOturum = _context.Oturums.OrderByDescending(a => a.Id).FirstOrDefault();
            if (lastOturum == null)
            {
                return NotFound();
            }



            // Süreyi azalt ve güncelle
            if (lastOturum.Mola1 > 0)
            {
                lastOturum.Mola1--;
                 _context.Oturums.Update(lastOturum);
                _context.SaveChanges();
            }

            return Ok(new { remainingTime = lastOturum.Mola1 });
        }

        [HttpGet("Mola2Getir")]
        public async Task<IActionResult> Mola2Getir()
        {

            var session = await _context.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            var lastOturum = _context.Oturums.OrderByDescending(a => a.Id).FirstOrDefault();
            if (lastOturum == null)
            {
                return NotFound();
            }


            // Süreyi azalt ve güncelle
            if (lastOturum.Mola2 > 0)
            {
                lastOturum.Mola2--;
                _context.Oturums.Update(lastOturum);
                _context.SaveChanges();

            }

            return Ok(new { remainingTime = lastOturum.Mola2 });
        }
    }
}
