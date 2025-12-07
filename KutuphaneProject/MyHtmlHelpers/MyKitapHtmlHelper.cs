using KutuphaneProject.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KutuphaneProject.MyHtmlHelpers
{
    public static class MyKitapHtmlHelper
    {
        public static IHtmlContent KitapBilgileri(this IHtmlHelper htmlHelper, Kitap kitap)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            tagBuilder.AddCssClass("row justify-content-center");
            //-------------------------------
            TagBuilder div1 = new TagBuilder("div");
            div1.AddCssClass("col-11 col-md-10 col-lg-4");

            TagBuilder h3 = new TagBuilder("h3");
            h3.AddCssClass("kitap-ad p-2 mb-4");
            h3.InnerHtml.Append(kitap.Ad);
            div1.InnerHtml.AppendHtml(h3);

            TagBuilder div2 = new TagBuilder("div");
            div2.AddCssClass("img-cont mb-4 mb-lg-0");

            TagBuilder img = new TagBuilder("img");
            img.AddCssClass("img-thumbnail w-100");
            img.Attributes.Add("src", kitap.ImageSrc);
            div2.InnerHtml.AppendHtml(img);
            div1.InnerHtml.AppendHtml(div2);

            tagBuilder.InnerHtml.AppendHtml(div1);
            //-------------------------------
            TagBuilder div3 = new TagBuilder("div");
            div3.AddCssClass("col-12 col-md-10 col-lg-8");

            TagBuilder div4 = new TagBuilder("div");
            div4.AddCssClass("icerik");

            TagBuilder p = new TagBuilder("p");
            p.AddCssClass("aciklama p-3");
            p.InnerHtml.Append(kitap.Aciklama);
            div4.InnerHtml.AppendHtml(p);
            div3.InnerHtml.AppendHtml(div4);

            TagBuilder div5 = new TagBuilder("div");
            div5.AddCssClass("eklme-tarihi mb-3");
            div5.InnerHtml.Append("Ekleme Tarihi: ");

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("tarih");
            span.InnerHtml.Append(kitap.EklemeTarihi.ToShortDateString());
            div5.InnerHtml.AppendHtml(span);
            div3.InnerHtml.AppendHtml(div5);

            TagBuilder a = new TagBuilder("a");
            a.AddCssClass("btn-al text-decoration-none text-white p-1 px-2 border-0 d-block mx-auto");
            a.Attributes.Add("href", $"/Ogrenci/OduncAl?kitapId={kitap.Id}");
            a.InnerHtml.Append("Ödünç Al");
            div3.InnerHtml.AppendHtml(a);

            tagBuilder.InnerHtml.AppendHtml(div3);
            //-------------------------------
            return tagBuilder;
        }
    }
}
