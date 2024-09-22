namespace Quiz.WebUI.Entities
{
    public class AnswerQuestion
    {
        public int Id { get; set; }
        public int Answer { get; set; }
        public string UserToken { get; set; }

        public int Munite { get; set; }
        public int QuestionToken { get; set; }

        public int OturumId { get; set; }

    }
}
