using binaries.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.Representation
{
    internal class AnyValue : Value
    {
        public AnyValue(string value, bool overflow = false) : base(value, overflow)
        {
        }
    }
}
