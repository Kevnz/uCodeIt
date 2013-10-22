using System;
using uCodeIt.DocumentTypes;
using Umbraco.Core.Models;

namespace uCodeIt.Demo.Models.DocumentTypes
{
    [DocumentType(Alias = "Home", AllowedChildren = new[] { typeof(HomePage) }, Templates = new[] { "Home Page", "Alt Home Page" })]
    public class HomePage : DocumentTypeBase
    {
        public HomePage(IPublishedContent content) 
            : base(content)
        {
        }

        [Property(DataType = DataType.TextString, Tab = Tab.Tab2, Name = "Second Title")]
        public string Title { get; set; }

        [Property(DataType = DataType.TextString, Tab = Tab.Content)]
        public string BodyContent { get; set; }

        public enum Tab
        {
            Content = 1,
            Tab2 = 2
        }
    }
}