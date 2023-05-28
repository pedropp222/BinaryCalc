using binaries.src.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.Parsing
{
    internal class ParseErrorException : Exception
    {
        public readonly int errorLocation;

        public ParseErrorException(string? message, int errorLocation) : base(message)
        {
            this.errorLocation = errorLocation;
        }
    }
}
