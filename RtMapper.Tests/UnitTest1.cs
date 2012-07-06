using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using RtMapper.Exceptions;
using RtMapper.Tests.TestClasses;

namespace RtMapper.Tests
{
    using SourceCustomer = RtMapper.Tests.TestClasses.SourceNamespace.Customer;
    using SourceAddress = RtMapper.Tests.TestClasses.SourceNamespace.Address;
    using SourceCustomerType = RtMapper.Tests.TestClasses.SourceNamespace.CustomerType;

    using DestCustomer = RtMapper.Tests.TestClasses.DestNamespace.Customer;
    using DestAddress = RtMapper.Tests.TestClasses.DestNamespace.Address;
    using DestCustomerType = RtMapper.Tests.TestClasses.DestNamespace.CustomerType;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CallToMapUnconfiguredTypeThrowsException()
        {
            Assert.ThrowsException<MappingException>(
                () => RtMap.Map<SourceObject, DestObject>(new SourceObject()));
        }


        [TestMethod]
        public void ConfigureMappingAddsConfigurationToDictionary()
        {
            RtMap.Clear();
            RtMap.ConfigureMapping<SourceObject, DestObject>();

            var dictionary = GetDictionary();
            Assert.AreEqual(1, dictionary.Keys.Count);
            Assert.IsTrue(dictionary.ContainsKey(typeof(SourceObject)));
        }

        [TestMethod]
        public void StringsMapByName()
        {
            const string testValue = "some value to test";

            RtMap.Clear();
            RtMap.ConfigureMapping<SourceObject, DestObject>();

            var source = new SourceObject { StringValue = testValue };

            var instance = RtMap.Map<SourceObject, DestObject>(source);

            Assert.AreEqual(testValue, instance.StringValue);
        }

        [TestMethod]
        public void IntsMapByName()
        {
            const int testValue = 67;

            RtMap.Clear();
            RtMap.ConfigureMapping<SourceObject, DestObject>();

            var source = new SourceObject { IntValue = testValue };

            var instance = RtMap.Map<SourceObject, DestObject>(source);

            Assert.AreEqual(testValue, instance.IntValue);
        }


        [TestMethod]
        public void MultiplePropertiesMapByName()
        {
            const string testStringValue = "this is something";
            const int testIntValue = 67;

            RtMap.Clear();
            RtMap.ConfigureMapping<SourceObject, DestObject>();

            var source = new SourceObject { StringValue = testStringValue, IntValue = testIntValue };

            var instance = RtMap.Map<SourceObject, DestObject>(source);

            Assert.AreEqual(testStringValue, instance.StringValue);
            Assert.AreEqual(testIntValue, instance.IntValue);
        }

        [TestMethod]
        public void ClearRtMapRemovesAllMappings()
        {
            RtMap.Clear();
            var dictionary = GetDictionary();
            Assert.AreEqual(0, dictionary.Keys.Count);
        }

        
        [TestMethod]
        public void FirstLevelInnerTypesMapCorrectly()
        {
            RtMap.Clear();
            RtMap.ConfigureMapping<SourceAddress, DestAddress>();
            RtMap.ConfigureMapping<SourceCustomer, DestCustomer>();
            RtMap.ConfigureMapping<SourceCustomerType, DestCustomerType>();
            
            var source = new SourceCustomer
            {
                Id = 34354,
                Name = "Jonny Wilkinson",
                PostalAddress = new SourceAddress
                {
                    House = "PO Box 34544",
                    Street = "Posting Road",
                    Suburb = "CBD",
                    City = "Auckland",
                    Postcode = "1010",
                    Country = "New Zealand"
                },
                PhysicalAddress = new SourceAddress
                {
                    House = "100",
                    Street = "High Street",
                    Suburb = "CBD",
                    City = "Auckland",
                    Postcode = "1010",
                    Country = "New Zealand"
                },
                CustomerType = SourceCustomerType.Active
            };

            var instance = RtMap.Map<SourceCustomer, DestCustomer>(source);

            Assert.AreEqual(34354, instance.Id);
            Assert.AreEqual("Jonny Wilkinson", instance.Name);
            Assert.AreEqual("PO Box 34544", instance.PostalAddress.House);
            Assert.AreEqual("Posting Road", instance.PostalAddress.Street);
            Assert.AreEqual("CBD", instance.PostalAddress.Suburb);
            Assert.AreEqual("Auckland", instance.PostalAddress.City);
            Assert.AreEqual("1010", instance.PostalAddress.Postcode);
            Assert.AreEqual("New Zealand", instance.PostalAddress.Country);
            Assert.AreEqual("100", instance.PhysicalAddress.House);
            Assert.AreEqual("High Street", instance.PhysicalAddress.Street);
            Assert.AreEqual("CBD", instance.PhysicalAddress.Suburb);
            Assert.AreEqual("Auckland", instance.PhysicalAddress.City);
            Assert.AreEqual("1010", instance.PhysicalAddress.Postcode);
            Assert.AreEqual("New Zealand", instance.PhysicalAddress.Country);
            Assert.AreEqual(DestCustomerType.Active, instance.CustomerType);
        }



        private IDictionary<Type, Mapping> GetDictionary()
        {
            return typeof(RtMap).GetTypeInfo()
                .DeclaredFields.Single(f =>
                {
                    return f.Name == "_configDictionary" && f.IsStatic && f.IsPrivate;
                }).GetValue(null) as IDictionary<Type, Mapping>;
        }
    }
}
