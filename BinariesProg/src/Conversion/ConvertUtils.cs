using binaries.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Conversion
{
    public class ConvertUtils
    {
        /// <summary>
        /// Pads the given bit number to the padding. E.G., 5,4 -> 8
        /// </summary>
        /// <param name="number">A number in bits to pad</param>
        /// <param name="padding">The padding to apply</param>
        /// <returns>The padded number</returns>
        public static int PadNumber(int number, int padding)
        {
            if (number % padding == 0)
            {
                return number;
            }

            if (number < padding)
            {
                return padding;
            }

            int orPad = padding;

            while (padding < number)
            {
                padding += orPad;
            }

            number += padding - number;
            return number;
        }

        /// <summary>
        /// Returns the exact number of bits (with no padding) to fit the given decimal number
        /// </summary>
        public static int BitCount(int number)
        {
            if (number < 0) number *= -1;

            int exp = 0;

            int value = 0;

            while (number >= value)
            {
                exp++;
                value = (int)Math.Pow(2, exp);
            }

            return exp;
        }

        public static BinaryValue PadBinary(BinaryValue binary, int length, bool twoComplement = false)
        {
            StringBuilder sb = new StringBuilder(length);

            int pad = length - binary.value.Length;

            char appendBit = twoComplement ? '1' : '0';

            for (int i = 0; i < pad; i++)
            {
                sb.Append(appendBit);
            }

            sb.Append(binary);

            return new BinaryValue(sb.ToString());
        }
    }
}
