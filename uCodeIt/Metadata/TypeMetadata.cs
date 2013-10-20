using System;
namespace uCodeIt.Metadata
{
    public abstract class TypeMetadata
    {
        public string Name { get; internal set; }
        public string Alias { get; internal set; }
        public string Description { get; internal set; }

        public Type Type { get; internal set; }
    }
}
