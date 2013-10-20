using System;

namespace uCodeIt.DocumentTypes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
        public object Tab { get; set; }
    }
}
