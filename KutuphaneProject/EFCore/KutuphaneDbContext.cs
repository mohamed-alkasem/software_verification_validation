using KutuphaneProject.Models;
using KutuphaneProject.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KutuphaneProject.EFCore
{
    public class KutuphaneDbContext : IdentityDbContext<AppUser> //Identity user özellikleri dışında özellikler eklemek için AppUser tanımladık
    {
        public DbSet<Ogrenci> Ogrenciler { get; set; }

        public DbSet<Kategori> Kategoriler { get; set; }

        public DbSet<Kitap> Kitaplar { get; set; }

        public DbSet<Mesaj> Mesajlar { get; set; }

        public DbSet<Odunc> Oduncler { get; set; }
       
        public KutuphaneDbContext(DbContextOptions<KutuphaneDbContext> options) : base(options)
        {

        }
    }
}
