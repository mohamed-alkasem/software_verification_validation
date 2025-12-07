using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject.Services.Classes
{
    public class OgrenciService : IOgrenciService
    {
        private readonly KutuphaneDbContext _kutuphaneDbContext;

        public OgrenciService(KutuphaneDbContext kutuphaneDbContext)
        {
            _kutuphaneDbContext = kutuphaneDbContext;
        }

        //----------------------------------------------------
        public async Task<Ogrenci> GetOgrenciById(int ogrenciId)
        {
            var ogrenci = await _kutuphaneDbContext.Ogrenciler
                .Where(x => x.Id == ogrenciId)
                .FirstOrDefaultAsync();

            if (ogrenci != null)
            {
                return ogrenci;
            }
            else
            {
                return new Ogrenci();
            }            
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<Ogrenci> GetOgrenciByOgrenciNo(string ogrenciNo)
        {
            var ogrenci = await _kutuphaneDbContext.Ogrenciler
                .Where(x => x.OgrenciNo == ogrenciNo)
                .FirstOrDefaultAsync();

            if (ogrenci != null)
            {
                return ogrenci;
            }
            else
            {
                return new Ogrenci();
            }
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Ogrenci>> GetOgrenciler()
        {
            var ogrenciler = await _kutuphaneDbContext.Ogrenciler.ToListAsync();

            return ogrenciler;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task OgrenciEkle(Ogrenci ogrenci)
        {
            await _kutuphaneDbContext.Ogrenciler.AddAsync(ogrenci);
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task OgrenciyiGuncelle(Ogrenci ogrenci)
        {
            var guncellenecekOgrenci = await _kutuphaneDbContext.Ogrenciler.FirstOrDefaultAsync(x => x.Id == ogrenci.Id);

            if (guncellenecekOgrenci is not null)
            {
                guncellenecekOgrenci.Id = ogrenci.Id;
                guncellenecekOgrenci.AdSoyad = ogrenci.AdSoyad;
                guncellenecekOgrenci.BorcMiktari = ogrenci.BorcMiktari;
            }
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------
    }
}
