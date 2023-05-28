using binaries.Conversion;
using binaries.Utils;
using System.Text;

namespace binaries.Representation
{
    public class BinaryValue : Value
    {
        private bool preventOverflow;
        private bool twoComplement;

        public BinaryValue(string value, bool overflow = false, bool preventOverflow = false) : base(value, overflow)
        {
            this.preventOverflow = preventOverflow;
        }

        public static Optional<BinaryValue> Parse(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);

            for(int i = 0; i < value.Length; i++)
            {
                if (value[i] == '0' || value[i] == '1')
                {
                    sb.Append(value[i]);
                }
                else
                {
                    return Optional<BinaryValue>.Empty();
                }
            }

            return Optional<BinaryValue>.From(new BinaryValue(sb.ToString()));
        }

        public static BinaryValue ONE()
        {
            return new BinaryValue("1");
        }

        public static BinaryValue ZERO()
        {
            return new BinaryValue("0");
        }

        public BinaryValue Subtract(BinaryValue b)
        {
            b = b.Negative2Complement();
            b.twoComplement = true;
            return this.Add(b);
        }

        public BinaryValue Add(BinaryValue b)
        {
            string v1;
            string v2;
            bool second = false;

            if (value.Length >= b.value.Length)
            {
                v1 = value;
                v2 = b.value;
            }
            else
            {
                v1 = b.value;
                v2 = value;
                second = true;
            }

            v2 = ConvertUtils.PadBinary(new BinaryValue(v2), v1.Length,(second ? this.twoComplement : b.twoComplement)).value;

            char[] bits = new char[v1.Length];

            bool carry = false;
            for (int i = v1.Length-1; i >= 0; i--)
            {
                int valuea = v1[i];
                int valueb = v2[i];

                int sum = (valuea-48) + (valueb-48) + (carry ? 1 : 0);

                if (sum > 1)
                {
                    sum = sum == 2 ? 0 : 1;
                    carry = true;
                }
                else
                {
                    carry = false;
                }
                
                bits[i] = (char)(sum+48);
            }

            if ((carry && (this.preventOverflow || b.preventOverflow)) && !b.twoComplement)
            {
                string p = '1'+new string(bits);
                int len = ConvertUtils.PadNumber(p.Length, 4);
                return ConvertUtils.PadBinary(new BinaryValue(p), len, twoComplement);
            }

            if (twoComplement || b.twoComplement) carry = false;

            //Prevent two complement sumation that should become negative from becoming positive
            if (second && b.twoComplement && bits[0] != '1')
            {
                string p = '1' + new string(bits);
                int len = ConvertUtils.PadNumber(p.Length, 4);
                return ConvertUtils.PadBinary(new BinaryValue(p), len, b.twoComplement);
            }

            return new BinaryValue(new string(bits),carry);
        }

        public BinaryValue Negative2Complement()
        {
            BinaryValue bv = BinaryUtils.Negate(this);
            bv.twoComplement = true;
            return bv.Add(ONE());
        }

        public override string ToString()
        {
            return value;
        }
    }
}
