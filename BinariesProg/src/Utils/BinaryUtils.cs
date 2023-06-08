using binaries.Conversion;
using binaries.Representation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

            return new BinaryValue(sb.ToString(),false,input.preventOverflow);
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

        public static BinaryValue Shift(BinaryValue input, int amount)
        {
            if (Math.Abs(amount) >= input.value.Length)
            {
                return And(input, BinaryValue.ZERO());
            }
            else if (amount == 0)
            {
                return input;
            }
            else if (amount > 0)
            {
                char[] bin = And(input, BinaryValue.ZERO()).value.ToCharArray();

                for(int i = 0; i < bin.Length-amount; i++)
                {
                    bin[amount + i] = input.value[i];
                }

                if (input.twoComplement)
                {
                    for(int i = 0; i < bin.Length; i++)
                    {
                        if (bin[i] == '1') break;

                        bin[i] = '1';
                    }
                }

                return new BinaryValue(new string(bin));
            }
            else
            {
                amount *= -1;

                if (input.preventOverflow)
                {
                    char c = input.twoComplement ? '0' : '1';
                    int leftRoom = 0;
                    for(int i = 0; i < input.value.Length; i++)
                    {
                        if (input.value[i] == c)
                        {
                            leftRoom = i;
                            break;
                        }
                    }
                    if (leftRoom < amount)
                    {
                        int len = ConvertUtils.PadNumber(input.value.Length+amount-leftRoom, 4);
                        input = ConvertUtils.PadBinary(input, len, input.twoComplement);
                    }
                }

                char[] bin = And(input, BinaryValue.ZERO()).value.ToCharArray();         

                for (int i = bin.Length-1; i >= 0 + amount; i--)
                {
                    bin[i-amount] = input.value[i];
                }

                return new BinaryValue(new string(bin));
            }
        }
    }
}
