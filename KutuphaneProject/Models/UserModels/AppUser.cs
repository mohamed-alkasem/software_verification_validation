using Microsoft.AspNetCore.Identity;

namespace KutuphaneProject.UserModels
{
    public class AppUser : IdentityUser
    {
        public string OgrenciNo { get; set; }

        public string OgrenciAdSoyad { get; set; }

        public string TCNo { get; set; }
    }
}
