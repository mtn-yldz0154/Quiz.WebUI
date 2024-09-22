namespace Quiz.WebUI.Entities
{
    public class ContestantScores
    {
        public int Id { get; set; }
        public int ContestantId { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
    }
}
