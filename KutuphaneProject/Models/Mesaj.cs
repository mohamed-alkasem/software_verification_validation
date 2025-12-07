using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneProject.Models
{
    public class Mesaj
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad Boş Bırakalamaz!")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Mesaj Boş Bırakalamaz!")]
        public string Icerik { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Eposta Boş Bırakalamaz!")]
        public string Eposta { get; set; }

        public int OgrenciId { get; set; }
        [ForeignKey("OgrenciId")]
        public Ogrenci OgrenciFK { get; set; }

        public DateTime MesajTarihi { get; set; }
    }
}
