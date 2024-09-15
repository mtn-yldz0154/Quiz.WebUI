using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quiz.WebUI.Context;
using System;
using System.Linq; 
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection; 

namespace Quiz.WebUI.Helpers
{
    public class MyBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider; 

        public MyBackgroundService(ILogger<MyBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateQuiz();
                _logger.LogInformation("Running background task at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);  
            }
        }

        public async Task UpdateQuiz()
        {
            _logger.LogInformation("Updating quiz at {time}", DateTimeOffset.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<QuizContext>(); 
                var activeOturums = _db.Oturums.Where(a => a.Status == 1).ToList();

                foreach (var oturum in activeOturums)
                {
                    if (oturum.Date != null)
                    {
                        var elapsedTime = DateTime.Now - oturum.Date; 

                        if (elapsedTime > TimeSpan.FromMinutes(oturum.Munite+1))
                        {
                            oturum.Status = 0;
                            _logger.LogInformation("Oturum {id} has been set to inactive due to timeout.", oturum.Id);
                        }
                    }
                    _db.Oturums.Update(oturum);
                }

                await _db.SaveChangesAsync();
            }
        }
    }
}
