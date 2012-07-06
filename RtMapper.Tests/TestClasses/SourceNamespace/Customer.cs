using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtMapper.Tests.TestClasses.SourceNamespace
{
    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Address PostalAddress { get; set; }
        public Address PhysicalAddress { get; set; }
        
        public CustomerType CustomerType { get; set; } 
    }
}
