namespace Quiz.WebUI.Entities
{
    public class Oturum
    {
        public int Id { get; set; }

        public string Token { get; set; }


        // Yarışmacı Sayısı
        public int CompetitorsNumber { get; set; }

        public int Status { get; set; }


        public DateTime Date { get; set; }
        public List<Contestant> Contestants { get; set; }
    }
}
