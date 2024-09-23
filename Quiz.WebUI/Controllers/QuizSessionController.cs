using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quiz.WebUI.Context;
using System;
using System.Threading.Tasks;

namespace Quiz.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizSessionController : ControllerBase
    {
        private readonly QuizContext _context;

        // Mola1 ve Mola2 için başlangıç zamanlarını tutacak değişkenler
        private DateTime? _mola1StartTime = null;
        private DateTime? _mola2StartTime = null;

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

        [HttpGet("Mola1Getir")]
        public async Task<IActionResult> Mola1Getir()
        {
            var session = await _context.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            // Eğer mola1 başlatılmadıysa, başlat
            if (_mola1StartTime == null)
            {
                _mola1StartTime = DateTime.Now;
            }

            var lastOturum = _context.Oturums.LastOrDefault();
            // Mola1 süresi dakika cinsinden tutuluyor
            var mola1Dakika = lastOturum.Mola1; // Eğer Mola1 null ise, 0 olarak kabul et
            var mola1Suresi = TimeSpan.FromMinutes(mola1Dakika);

            // Geçen süreyi hesapla
            var elapsed = DateTime.Now - _mola1StartTime.Value;
            var remainingTime = mola1Suresi - elapsed;

            // Eğer süre bittiyse, 0 döndür
            var remainingSeconds = Math.Max(remainingTime.TotalSeconds, 0);

            return Ok(new { remainingTime = remainingSeconds });
        }

        [HttpGet("Mola2Getir")]
        public async Task<IActionResult> Mola2Getir()
        {
            var session = await _context.QuizSessions.FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            // Eğer mola2 başlatılmadıysa, başlat
            if (_mola2StartTime == null)
            {
                _mola2StartTime = DateTime.Now;
            }
            var lastOturum = _context.Oturums.LastOrDefault();
            // Mola2 süresi dakika cinsinden tutuluyor
            var mola2Dakika = lastOturum.Mola2; // Eğer Mola2 null ise, 0 olarak kabul et
            var mola2Suresi = TimeSpan.FromMinutes(mola2Dakika);

            // Geçen süreyi hesapla
            var elapsed = DateTime.Now - _mola2StartTime.Value;
            var remainingTime = mola2Suresi - elapsed;

            // Eğer süre bittiyse, 0 döndür
            var remainingSeconds = Math.Max(remainingTime.TotalSeconds, 0);

            return Ok(new { remainingTime = remainingSeconds });
        }
    }
}
