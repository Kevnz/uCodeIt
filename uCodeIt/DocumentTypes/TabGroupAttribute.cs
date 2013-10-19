using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCodeIt.DocumentTypes
{
    public class TabGroupAttribute : Attribute
    {
        public TabGroupAttribute(string name)
        {
            TabName = name;
        }

        public string TabName { get; set; }
    }
}
