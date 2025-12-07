using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject.Services.Classes
{
    public class OduncService : IOduncService
    {
        private readonly KutuphaneDbContext _kutuphaneDbContext;

        public OduncService(KutuphaneDbContext kutuphaneDbContext)
        {
            _kutuphaneDbContext = kutuphaneDbContext;
        }

        //----------------------------------------------------
        public async Task<Odunc> GetOduncById(int id)
        {
            var odunc = await _kutuphaneDbContext.Oduncler.FirstOrDefaultAsync(x => x.Id == id);

            if (odunc != null)
            {
                return odunc;
            }
            else
            {
                return new Odunc();
            }
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Odunc>> GetOduncler()
        {
            var oduncler = await _kutuphaneDbContext.Oduncler.ToListAsync();

            return oduncler;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Odunc>> GetOdunclerByOgrenciId(int id)
        {
            var oduncler = await _kutuphaneDbContext.Oduncler
                .Where(x => x.OgrenciId == id)
                .ToListAsync();

            return oduncler;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task OduncEkle(Odunc odunc)
        {
            await _kutuphaneDbContext.Oduncler.AddAsync(odunc);
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task OdemeYap(int id)
        {
            var iadeEdilecekOdunc = await _kutuphaneDbContext.Oduncler.FirstOrDefaultAsync(x => x.Id == id);

            if (iadeEdilecekOdunc is not null)
            {
                var OduncAlinanOgrenci = await _kutuphaneDbContext.Ogrenciler
                .Where(x => x.Id == iadeEdilecekOdunc.OgrenciId)
                .FirstOrDefaultAsync();

                if (OduncAlinanOgrenci is not null)
                {
                    OduncAlinanOgrenci.Id = OduncAlinanOgrenci.Id;
                    OduncAlinanOgrenci.AdSoyad = OduncAlinanOgrenci.AdSoyad;
                    OduncAlinanOgrenci.OgrenciNo = OduncAlinanOgrenci.OgrenciNo;
                    OduncAlinanOgrenci.BorcMiktari -= iadeEdilecekOdunc.BorcMiktari; //ödünç borcu öğrenci borcundan çıkar
                    await _kutuphaneDbContext.SaveChangesAsync();
                }
                _kutuphaneDbContext.Oduncler.Remove(iadeEdilecekOdunc);
                await _kutuphaneDbContext.SaveChangesAsync();
            }
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task OduncIade(int id)
        {
            var iadeEdilecekOdunc = await _kutuphaneDbContext.Oduncler.FirstOrDefaultAsync(x => x.Id == id);

            if (iadeEdilecekOdunc is not null)
            {
                _kutuphaneDbContext.Oduncler.Remove(iadeEdilecekOdunc);
                await _kutuphaneDbContext.SaveChangesAsync();
            }
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task OduncuyuGuncelle(Odunc odunc)
        {
            var guncellenecekOdunc = await _kutuphaneDbContext.Oduncler.FirstOrDefaultAsync(x => x.Id == odunc.Id);

            if (guncellenecekOdunc is not null)
            {
                guncellenecekOdunc.Id = odunc.Id;
                guncellenecekOdunc.Name = odunc.Name;
                guncellenecekOdunc.KalanSure = odunc.KalanSure;
                guncellenecekOdunc.BorcMiktari = odunc.BorcMiktari;
                guncellenecekOdunc.GeriDonusTarihi = odunc.GeriDonusTarihi;
            }
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------
    }
}
