using KutuphaneProject.Models;

namespace KutuphaneProject.Services.Interfaces
{
    public interface IKitapService
    {
        Task<List<Kitap>> GetKitaplar();

        Task<List<Kitap>> GetKitaplarByKategoriId(int id);

        Task<List<Kitap>> GetKitaplarByMetin(int kategoriId, string aramaMetni);

        Task<Kitap> GetKitapById(int id);

        Task KitapEkle(Kitap kitap);

        Task KitapGuncelle(Kitap kitap);

        Task KitapSil(int id);
    }
}
