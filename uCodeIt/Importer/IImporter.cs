using System.Collections.Generic;
using uCodeIt.Metadata;

namespace uCodeIt.Importer
{
    internal interface IImporter
    {
        void Process(IEnumerable<DocumentTypeMetadata> types);
    }
}
