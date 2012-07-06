using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                throw new MappingException(string.Format("Failed to located mapping config for {0}", fromType.FullName));

            if (toType.GetTypeInfo().IsEnum)
            {
                if (fromType.GetTypeInfo().IsEnum)
                {
                    var fromEnum = Enum.GetName(fromType, source);
                    return Enum.Parse(toType, fromEnum);
                }
            }

            var result = Activator.CreateInstance(toType);
            var mapping = _configDictionary[fromType];

            foreach (var propertyMapping in mapping.PropertyMappings)
            {
                try
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
                catch (Exception ex)
                {
                    string message = string.Format("Failed to map {0} to {1}",
                        propertyMapping.SourceInfo.Name, propertyMapping.DestInfo.Name);
                    throw new InvalidOperationException(message, ex);
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
