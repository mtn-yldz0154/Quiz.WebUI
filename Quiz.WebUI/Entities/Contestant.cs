namespace Quiz.WebUI.Entities
{
    public class Contestant
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public int Skor { get; set; }

        public int Answer { get; set; }

        public Oturum Oturum { get; set; }
        public int OturumId { get; set; }


    }
}
