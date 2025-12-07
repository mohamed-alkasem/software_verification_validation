using KutuphaneProject.Models;

namespace KutuphaneProject.Services.Interfaces
{
    public interface IMesajService
    {
        Task<List<Mesaj>> GetMesajlar();

        IQueryable<Mesaj> GetMesajlarForAdmin();

        Task<List<Mesaj>> GetMesajlarByOgrencId(int id);

        Task MesajGonder(Mesaj mesaj);
    }
}
