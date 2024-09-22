using Microsoft.EntityFrameworkCore;
using Quiz.WebUI.Entities;

namespace Quiz.WebUI.Context
{
    public class QuizContext:DbContext
    {
        public QuizContext()
        {
        }

        public QuizContext(DbContextOptions<QuizContext> options)
           : base(options)
        {
        }

        public DbSet<Oturum> Oturums { get; set; }
        public DbSet<Contestant> Contestants { get; set; }
        public DbSet<QuizSession> QuizSessions { get; set; }
  
        public DbSet<Questions> Questions { get; set; }
        public DbSet<AnswerQuestion> AnswerQuestions { get; set; }
        public DbSet<ContestantScores> ContestantScores { get; set; }
    }
}
