namespace KutuphaneProject.Models
{
    public class Ogrenci
    {
        public int Id { get; set; }

        public string OgrenciNo { get; set; }

        public string AdSoyad { get; set; }

        public int BorcMiktari { get; set; } = 0;
    }
}
