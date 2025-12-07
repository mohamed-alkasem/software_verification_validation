using System.ComponentModel.DataAnnotations;

namespace KutuphaneProject.Models.UserModels
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "TC No gerekli!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC No 11 rakam olmalıdır.")]
        public string TCNo { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre gerekli!")]
        public string Sifre { get; set; }
    }
}
