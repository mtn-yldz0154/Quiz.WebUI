using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public QuizSessionController(QuizContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("Mola1Getir")]
        public async Task<IActionResult> Mola1Getir()
        {
            const string cacheKey = "Mola1RemainingSeconds";

            // Cache'ten kalan süreyi al
            if (!_cache.TryGetValue(cacheKey, out int? remainingSeconds))
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

                // Eğer mola süresi henüz ayarlanmamışsa
                var mola1Dakika = lastOturum.Mola1; // Null ise 0 kabul et
                remainingSeconds = mola1Dakika * 60;

                // Cache'e ekle
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(mola1Dakika));
                _cache.Set(cacheKey, remainingSeconds, cacheEntryOptions);
            }

            // Süreyi azalt ve güncelle
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                _cache.Set(cacheKey, remainingSeconds); // Cache'i güncelle
            }

            return Ok(new { remainingTime = remainingSeconds });
        }

        [HttpGet("Mola2Getir")]
        public async Task<IActionResult> Mola2Getir()
        {
            const string cacheKey = "Mola2RemainingSeconds";

            // Cache'ten kalan süreyi al
            if (!_cache.TryGetValue(cacheKey, out int? remainingSeconds))
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

                // Eğer mola süresi henüz ayarlanmamışsa
                var mola2Dakika = lastOturum.Mola2; // Null ise 0 kabul et
                remainingSeconds = mola2Dakika * 60;

                // Cache'e ekle
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(mola2Dakika));
                _cache.Set(cacheKey, remainingSeconds, cacheEntryOptions);
            }

            // Süreyi azalt ve güncelle
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                _cache.Set(cacheKey, remainingSeconds); // Cache'i güncelle
            }

            return Ok(new { remainingTime = remainingSeconds });
        }
    }
}
