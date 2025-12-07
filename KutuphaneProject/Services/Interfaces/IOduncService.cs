using KutuphaneProject.Models;

namespace KutuphaneProject.Services.Interfaces
{
    public interface IOduncService
    {
        Task<List<Odunc>> GetOduncler();

        Task<Odunc> GetOduncById(int id);
        
        Task<List<Odunc>> GetOdunclerByOgrenciId(int id);

        Task OduncuyuGuncelle(Odunc odunc);

        Task OduncEkle(Odunc odunc);

        Task OduncIade(int id);

        Task OdemeYap(int id);
    }
}
