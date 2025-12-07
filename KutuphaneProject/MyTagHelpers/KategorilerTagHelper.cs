using KutuphaneProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KutuphaneProject.MyTagHelpers
{
    public class KategorilerTagHelper : TagHelper
    {
        public List<Kategori> Kategorilerlist { get; set; }

        public KategorilerTagHelper()
        {
            Kategorilerlist = new List<Kategori>();
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            TagBuilder div1Tag = new TagBuilder("div");
            div1Tag.AddCssClass("row justify-content-center justify-content-md-start g-4");

            foreach (var kategori in Kategorilerlist)
            {
                TagBuilder div2Tag = new TagBuilder("div");
                div2Tag.AddCssClass("col-10 col-md-6 col-lg-4");

                TagBuilder aTag = new TagBuilder("a");
                aTag.Attributes.Add("href", $"/Ogrenci/Kategori?kategoriId={kategori.Id}");

                TagBuilder div3Tag = new TagBuilder("div");
                div3Tag.AddCssClass("img-cont position-relative overflow-hidden");

                TagBuilder div4Tag = new TagBuilder("div");
                div4Tag.AddCssClass("overlay position-absolute top-0 start-0 h-100 w-100");
                TagBuilder imgTag = new TagBuilder("img");
                imgTag.AddCssClass("kategori-img");
                imgTag.Attributes.Add("src", kategori.ImageSrc);
                TagBuilder pTag = new TagBuilder("p");
                pTag.AddCssClass("kategori-ad text-white");
                pTag.InnerHtml.Append(kategori.Ad);

                div3Tag.InnerHtml.AppendHtml(div4Tag);
                div3Tag.InnerHtml.AppendHtml(imgTag);
                div3Tag.InnerHtml.AppendHtml(pTag);

                aTag.InnerHtml.AppendHtml(div3Tag);

                div2Tag.InnerHtml.AppendHtml(aTag);

                div1Tag.InnerHtml.AppendHtml(div2Tag);
            }

            output.Content.SetHtmlContent(div1Tag);

            base.Process(context, output);
        }
    }
}
