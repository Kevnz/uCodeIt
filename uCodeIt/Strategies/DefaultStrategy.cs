using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCodeIt.DocumentTypes;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uCodeIt.Strategies
{
    /// <summary>
    /// The default document type strategy.
    /// This strategy will:
    /// 1) search for existing content types
    /// 2) if types exist that match the document type in your site, then:
    ///     a) add non-existent properties from class to doctype;
    ///     b) delete non-existent properties in class from doctype;
    /// 3) any other doctypes will be ignored (e.g. package-added ones from uBlogsy etc.)
    /// </summary>
    public class DefaultStrategy : IDocumentTypeInitStrategy
    {
        public IContentTypeService ContentTypeService { get; set; }
        private IEnumerable<IContentType> AllContentTypes { get; set; }

        public void Process(IEnumerable<Type> types)
        {
            AllContentTypes = ContentTypeService.GetAllContentTypes();
            
            foreach (var type in types)
            {
                var matchedType = (from c in AllContentTypes
                                   where c.Alias.ToLowerInvariant() == type.Name.ToLowerInvariant()
                                   select c).Single();
                if (matchedType != null)
                {
                    var allProps = (from prop in type.GetProperties()
                                    where prop.CustomAttributes.Any(p => p.AttributeType == typeof(TabGroupAttribute))
                                    select prop);
                }
            }
        }

    }
}
