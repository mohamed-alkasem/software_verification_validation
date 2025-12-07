using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject.Services.Classes
{
    public class MesajService : IMesajService
    {
        private readonly KutuphaneDbContext _kutuphaneDbContext;

        public MesajService(KutuphaneDbContext kutuphaneDbContext)
        {
            _kutuphaneDbContext = kutuphaneDbContext;
        }

        //----------------------------------------------------
        public IQueryable<Mesaj> GetMesajlarForAdmin()
        {
            var mesajlar = _kutuphaneDbContext.Mesajlar
                .OrderBy(x => x.MesajTarihi)
                .AsQueryable();

            return mesajlar;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Mesaj>> GetMesajlar() //öğrenci için
        {
            var mesajlar = await _kutuphaneDbContext.Mesajlar
                .OrderBy(x => x.MesajTarihi)
                .ToListAsync();

            return mesajlar;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Mesaj>> GetMesajlarByOgrencId(int id)
        {
            var mesajlar = await _kutuphaneDbContext.Mesajlar
                .Where(x => x.OgrenciId == id)
                .OrderBy(x => x.MesajTarihi)
                .ToListAsync();

            return mesajlar;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task MesajGonder(Mesaj mesaj)
        {
            var gonderilecekMesaj = await _kutuphaneDbContext.Mesajlar.AddAsync(mesaj);
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------
    }
}
