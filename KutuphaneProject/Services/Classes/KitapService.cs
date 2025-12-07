using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject.Services.Classes
{
    public class KitapService : IKitapService
    {
        private readonly KutuphaneDbContext _kutuphaneDbContext;

        public KitapService(KutuphaneDbContext kutuphaneDbContext)
        {
            _kutuphaneDbContext = kutuphaneDbContext;
        }

        //----------------------------------------------------
        public async Task<Kitap> GetKitapById(int id)
        {
            var kitap = await _kutuphaneDbContext.Kitaplar.FirstOrDefaultAsync(x => x.Id == id);

            if (kitap == null)
            {
                return new Kitap();
            }
            else
            {
                return kitap;
            }
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Kitap>> GetKitaplarByMetin(int kategoriId, string aramaMetni) 
        {
            var aramaSonuclari = await _kutuphaneDbContext.Kitaplar
                .Where(k => k.KategoriId == kategoriId && k.Ad.Contains(aramaMetni))
                .ToListAsync();

            return aramaSonuclari;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Kitap>> GetKitaplar()
        {
            var kitaplar = await _kutuphaneDbContext.Kitaplar.ToListAsync();

            return kitaplar;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Kitap>> GetKitaplarByKategoriId(int id)
        {
            var kitaplar = await _kutuphaneDbContext.Kitaplar
                .Where(x => x.KategoriId == id)
                .ToListAsync();

            return kitaplar;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task KitapEkle(Kitap kitap)
        {
            await _kutuphaneDbContext.Kitaplar.AddAsync(kitap);
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task KitapGuncelle(Kitap kitap)
        {
            var guncellenecekKitap = await _kutuphaneDbContext.Kitaplar.FirstOrDefaultAsync(x => x.Id == kitap.Id);

            if (guncellenecekKitap is not null)
            {
                guncellenecekKitap.Id = kitap.Id;
                guncellenecekKitap.Ad = kitap.Ad;
                guncellenecekKitap.Image = kitap.Image;
                guncellenecekKitap.Aciklama = kitap.Aciklama;
                guncellenecekKitap.KategoriFK = kitap.KategoriFK;
            }

            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task KitapSil(int id)
        {
            var silenecekKitap = _kutuphaneDbContext.Kitaplar.Find(id);

            if (silenecekKitap is null) return;

            _kutuphaneDbContext.Kitaplar.Remove(silenecekKitap);
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------
    }
}
