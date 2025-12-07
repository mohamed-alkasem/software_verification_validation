using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneProject.Models
{
    public class Odunc
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int KalanSure { get; set; }
            
        public int BorcMiktari { get; set; }

        public DateTime GeriDonusTarihi { get; set; }

        public DateTime AlinmaTarihi { get; set; }

        public DateTime KontrolTarihi {  get; set; }

        [Required(ErrorMessage = "Kart No Boş Bırakalamaz!")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart No 16 Rakam Olmalıdır!")]
        public string KartNo { get; set; }

        public int OgrenciId { get; set; }
        [ForeignKey("OgrenciId")]
        public Ogrenci OgrenciFK { get; set; }

        public int KitapId { get; set; }
        [ForeignKey("KitapId")]
        public Kitap KitapFK { get; set; }
    }
}
