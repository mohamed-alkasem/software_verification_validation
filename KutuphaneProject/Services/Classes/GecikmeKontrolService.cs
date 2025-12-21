using KutuphaneProject.Services.Interfaces;
using KutuphaneProject.Models;
using Microsoft.Extensions.Logging;

namespace KutuphaneProject.Services.Classes
{
    public class GecikmeKontrolService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GecikmeKontrolService> _logger;

        public GecikmeKontrolService(IServiceProvider serviceProvider, ILogger<GecikmeKontrolService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
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
                                    // Sadece henüz işlenmemiş gecikme günlerini hesapla
                                    var sonKontrolTarihi = odunc.KontrolTarihi;
                                    var yeniGecikmeGunleri = (DateTime.Now - sonKontrolTarihi).Days;
                                    
                                    if (yeniGecikmeGunleri > 0)
                                    {
                                        // Günlük 1 TL gecikme ücreti ekle
                                        odunc.BorcMiktari += yeniGecikmeGunleri;
                                        odunc.KontrolTarihi = DateTime.Now;
                                        
                                        // Öğrencinin toplam borcunu güncelle
                                        var ogrenci = await ogrenciService.GetOgrenciById(odunc.OgrenciId);
                                        if (ogrenci != null)
                                        {
                                            ogrenci.BorcMiktari += yeniGecikmeGunleri;
                                            await ogrenciService.OgrenciyiGuncelle(ogrenci);
                                            _logger.LogInformation($"{ogrenci.AdSoyad} için {yeniGecikmeGunleri} günlük gecikme ücreti eklendi. Toplam borç: {ogrenci.BorcMiktari} TL");
                                        }
                                        
                                        await oduncService.OduncuyuGuncelle(odunc);
                                    }
                                }
                            }
                            
                            // Kalan süreyi güncelle
                            if (odunc.KalanSure > 0 && odunc.KontrolTarihi.Date < DateTime.Today)
                            {
                                odunc.KalanSure = Math.Max(0, odunc.KalanSure - 1);
                                await oduncService.OduncuyuGuncelle(odunc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gecikme kontrolü sırasında bir hata oluştu");
            }
        }
    }
}
