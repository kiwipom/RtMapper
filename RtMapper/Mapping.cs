using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RtMapper
{
    public class Mapping
    {
        public Mapping()
        {
            PropertyMappings = new List<PropertyMapping>();
        }

        internal static Mapping Create<TFrom, TTo>()
        {
            var result = new Mapping();
            var sourceProperties = typeof(TFrom).GetTypeInfo()
                .DeclaredProperties;
            var destProperties = typeof(TTo).GetTypeInfo()
                .DeclaredProperties;

            var comparer = new PropertyInfoComparer();
            foreach (var sourceProp in sourceProperties)
            {
                var destProp = destProperties.SingleOrDefault(p => comparer.Equals(sourceProp, p));
                if (destProp != null)
                {
                    var propertyMapping = new PropertyMapping(sourceProp, destProp);
                    result.PropertyMappings.Add(propertyMapping);
                }
            }
            return result;
        }

        internal IList<PropertyMapping> PropertyMappings { get; private set; }
    }
}
