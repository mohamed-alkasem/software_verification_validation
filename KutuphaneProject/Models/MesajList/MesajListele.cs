namespace KutuphaneProject.Models.MesajList
{
    public class MesajListele
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public int Draw { get; set; }

        public string AdSoyad { get; set; }
        public string Icerik { get; set; }
        public string Eposta { get; set; }
        public DateTime MesajTarihi { get; set; }

        public SearchDto Search { get; set; }
    }
}
