namespace Quiz.WebUI.Entities
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Name  { get; set; }

        public List<Questions> Questions { get; set; }
    }
}
