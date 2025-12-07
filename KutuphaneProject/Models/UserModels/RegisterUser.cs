using System.ComponentModel.DataAnnotations;

namespace KutuphaneProject.UserModels
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Öğrenci No Boş Bırakalamaz!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Öğrenci No 10 rakam olmalıdır.")]
        public string OgrenciNo { get; set; }

        [Required(ErrorMessage = "Ad ve Soyad Boş Bırakalamaz!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "karakter sayısı 6-20 arasında olmalıdır.")]
        public string OgrenciAdSoyad { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Bırakalamaz!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Şifre uzunluğu 8 ile 20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,20}$", ErrorMessage = "Şifre en az bir küçük harf, bir büyük harf ve bir rakam içermelidir.")]
        public string OgrenciSifre { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("OgrenciSifre", ErrorMessage = "Şifreler birbiriyle eşleşmiyor.")]
        public string OgrenciSifreTekrar { get; set; }
    }
}
