﻿using System;
using System.Collections.Generic;

namespace uCodeIt.DocumentTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class DocumentTypeAttribute : Attribute
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Templates { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Thumbnail { get; set; }
        public bool AllowAsRoot { get; set; }

        public IEnumerable<Type> AllowedChildren { get; set; }
    }
}
