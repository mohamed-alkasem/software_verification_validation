using KutuphaneProject.Models;

namespace KutuphaneProject.Services.Interfaces
{
    public interface IKategoriService
    {
        Task<List<Kategori>> GetKategoriler();

        Task<Kategori> GetKategoriById(int id);

        Task KategoriEkle(Kategori kategori);

        Task KategoriGuncelle(Kategori kategori);

        void KategoriSil(int id);
    }
}
