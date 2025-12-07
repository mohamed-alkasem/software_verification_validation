using KutuphaneProject.Models;
using Microsoft.AspNetCore.Mvc;
using KutuphaneProject.UserModels;
using Microsoft.AspNetCore.Identity;
using KutuphaneProject.Models.MesajList;
using KutuphaneProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace KutuphaneProject.Areas.Adminler.Controllers
{
    [Area("Adminler")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMesajService _mesajService;
        private readonly IKitapService _kitapService;
        private readonly IKategoriService _kategoriService;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(SignInManager<AppUser> signInManager,
            IKategoriService kategoriService, IMesajService mesajService,IKitapService kitapService)
        {
            _mesajService = mesajService;
            _kitapService = kitapService;
            _signInManager = signInManager;
            _kategoriService = kategoriService;
        }

        //AnasayfaAdmin---------------------------------------
        public IActionResult AnasayfaAdmin()
        {
            return View();
        }
        //----------------------------------------------------

        //KategoriEkle----------------------------------------
        public IActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KategoriEkle(Kategori model)
        {
            if (model.clientFile == null || model.clientFile.Length == 0)
            {
                ModelState.AddModelError("clientFile", "Lütfen bir dosya seçiniz.");
                return View(model);
            }
            if (model.clientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                model.clientFile.CopyTo(stream);
                model.Image = stream.ToArray();
            }
            if(model.Image != null)
            {
                _kategoriService.KategoriEkle(model);
                TempData["SuccessMessage"] = "Kategori başarıyla eklendi!";
                ModelState.Clear();
                return View();
            }
            return View(model);
        }
        //----------------------------------------------------

        //KitapEkle-------------------------------------------
        public async Task<IActionResult> KitapEkle()
        {
            var kategoriler = await _kategoriService.GetKategoriler();
            ViewBag.Kategoriler = kategoriler;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KitapEkle(Kitap model)
        {
            var kategoriler = await _kategoriService.GetKategoriler();
            ViewBag.Kategoriler = kategoriler;

            if (model.clientFile == null || model.clientFile.Length == 0)
            {
                ModelState.AddModelError("clientFile", "Lütfen bir dosya seçiniz.");
                return View(model);
            }
            if (model.clientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                model.clientFile.CopyTo(stream);
                model.Image = stream.ToArray();
            }
            if (model.Image != null)
            {
                model.EklemeTarihi = DateTime.Now;
                await _kitapService.KitapEkle(model);
                TempData["SuccessMessage"] = "Kitap başarıyla eklendi!";
                ModelState.Clear();
                return View();
            }
            ViewBag.Kategoriler = await _kategoriService.GetKategoriler();
            return View(model);
        }
        //----------------------------------------------------

        //KategoriDuzelt--------------------------------------
        public async Task<IActionResult> KategoriDuzelt(int kategoriId)
        {
            var kategori = await _kategoriService.GetKategoriById(kategoriId);

            if (kategori == null) return NotFound();

            return View(kategori);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriDuzelt(Kategori model)
        {
            if (model == null) return NotFound();

            if (model.clientFile != null && model.clientFile.Length > 0)
            {
                MemoryStream stream = new MemoryStream();
                model.clientFile.CopyTo(stream);
                model.Image = stream.ToArray();
            }
            if (model.Image != null)
            {
                await _kategoriService.KategoriGuncelle(model);
                TempData["SuccessMessage"] = "Kategori başarıyla düzenlendi!";
                ModelState.Clear();
                return RedirectToAction("KategoriDuzelt", new { kategoriId = model.Id });
            }
            return View(model);
        }
        //----------------------------------------------------

        //KitapDuzelt-----------------------------------------
        public async Task<IActionResult> KitapDuzelt(int kitapId)
        {
            var kitap = await _kitapService.GetKitapById(kitapId);

            if (kitap == null) return NotFound();

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> KitapDuzelt(Kitap model)
        {
            if (model == null) return NotFound();

            if (model.clientFile != null && model.clientFile.Length > 0)
            {
                MemoryStream stream = new MemoryStream();
                model.clientFile.CopyTo(stream);
                model.Image = stream.ToArray();
            }
            if (model.Image != null)
            {
                await _kitapService.KitapGuncelle(model);
                TempData["SuccessMessage"] = "Kitap başarıyla düzenlendi!";
                ModelState.Clear();
                return RedirectToAction("KitapDuzelt", new { kitapId = model.Id });
            }
            return View(model);
        }
        //----------------------------------------------------

        //KitapSil--------------------------------------------
        [HttpGet]
        public async Task<IActionResult> KitapSil(int kitapId)
        {
            var kitap = await _kitapService.GetKitapById(kitapId);
            if (kitap == null) return NotFound();
            await _kitapService.KitapSil(kitapId);
            TempData["SuccessMessage"] = "Kitap başarıyla silindi.";

            return RedirectToAction("KitaplarAdmin", new { kategoriId = kitap.KategoriId});
        }
        //----------------------------------------------------

        //KitaplarAdmin---------------------------------------
        public async Task<IActionResult> KitaplarAdmin(int kategoriId)
        {
            var kategori = await _kategoriService.GetKategoriById(kategoriId);
            if (kategori == null) return NotFound();

            var kitaplar = await _kitapService.GetKitaplarByKategoriId(kategoriId);
            if (kitaplar == null) return NotFound();

            return View(kitaplar);
        }
        //----------------------------------------------------

        //Mesajlar--------------------------------------------
        public IActionResult Mesajlar()
        {
            return View();
        }

        [HttpPost]
        public PagedListDto<Mesaj> MesajListele(MesajListele input)
        {
            var parameters = Request.Form.ToList();

            var mesajlar = _mesajService.GetMesajlarForAdmin();

            var toplamKayitSayisi = mesajlar.Count();

            mesajlar = mesajlar
               .WhereIf(!string.IsNullOrEmpty(input.AdSoyad), x => x.AdSoyad.ToLower().Contains(input.AdSoyad.ToLower()))
               .WhereIf(!string.IsNullOrEmpty(input.Eposta), x => x.Eposta.ToLower().Contains(input.Eposta.ToLower()))
               .WhereIf(!string.IsNullOrEmpty(input.Icerik), x => x.Icerik.ToLower().Contains(input.Icerik.ToLower()))
               .WhereIf(input.Search is not null &&
                   !string.IsNullOrEmpty(input.Search.Value),
                   x => x.AdSoyad.ToLower().Contains(input.Search!.Value.ToLower())
                   || x.Eposta.ToLower().Contains(input.Search.Value.ToLower())
                   || x.Icerik.ToLower().Contains(input.Search.Value.ToLower()));

            var filtrelenmisKayitSayisi = mesajlar.Count();
            var sonuc = mesajlar
                .Skip(input.Start)
                .Take(input.Length)
                .ToList();

            return new PagedListDto<Mesaj>
            {
                Data = sonuc,
                Draw = input.Draw,
                RecordsTotal = toplamKayitSayisi,
                RecordsFiltered = filtrelenmisKayitSayisi
            };
        }
        //----------------------------------------------------
    }
}
