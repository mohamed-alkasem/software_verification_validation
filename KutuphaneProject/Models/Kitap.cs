using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneProject.Models
{
    public class Kitap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap Adı Boş Bırakalamaz!")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Açıklama Boş Bırakalamaz!")]
        public string Aciklama { get; set; }

        public DateTime EklemeTarihi { get; set; }

        public int KategoriId { get; set; }
        [ForeignKey("KategoriId")]
        public Kategori KategoriFK { get; set; }

        [Required(ErrorMessage = "Resim Seçin!")]
        public byte[] Image { get; set; }

        [NotMapped] //database gitmiyor
        [Required(ErrorMessage = "Resim Seçin!")]
        public IFormFile clientFile { get; set; }

        [NotMapped]
        public string ImageSrc
        {
            get
            {
                if (Image != null)
                {
                    string base64String = Convert.ToBase64String(Image, 0, Image.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
