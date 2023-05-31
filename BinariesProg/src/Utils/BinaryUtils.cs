using binaries.Conversion;
using binaries.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Utils
{
    public class BinaryUtils
    {
        public static BinaryValue Negate(BinaryValue input)
        {
            StringBuilder sb = new StringBuilder(input.value.Length);

            for (int i = 0; i < input.value.Length; i++)
            {
                sb.Append(input.value[i] == '0' ? '1' : '0');
            }

            return new BinaryValue(sb.ToString());
        }

        public static BinaryValue And(BinaryValue input, BinaryValue mask)
        {
            if (input.value.Length > mask.value.Length)
            {
                mask = ConvertUtils.PadBinary(mask, input.value.Length);
            }
            else if (input.value.Length < mask.value.Length)
            {
                input = ConvertUtils.PadBinary(input, mask.value.Length);
            }

            StringBuilder sb = new StringBuilder(input.value.Length);


            for (int i = 0; i < input.value.Length; i++)
            {
                if (input.value[i] == '1' && mask.value[i] == '1')
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('0');
                }
            }

            return new BinaryValue(sb.ToString());
        }

        public static BinaryValue Or(BinaryValue input, BinaryValue mask)
        {
            if (input.value.Length > mask.value.Length)
            {
                mask = ConvertUtils.PadBinary(mask, input.value.Length);
            }
            else if (input.value.Length < mask.value.Length)
            {
                input = ConvertUtils.PadBinary(input, mask.value.Length);
            }

            StringBuilder sb = new StringBuilder(input.value.Length);

            for (int i = 0; i < input.value.Length; i++)
            {
                if (input.value[i] == '1' || mask.value[i] == '1')
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('0');
                }
            }

            return new BinaryValue(sb.ToString());
        }

        public static BinaryValue Xor(BinaryValue input, BinaryValue mask)
        {
            if (input.value.Length > mask.value.Length)
            {
                mask = ConvertUtils.PadBinary(mask, input.value.Length);
            }
            else if (input.value.Length < mask.value.Length)
            {
                input = ConvertUtils.PadBinary(input, mask.value.Length);
            }

            StringBuilder sb = new StringBuilder(input.value.Length);

            for(int i = 0; i < input.value.Length; i++)
            {
                if (input.value[i] != mask.value[i])
                {
                    sb.Append('1');
                }
                else
                {
                    sb.Append('0');
                }
            }

            return new BinaryValue(sb.ToString());
        }

        public static BinaryValue Nand(BinaryValue input, BinaryValue mask)
        {
            if (input.value.Length > mask.value.Length)
            {
                mask = ConvertUtils.PadBinary(mask, input.value.Length);
            }
            else if (input.value.Length < mask.value.Length)
            {
                input = ConvertUtils.PadBinary(input, mask.value.Length);
            }

            StringBuilder sb = new StringBuilder(input.value.Length);

            for (int i = 0; i < input.value.Length; i++)
            {
                if (input.value[i] == '1' && mask.value[i] == '1')
                {
                    sb.Append('0');
                }
                else
                {
                    sb.Append('1');
                }
            }

            return new BinaryValue(sb.ToString());
        }
    }
}
