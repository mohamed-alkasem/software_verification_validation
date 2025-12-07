using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject.Services.Classes
{
    public class GecikmeKontrolService
    {
        private readonly IServiceProvider _serviceProvider;
        public GecikmeKontrolService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Kontrol()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var oduncService = scope.ServiceProvider.GetRequiredService<IOduncService>();
                    var ogrenciService = scope.ServiceProvider.GetRequiredService<IOgrenciService>();
                    var oduncler = await oduncService.GetOduncler();

                    if (oduncler != null)
                    {
                        foreach (var odunc in oduncler)
                        {
                            if (odunc.GeriDonusTarihi < DateTime.Now)
                            {
                                var gecikmeGunleri = (DateTime.Now - odunc.GeriDonusTarihi).Days;
                                if (gecikmeGunleri > 0)
                                {
                                    //BorcMiktari ödunçte güncel
                                    odunc.BorcMiktari += gecikmeGunleri; // Her gün 1 TL
                                    if (odunc.KalanSure > 0)
                                    {
                                        odunc.KalanSure -= 1;
                                    }
                                    await oduncService.OduncuyuGuncelle(odunc);

                                    //BorcMiktari öğrencide güncel
                                    var ogrenci = await ogrenciService.GetOgrenciById(odunc.OgrenciId);
                                    ogrenci.BorcMiktari += gecikmeGunleri; // Her gün 1 TL
                                    await ogrenciService.OgrenciyiGuncelle(ogrenci);
                                }
                            }
                            else if (odunc.KontrolTarihi.AddHours(24) == DateTime.Now)
                            {
                                odunc.KontrolTarihi = DateTime.Now;
                                await oduncService.OduncuyuGuncelle(odunc);
                                if (odunc.KalanSure > 0)
                                {
                                    odunc.KalanSure -= 1;
                                    await oduncService.OduncuyuGuncelle(odunc);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }
    }
}
