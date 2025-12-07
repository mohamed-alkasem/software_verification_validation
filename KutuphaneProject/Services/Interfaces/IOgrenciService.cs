using KutuphaneProject.Models;

namespace KutuphaneProject.Services.Interfaces
{
    public interface IOgrenciService
    {
        Task<List<Ogrenci>> GetOgrenciler();

        Task<Ogrenci> GetOgrenciByOgrenciNo(string ogrenciNo);

        Task<Ogrenci> GetOgrenciById(int ogrenciId);

        Task OgrenciyiGuncelle(Ogrenci ogrenci);

        Task OgrenciEkle(Ogrenci ogrenci);
    }
}
