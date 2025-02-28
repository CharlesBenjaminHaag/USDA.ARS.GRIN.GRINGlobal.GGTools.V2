using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public static class HtmlHelperImage
    {
        public static IHtmlString ImageUpload(this HtmlHelper<AccessionInventoryAttachmentViewModel> htmlHelper, AccessionInventoryAttachmentViewModel viewModel)
        {
            var outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("pull-left upload-img-wrapper");
            var label = new TagBuilder("label");
            label.AddCssClass("upload-img");
            label.MergeAttribute("data-content", viewModel.Entity.ContentType);

            var image = new TagBuilder("img");
            image.AddCssClass("img-responsive");
            image.MergeAttribute("src", viewModel.Entity.ThumbnailVirtualPath);
            image.MergeAttribute("width", "250");
            image.MergeAttribute("height", "250");

            var textbox = InputExtensions.TextBoxFor(htmlHelper, m => m.Entity.Title, new { type = "file", style = "display:none" });

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append(label.ToString(TagRenderMode.StartTag));
            htmlBuilder.Append(image.ToString(TagRenderMode.Normal));
            htmlBuilder.Append(label.ToString(TagRenderMode.EndTag));
            htmlBuilder.Append(textbox.ToHtmlString());
            outerDiv.InnerHtml = htmlBuilder.ToString();
            var html = outerDiv.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(html);
        }
    }
}