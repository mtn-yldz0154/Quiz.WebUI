namespace Quiz.WebUI.Models
{
    public class QuizResultModel
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public int Skor { get; set; }

        public int Time { get; set; }
        public int Answer { get; set; }

        public int QuestionType { get; set; }
        public int Munite { get; set; }

        public int QuestionId { get; set; }
    }
}
