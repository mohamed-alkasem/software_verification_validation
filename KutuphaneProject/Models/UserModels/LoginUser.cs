using System.ComponentModel.DataAnnotations;

namespace KutuphaneProject.UserModels
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Öğrenci No Boş Bırakalamaz!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Öğrenci No 10 karakter olmalıdır.")]
        public string OgrenciNo { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Bırakalamaz!")]
        public string OgrenciSifre { get; set; }
    }
}
