using System.Globalization;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace uCodeIt.DocumentTypes
{
    public abstract class DocumentTypeBase : RenderModel
    {
        protected DocumentTypeBase(IPublishedContent content)
            : this(content, UmbracoContext.Current.PublishedContentRequest.Culture)
        {
        }

        protected DocumentTypeBase(IPublishedContent content, CultureInfo cultureInfo)
            : base(content, cultureInfo)
        {
        }
    }
}
