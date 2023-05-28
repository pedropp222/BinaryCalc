using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Representation
{
    public abstract class Value
    {
        public readonly string value;
        public readonly bool overflow;
        public Value(string value, bool overflow = false)
        {
            this.value = value.Replace(" ","");
            this.overflow = overflow;
        }
    }
}
