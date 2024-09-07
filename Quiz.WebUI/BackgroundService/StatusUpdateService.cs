//using Microsoft.EntityFrameworkCore;
//using Quiz.WebUI.Context;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//namespace Quiz.WebUI.BackgroundService
//{
//    public class StatusUpdateService : BackgroundService
//    {
//        private readonly QuizContext _quizContext;
//        private readonly ILogger<StatusUpdateService> _logger;

//        public StatusUpdateService(QuizContext quizContext, ILogger<StatusUpdateService> logger)
//        {
//            _quizContext = quizContext;
//            _logger = logger;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
            
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                await QuizModControl(stoppingToken);
//                await UpdateStatusAsync(stoppingToken);
//                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Her dakika kontrol et
//            }
//        }

//        private async Task UpdateStatusAsync(CancellationToken cancellationToken)
//        {
//            var now = DateTime.Now;
//            var oturumsToUpdate = await _quizContext.Oturums
//                .Where(o => o.Status == 1 && o.Date.AddMinutes(10) <= now)
//                .ToListAsync(cancellationToken);

//            foreach (var oturum in oturumsToUpdate)
//            {
//                oturum.Status = 0;
//                _quizContext.Oturums.Update(oturum);
//            }

//            await _quizContext.SaveChangesAsync(cancellationToken);
//        }

//        private async Task QuizModControl(CancellationToken cancellationToken)
//        {
//            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
//        }
//    }

//}
