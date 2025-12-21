using System;
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

            var minDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            var defaultDate = DateTime.Today.AddDays(10).ToString("yyyy-MM-dd");

            TagBuilder form = new TagBuilder("form");
            form.Attributes.Add("method", "post");
            form.Attributes.Add("action", "/Ogrenci/OduncAl");
            form.AddCssClass("d-flex flex-column gap-2");

            TagBuilder hiddenKitapId = new TagBuilder("input");
            hiddenKitapId.Attributes.Add("type", "hidden");
            hiddenKitapId.Attributes.Add("name", "kitapId");
            hiddenKitapId.Attributes.Add("value", kitap.Id.ToString());
            form.InnerHtml.AppendHtml(hiddenKitapId);

            TagBuilder label = new TagBuilder("label");
            label.AddCssClass("form-label");
            label.Attributes.Add("for", "geriDonusTarihi");
            label.InnerHtml.Append("İade Tarihi Seçiniz:");
            form.InnerHtml.AppendHtml(label);

            TagBuilder dateInput = new TagBuilder("input");
            dateInput.AddCssClass("form-control");
            dateInput.Attributes.Add("type", "date");
            dateInput.Attributes.Add("id", "geriDonusTarihi");
            dateInput.Attributes.Add("name", "geriDonusTarihi");
            dateInput.Attributes.Add("min", minDate);
            dateInput.Attributes.Add("value", defaultDate);
            dateInput.Attributes.Add("required", "required");
            form.InnerHtml.AppendHtml(dateInput);

            TagBuilder p2 = new TagBuilder("p");
            p2.AddCssClass("mb-2");
            p2.InnerHtml.Append("Gecikme durumunda her gün için 1 TL ceza uygulanacaktır.");
            form.InnerHtml.AppendHtml(p2);

            TagBuilder button = new TagBuilder("button");
            button.AddCssClass("btn-al text-decoration-none text-white p-1 px-2 border-0 d-block mx-auto");
            button.Attributes.Add("type", "submit");
            button.InnerHtml.Append("Ödünç Al");
            form.InnerHtml.AppendHtml(button);

            div3.InnerHtml.AppendHtml(form);

            tagBuilder.InnerHtml.AppendHtml(div3);
            //-------------------------------
            return tagBuilder;
        }
    }
}
