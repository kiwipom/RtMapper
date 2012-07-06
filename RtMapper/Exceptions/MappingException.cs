using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtMapper.Exceptions
{
    public class MappingException : Exception
    {
        public MappingException() : base()
        { }

        public MappingException(string message) : base(message)
        { }
    }
}
