namespace Quiz.WebUI
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class QuizHub : Hub
    {
        // Admin quiz'i başlattığında bu metodu çağırır
        public async Task SendQuizStart()
        {
            // Tüm bağlı kullanıcılara quiz'in başladığını bildirir
            await Clients.All.SendAsync("ReceiveQuizStart");
        }
    }

}
