using binaries.Representation;
using binaries.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.Conversion
{
    public class BinaryConverter
    {
        public static string BinaryToHexadecimal(BinaryValue bin)
        {
            StringBuilder sb = new StringBuilder();

            if (bin.value.Length % 4 != 0 )
            {
                bin = ConvertUtils.PadBinary(bin, ConvertUtils.PadNumber(bin.value.Length,4));
            }

            for (int i = 0; i < bin.value.Length; i += 4)
            {
                int v = BinaryToDecimal(new BinaryValue(bin.value[i..(i + 4)]));             

                char c = (char)(v < 10 ? 48 + v : 55 + v);

                sb.Append(c);
            }

            return sb.ToString();
        }

        public static BinaryValue HexadecimalToBinary(string hexValue)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hexValue.Length; i++)
            {
                int v;
                if (!int.TryParse(hexValue[i].ToString(), out v))
                {
                    v = 10 + int.Parse(((char)(hexValue[i] - 17)).ToString());
                }
                sb.Append(DecimalToBinary(v));
            }

            return new BinaryValue(sb.ToString());
        }

        public static int HexadecimalToDecimal(string hex)
        {
            return BinaryToDecimal(HexadecimalToBinary(hex));
        }

        public static BinaryValue DecimalToBinary(int number, int padding = 4)
        {
            bool negative = number < 0;

            if (negative)
            {
                number *= -1;
            }

            char[] bitValue;

            int exp = 0;

            int bits = ConvertUtils.BitCount(negative?number*2:number);

            bits = ConvertUtils.PadNumber(bits, padding);

            bitValue = new char[bits];

            FillCharArr(bitValue);

            while (number != 0)
            {
                int val = (int)Math.Pow(2, exp);

                if (val < number)
                {
                    exp++;
                }
                else
                {
                    if (val > number) exp--;

                    number -= (int)Math.Pow(2, exp);
                    bitValue[bits - 1 - exp] = '1';
                    exp = 0;
                }
            }

            if (negative)
            {
                return new BinaryValue(new string(bitValue)).Negative2Complement();
            }
            return new BinaryValue(new string(bitValue));
        }

        public static int BinaryToDecimal(BinaryValue binary)
        {
            int num = 0;
            for (int i = 0; i < binary.value.Length; i++)
            {
                if (binary.value[i] == '1')
                {
                    num += (int)Math.Pow(2, binary.value.Length - 1 - i);
                }
            }
            return num;
        }

        public static int BinaryToDecimal2Complement(BinaryValue binary)
        {
            if (binary.value[0] == '0')
            {
                return BinaryToDecimal(binary);
            }

            return -BinaryToDecimal(BinaryUtils.Negate(binary).Add(BinaryValue.ONE()));
        }

        public static int DecimalToBase(int dec, int b)
        {
            int num = 0;
            int iter = 0;

            while(dec > 0)
            {
                int rem = dec % b;
                if (iter == 0)
                {
                    num = rem;
                }
                else
                {
                    int index = (int)Math.Pow(10, iter);
                    num += rem*(index);
                }

                dec /= b;
                iter++;
            }

            return num;
        }

        public static int QuadToDecimal(int quad)
        {
            string str = quad.ToString();

            int value = 0;

            for(int i = str.Length-1; i >= 0; i--)
            {
                value += (int)(Math.Pow(4,str.Length-1-i) * (str[i]-48));
            }

            return value;
        }

        public static int OctalToDecimal(int oct)
        {
            string str = oct.ToString();

            int value = 0;

            for (int i = str.Length - 1; i >= 0; i--)
            {
                value += (int)(Math.Pow(8, str.Length - 1 - i) * (str[i] - 48));
            }

            return value;
        }

        private static void FillCharArr(char[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = '0';
            }
        }
    }
}
