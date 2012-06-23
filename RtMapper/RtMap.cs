using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtMapper.Exceptions;

namespace RtMapper
{
    public static class RtMap
    {
        private readonly static IDictionary<Type, Mapping> _configDictionary
            = new Dictionary<Type, Mapping>();

        public static TTo Map<TFrom, TTo>(TFrom source) where TTo : new()
        {
            return (TTo)Map(typeof(TFrom), typeof(TTo), source);
        }


        public static object Map(Type fromType, Type toType, object source)
        {
            if (!_configDictionary.ContainsKey(fromType))
                throw new MappingException();

            var result = Activator.CreateInstance(toType);
            var mapping = _configDictionary[fromType];

            foreach (var propertyMapping in mapping.PropertyMappings)
            {
                if (propertyMapping.SourceInfo.PropertyType == propertyMapping.DestInfo.PropertyType)
                {
                    var value = propertyMapping.SourceInfo.GetValue(source);
                    propertyMapping.DestInfo.SetValue(result, value);
                }
                else
                {
                    var value = propertyMapping.SourceInfo.GetValue(source);
                    var newValue = Map(propertyMapping.SourceInfo.PropertyType, propertyMapping.DestInfo.PropertyType, value);
                    propertyMapping.DestInfo.SetValue(result, newValue);
                }
            }
            return result;
        }

        public static void Clear()
        {
            _configDictionary.Clear();
        }

        public static void ConfigureMapping<TFrom, TTo>()
        {
            _configDictionary[typeof(TFrom)] = Mapping.Create<TFrom, TTo>();
        }

    }
}
