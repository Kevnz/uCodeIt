using uCodeIt.DocumentTypes;

namespace uCodeIt.Demo.Models.DocumentTypes
{
    [DocumentType(Alias = "Home")]
    public class HomePage : DocumentTypeBase
    {
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