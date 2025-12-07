using KutuphaneProject.EFCore;
using KutuphaneProject.Models;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject.Services.Classes
{
    public class KategoriService : IKategoriService
    {
        private readonly KutuphaneDbContext _kutuphaneDbContext;

        public KategoriService(KutuphaneDbContext kutuphaneDbContext)
        {
            _kutuphaneDbContext = kutuphaneDbContext;
        }

        //----------------------------------------------------
        public async Task<Kategori> GetKategoriById(int id)
        {
            var kategori = await _kutuphaneDbContext.Kategoriler.FirstOrDefaultAsync(x => x.Id == id);

            if (kategori == null)
            {
                return new Kategori();
            }
            else
            {
                return kategori;
            }
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task<List<Kategori>> GetKategoriler()
        {
            var kategoriler = await _kutuphaneDbContext.Kategoriler.ToListAsync();

            return kategoriler;
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task KategoriEkle(Kategori kategori)
        {
            await _kutuphaneDbContext.Kategoriler.AddAsync(kategori);
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public async Task KategoriGuncelle(Kategori kategori)
        {
            var guncellenecekKategori = await _kutuphaneDbContext.Kategoriler.FirstOrDefaultAsync(x => x.Id == kategori.Id);

            if (guncellenecekKategori is not null)
            {
                guncellenecekKategori.Id = kategori.Id;
                guncellenecekKategori.Ad = kategori.Ad;
                guncellenecekKategori.Image = kategori.Image;
            }
            await _kutuphaneDbContext.SaveChangesAsync();
        }
        //----------------------------------------------------

        //----------------------------------------------------
        public void KategoriSil(int id)
        {
            var silenecekKategori = _kutuphaneDbContext.Kategoriler.Find(id);
            if (silenecekKategori != null)
            {
                _kutuphaneDbContext.Kategoriler.Remove(silenecekKategori);
                _kutuphaneDbContext.SaveChanges();
            }
        }
        //----------------------------------------------------
    }
}
