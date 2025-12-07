using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneProject.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori Adı Boş Bırakalamaz!")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Resim Seçin!")]
        public byte[] Image { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Resim Seçin!")]
        public IFormFile clientFile { get; set; }

        [NotMapped]
        public string ImageSrc
        {
            get
            {
                if (Image != null)
                {
                    string base64String = Convert.ToBase64String(Image);
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
