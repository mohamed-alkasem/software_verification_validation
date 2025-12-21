using KutuphaneProject.Models;
using KutuphaneProject.Models.UserModels;
using KutuphaneProject.Services.Classes;
using KutuphaneProject.Services.Interfaces;
using KutuphaneProject.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneProject.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly IOduncService _oduncService;
        private readonly IKitapService _kitapService;
        private readonly IMesajService _mesajService;
        private readonly IOgrenciService _ogrenciService;
        private readonly IKategoriService _kategoriService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public OgrenciController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            IOgrenciService ogrenciService, IMesajService mesajService, IKategoriService kategoriService,
            IKitapService kitapService, IOduncService oduncService, GecikmeKontrolService gecikmeKontrolService)
        {
            _userManager = userManager;
            _oduncService = oduncService;
            _kitapService = kitapService;
            _mesajService = mesajService;
            _signInManager = signInManager;
            _ogrenciService = ogrenciService;
            _kategoriService = kategoriService;
        }

        //Index-----------------------------------------------
        public async Task<IActionResult> Index()
        {           
            var kitaplar = await _kitapService.GetKitaplar();
            return View(kitaplar);
        }
        //----------------------------------------------------

        //OgrenciGiris----------------------------------------
        public IActionResult OgrenciGiris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OgrenciGiris(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı OgrenciNo ile arama
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OgrenciNo == model.OgrenciNo);

                // Eğer kullanıcı bulunmazsa    
                if (user == null)
                {
                    ModelState.AddModelError("", "Öğrenci No bulunamadı.");
                    return View(model); //girilen bilgileri yanlışsa inputları boşaltmasın
                }

                // Kullanıcı bulunduysa, şifreyi kontrol et
                var result = await _signInManager.PasswordSignInAsync(
                    model.OgrenciNo!, model.OgrenciSifre!, true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Şifre hatalı.");
                    return View(model);
                }
            }
            return View(model);
        }
        //----------------------------------------------------

        //OgrenciUye------------------------------------------
        public IActionResult OgrenciUye()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OgrenciUye(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    UserName = model.OgrenciNo, // Kullanıcı adı olarak OgrenciNo kullan
                    OgrenciNo = model.OgrenciNo,
                    OgrenciAdSoyad = model.OgrenciAdSoyad,
                    TCNo = model.OgrenciNo
                };

                var result = await _userManager.CreateAsync(user, model.OgrenciSifre!);
                if (result.Succeeded)
                {
                    //Ogrenciler tablosuna ogrenci ekleme---------------------
                    var eklenecekOgrenci = new Ogrenci
                    {
                        OgrenciNo = model.OgrenciNo!,
                        AdSoyad = model.OgrenciAdSoyad!,
                        BorcMiktari = 0
                    };
                    await _ogrenciService.OgrenciEkle(eklenecekOgrenci);
                    //---------------------------------------------------------
                    TempData["SuccessMessage"] = "Üyelik başarıyla tamamlandı!";
                    ModelState.Clear();
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        //----------------------------------------------------

        //AdminGiris------------------------------------------
        public IActionResult AdminGiris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminGiris(AdminLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Admin TCNo'ya göre bulunuyor
                var user = await _signInManager.UserManager.Users.FirstOrDefaultAsync(u => u.TCNo == model.TCNo);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Sifre!, true, false);
                    if (result.Succeeded && await _signInManager.UserManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("AnasayfaAdmin", "Admin", new { area = "Adminler" });
                    }
                    ModelState.AddModelError("", "Şifre hatalı.");
                }
                else
                {
                    ModelState.AddModelError("", "TC No veya Şifre hatalı.");
                }
            }
            return View(model);
        }
        //----------------------------------------------------

        //SifreUnuttum----------------------------------------
        public IActionResult SifreUnuttum()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SifreUnuttum(SifreUnuttum model)
        {
            if (ModelState.IsValid)
            {
                // Öğrenci No ile kullanıcıyı bul
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OgrenciNo == model.OgrenciNo);
                if (user == null)
                {
                    ModelState.AddModelError("", "Öğrenci bulunamadı.");
                    return View(model);
                }

                // Şifre kontrolü
                var isPasswordMatch = await _userManager.CheckPasswordAsync(user, model.YeniSifre);
                if (isPasswordMatch)
                {
                    TempData["WarningMessage"] = "Yeni şifre, mevcut şifre ile aynı olamaz!";
                    return View(model);
                }

                // Şifre sıfırlama işlemi
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, model.YeniSifre);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                    ModelState.Clear();
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        //----------------------------------------------------

        //Kategori--------------------------------------------
        public async Task<IActionResult> Kategori(int kategoriId)
        {
            var kategori = await _kategoriService.GetKategoriById(kategoriId);
            if (kategori == null) return NotFound();

            var kitaplar = await _kitapService.GetKitaplarByKategoriId(kategoriId);
            if (kitaplar == null) return NotFound();
            ViewBag.KategoriAdi = kategori.Ad;
            ViewBag.KategoriId = kategori.Id;

            return View(kitaplar);
        }
        //----------------------------------------------------

        //KitapOgrenci----------------------------------------
        public async Task<IActionResult> KitapOgrenci(int kitapId)
        {
            var kitap = await _kitapService.GetKitapById(kitapId);
            if (kitap == null) return NotFound();

            return View(kitap);
        }
        //----------------------------------------------------

        //OduncAl---------------------------------------------
        [HttpGet]
        public async Task<IActionResult> OduncAl(int kitapId)
        {
            TempData["ErrorMessage"] = "Lütfen kitabı ödünç almadan önce iade tarihini seçiniz.";
            return RedirectToAction("KitapOgrenci", new { kitapId });
        }

        [HttpPost]
        public async Task<IActionResult> OduncAl(int kitapId, DateTime geriDonusTarihi)
        {
            var kitap = await _kitapService.GetKitapById(kitapId);
            if (kitap == null) return NotFound();

            if (geriDonusTarihi.Date <= DateTime.Today)
            {
                TempData["ErrorMessage"] = "İade tarihi bugünden ileri bir tarih olmalıdır.";
                return RedirectToAction("KitapOgrenci", new { kitapId = kitap.Id });
            }

            var ogrenciNo = User.Identity!.Name;  //giriş yapan kişinin öğrenci numarasını sakla
            if (ogrenciNo is not null)
            {
                var ogrenci = await _ogrenciService.GetOgrenciByOgrenciNo(ogrenciNo);
                if (ogrenci is not null)
                {
                    var kalanSure = (geriDonusTarihi.Date - DateTime.Today).Days;
                    var odunc = new Odunc()
                    {
                        OgrenciId = ogrenci.Id,
                        Name = kitap.Ad,
                        KitapId = kitap.Id,
                        KartNo = "",
                        KalanSure = kalanSure,
                        BorcMiktari = 0,
                        AlinmaTarihi = DateTime.Now,
                        KontrolTarihi = DateTime.Now,
                        GeriDonusTarihi = geriDonusTarihi.Date,
                    };

                    await _oduncService.OduncEkle(odunc);
                    TempData["SuccessMessage"] = $"Ödünç başarıyla alındı! Son teslim tarihi: {geriDonusTarihi:dd.MM.yyyy}";
                    return RedirectToAction("KitapOgrenci", new { kitapId = kitap.Id });
                }
            }

            TempData["ErrorMessage"] = "Ödünç alma sırasında bir hata oluştu.";
            return RedirectToAction("KitapOgrenci", new { kitapId = kitap.Id });
        }
        //----------------------------------------------------

        //Odunclerim------------------------------------------
        public async Task<IActionResult> Odunclerim()
        {
            var ogrenciNo = User.Identity!.Name;

            if (ogrenciNo is not null)
            {
                var ogrenci = await _ogrenciService.GetOgrenciByOgrenciNo(ogrenciNo);
                if (ogrenci is not null)
                {
                    ViewBag.ogrenciBorcu = ogrenci.BorcMiktari;
                    var oduncler = await _oduncService.GetOdunclerByOgrenciId(ogrenci.Id);
                    if (oduncler is not null)
                    {
                        var odunclerDto = new List<OduncDto>();
                        foreach (var item in oduncler)
                        {
                            var kitap = await _kitapService.GetKitapById(item.KitapId);
                            var odunc = new OduncDto()
                            {
                                OduncId = item.Id,
                                OduncAdi = kitap?.Ad ?? item.Name,
                                BorcMiktari = item.BorcMiktari,
                                Image = kitap?.Image,
                                KalanSure = item.KalanSure
                            };
                            odunclerDto.Add(odunc);
                        }
                        return View(odunclerDto);
                    }

                }
            }
            return RedirectToAction("Index");
        }
        //----------------------------------------------------

        //OdemeYap--------------------------------------------
        public async Task<IActionResult> OdemeYap(int oduncId)
        {
            var odunc = await _oduncService.GetOduncById(oduncId);

            if (odunc == null) return View(oduncId);

            return View(odunc);
        }

        [HttpPost]
        public async Task<IActionResult> OdemeYap(Odunc model)
        {
            if (model.KartNo is not null)
            {
                long sonucNumber;
                bool kartNoRakamlardanMiOlusuyor = long.TryParse(model.KartNo, out sonucNumber);
                if (kartNoRakamlardanMiOlusuyor) 
                {
                    if (model.KartNo.Length == 16)
                    {
                        await _oduncService.OdemeYap(model.Id);
                        TempData["SuccessMessage"] = "Ödünç başarıyla iade edildi!";
                        return RedirectToAction("Odunclerim");
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        //----------------------------------------------------

        //OdemeYap--------------------------------------------
        [HttpGet]
        public async Task<IActionResult> OduncIade(int oduncId)
        {
            if (ModelState.IsValid)
            {
                await _oduncService.OduncIade(oduncId);
                TempData["SuccessMessage"] = "Ödünç başarıyla iade edildi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
            }
            return RedirectToAction("Odunclerim");
        }
        //----------------------------------------------------

        //Kategoriler-----------------------------------------
        public async Task<IActionResult> Kategoriler()
        {
            var kategoriler = await _kategoriService.GetKategoriler();
            return View(kategoriler);
        }
        //----------------------------------------------------

        //AramaSonuclari--------------------------------------
        public async Task<IActionResult> AramaSonuclari(int kategoriId, string aramaMetni)
        {
            var aramaSonuclari = await _kitapService.GetKitaplarByMetin(kategoriId ,aramaMetni);

            return View(aramaSonuclari);
        }
        //----------------------------------------------------

        //Hakkinda--------------------------------------------
        public IActionResult Hakkinda()
        {
            return View();
        }
        //----------------------------------------------------

        //Iletisim--------------------------------------------
        public IActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Iletisim(Mesaj mesaj)
        {
            var ogrenciNo = User.Identity!.Name;
            if (ogrenciNo != null)
            {
                var ogrenci = await _ogrenciService.GetOgrenciByOgrenciNo(ogrenciNo);
                if (ogrenci != null)
                {
                    mesaj.OgrenciId = ogrenci.Id;
                    mesaj.MesajTarihi = DateTime.Now;
                    await _mesajService.MesajGonder(mesaj);
                    TempData["SuccessMessage"] = "Form başarıyla gönderildi!";
                    return RedirectToAction("Iletisim");
                }
            }
            return View(mesaj);
        }  
        //----------------------------------------------------

        //Mesajlar--------------------------------------------
        public async Task<IActionResult> Mesajlar()
        {
            var ogrenciNo = User.Identity!.Name;
            if (ogrenciNo != null)
            {
                var ogrenci = await _ogrenciService.GetOgrenciByOgrenciNo(ogrenciNo);
                if (ogrenci is not null)
                {
                    var mesajlar = await _mesajService.GetMesajlarByOgrencId(ogrenci.Id);
                    return View(mesajlar);
                }
            }
            return View();
        }
        //----------------------------------------------------

        //CikisYap--------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CikisYap()
        {
            var ogrenciNo = User.Identity!.Name;

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Ogrenci", null);
        }
        //----------------------------------------------------
    }
}