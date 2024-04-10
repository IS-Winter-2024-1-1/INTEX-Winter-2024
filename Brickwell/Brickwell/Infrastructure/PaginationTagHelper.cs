using Brickwell.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Brickwell.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            urlHelperFactory = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public string? PageAction { get; set; }
        public PaginationInfo? PageModel { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null && PageModel != null && PageModel.TotalPages > 1)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);

                TagBuilder result = new TagBuilder("div");
                TagBuilder ul = new TagBuilder("ul");
                ul.AddCssClass("pagination justify-content-center");

                int currentPage = PageModel.CurrentPage;

                // Adjust these values as needed
                int maxVisiblePages = 5; // Maximum number of visible page links
                int startPage = Math.Max(1, currentPage - maxVisiblePages / 2);
                int endPage = Math.Min(startPage + maxVisiblePages - 1, PageModel.TotalPages);
                startPage = Math.Max(1, endPage - maxVisiblePages + 1);

                if (startPage > 1)
                {
                    ul.InnerHtml.AppendHtml(CreatePageLink(urlHelper, 1, "First"));
                    ul.InnerHtml.AppendHtml(CreateEllipsis());
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    ul.InnerHtml.AppendHtml(CreatePageLink(urlHelper, i));
                }

                if (endPage < PageModel.TotalPages)
                {
                    ul.InnerHtml.AppendHtml(CreateEllipsis());
                    ul.InnerHtml.AppendHtml(CreatePageLink(urlHelper, PageModel.TotalPages, "Last"));
                }

                result.InnerHtml.AppendHtml(ul);
                output.Content.AppendHtml(result.InnerHtml);
            }
        }

        private TagBuilder CreatePageLink(IUrlHelper urlHelper, int pageNumber, string? label = null)
        {
            TagBuilder tag = new TagBuilder("li");
            tag.Attributes["class"] = "page-item";
            TagBuilder anchorTag = new TagBuilder("a");
            anchorTag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNum = pageNumber });
            anchorTag.Attributes["class"] = "page-link";
            anchorTag.InnerHtml.Append(pageNumber.ToString());
            if (!string.IsNullOrEmpty(label))
            {
                anchorTag.InnerHtml.Append(" ");
                anchorTag.InnerHtml.Append(label);
            }
            tag.InnerHtml.AppendHtml(anchorTag);
            return tag;
        }

        private TagBuilder CreateEllipsis()
        {
            TagBuilder tag = new TagBuilder("li");
            tag.InnerHtml.AppendHtml("<span> ... </span>");
            return tag;
        }
    }
}
