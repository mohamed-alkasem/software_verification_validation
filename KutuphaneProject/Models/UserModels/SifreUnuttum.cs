using System.ComponentModel.DataAnnotations;

namespace KutuphaneProject.Models.UserModels
{
    public class SifreUnuttum
    {
        [Required(ErrorMessage = "Öğrenci Numarası gereklidir.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Öğrenci numarası tam olarak 10 karakter olmalıdır.")]
        public string OgrenciNo { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifre boş bırakılamaz.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Şifre uzunluğu 8 ile 20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,20}$", ErrorMessage = "Şifre en az bir küçük harf, bir büyük harf ve bir rakam içermelidir.")]
        public string YeniSifre { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre tekrarı gereklidir.")]
        [Compare("YeniSifre", ErrorMessage = "Şifreler birbiriyle eşleşmiyor.")]
        public string SifreTekrar { get; set; }
    }
}
