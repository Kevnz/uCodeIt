using System;
using System.Collections.Generic;
using uCodeIt.DocumentTypes;

namespace uCodeIt.Metadata
{
    public sealed class DocumentTypeMetadata : TypeMetadata
    {
        public string Icon { get; internal set; }
        public string Thumbnail { get; internal set; }
        public bool AllowAsRoot { get; internal set; }
        public IEnumerable<string> Templates { get; internal set; }
        public IEnumerable<DocumentTypeMetadata> AllowedChildren { get; internal set; }

        internal DocumentTypeAttribute Attribute { get; set; }

        public IEnumerable<PropertyMetadata> Properties { get; internal set; }
    }
}
