using KutuphaneProject.EFCore;
using Microsoft.Net.Http.Headers;
using KutuphaneProject.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.Services.Classes;
using Microsoft.AspNetCore.Authentication;
using KutuphaneProject.Services.Interfaces;

namespace KutuphaneProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //SqlServer-------------------------------------------
            var conStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<KutuphaneDbContext>(x => x.UseSqlServer(conStr));
            //----------------------------------------------------

            builder.Services.AddControllersWithViews();
            
            //Services--------------------------------------------
            builder.Services.AddScoped<IKitapService, KitapService>();
            builder.Services.AddScoped<IMesajService, MesajService>();
            builder.Services.AddScoped<IOduncService, OduncService>();
            builder.Services.AddScoped<IOgrenciService, OgrenciService>();
            builder.Services.AddScoped<IKategoriService, KategoriService>();

            builder.Services.AddScoped<GecikmeKontrolService>();
            //----------------------------------------------------

            //Configure Identity----------------------------------
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<KutuphaneDbContext>()
                    .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true; // En az bir rakam zorunlu
                options.Password.RequireNonAlphanumeric = false; // Özel karakter zorunluluğu yok
                options.Password.RequiredLength = 8; // Şifrenin minimum uzunluğu
            });

            // Kullanıcı oturum süresi yapılandırma----------------------------------
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Oturum süresini 60 dakika olarak ayarlar.
                options.SlidingExpiration = true; // Kullanıcı aktifse oturum süresi yenilenir.
                options.Events.OnSigningOut = async context =>
                {
                    // Kullanıcı oturum kapatma işlemine geçtiğinde yapılacak işlemler.
                    await context.HttpContext.SignOutAsync();
                };
            });
            //----------------------------------------------------

            var app = builder.Build();

            // Rolleri ve admin kullanıcıyı ekleme----------------------------------
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                Task.Run(async () =>
                {
                    // Admin rolü ekle
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    string adminTCNo = "12345678901";
                    string adminPassword = "Admin123!";

                    if (await userManager.FindByNameAsync(adminTCNo) == null)
                    {
                        var adminUser = new AppUser
                        {
                            UserName = "AdminUser",
                            TCNo = adminTCNo,
                            OgrenciAdSoyad = "Sistem Yöneticisi",
                            OgrenciNo = adminTCNo,
                        };

                        var result = await userManager.CreateAsync(adminUser, adminPassword);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                        }
                    }
                }).GetAwaiter().GetResult();
            }
            //-----------------------------------------------------------------------

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //-----------------------------------------------------------------------   
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = false,
                OnPrepareResponse = ctx =>
                {
                    const int sure = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + sure;
                    ctx.Context.Response.Headers[HeaderNames.Expires] = new[]
                    {
                        DateTime.Now.AddMonths(1).ToString("R"),
                    };
                }
            });
            //-----------------------------------------------------------------------

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Admin}/{action=AnasayfaAdmin}/{id?}")
                .WithStaticAssets();
            
            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Ogrenci}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
