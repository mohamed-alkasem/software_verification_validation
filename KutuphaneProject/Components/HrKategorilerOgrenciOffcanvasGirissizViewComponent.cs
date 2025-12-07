using KutuphaneProject.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneProject.Components
{
    public class HrKategorilerOgrenciOffcanvasGirissizViewComponent : ViewComponent
    {
        private readonly IServiceProvider _serviceProvider;

        public HrKategorilerOgrenciOffcanvasGirissizViewComponent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<KutuphaneDbContext>();
                var kategoriler = await context.Kategoriler.AsNoTracking().ToListAsync();
                return View(kategoriler);
            }
        }   
    }
}
