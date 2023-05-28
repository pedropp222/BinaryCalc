using binaries.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Utils
{
    public class Optional<T>
    {
        private readonly T? _value;

        private Optional(T obj)
        {
            _value = obj;
        }

        private Optional()
        {
            _value = default;
        }
    
        public static Optional<T> From(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> Empty()
        {
            return new Optional<T>();
        }

        public bool HasValue()
        {
            return this._value != null;
        }

        public T? Get()
        {
            return this._value;
        }
    }
}
