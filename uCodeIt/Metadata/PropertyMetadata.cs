namespace uCodeIt.Metadata
{
    public sealed class PropertyMetadata : TypeMetadata
    {
        public string DataType { get; internal set; }
        public object Tab { get; set; }
    }
}
