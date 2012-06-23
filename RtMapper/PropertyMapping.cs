using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RtMapper
{
    class PropertyMapping
    {
        public PropertyMapping(PropertyInfo sourceInfo, PropertyInfo destInfo)
        {
            SourceInfo = sourceInfo;
            DestInfo = destInfo;
        }

        internal PropertyInfo SourceInfo { get; private set; }
        internal PropertyInfo DestInfo { get; private set; }
    }
}
