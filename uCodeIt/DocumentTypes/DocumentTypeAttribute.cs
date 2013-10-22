using System;
using System.Collections.Generic;
using System.Linq;

namespace uCodeIt.DocumentTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DocumentTypeAttribute : Attribute
    {
        static DocumentTypeAttribute()
        {
            DefaultThumbnail = "doc.png";
            DefaultIcon = "doc.gif";
        }

        public DocumentTypeAttribute()
        {
            AllowedChildren = new Type[0];
            Templates = new string[0];
        }

        public string Alias { get; set; }
        public string Name { get; set; }
        public string[] Templates { get; set; }
        public string DefaultTemplate { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Thumbnail { get; set; }
        public bool AllowAsRoot { get; set; }

        public Type[] AllowedChildren { get; set; }

        public static string DefaultThumbnail { get; set; }
        public static string DefaultIcon { get; set; }
    }
}
