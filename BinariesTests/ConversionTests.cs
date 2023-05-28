using binaries.Conversion;
using binaries.Representation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryConverter = binaries.Conversion.BinaryConverter;

namespace BinariesTests
{
    [TestClass]
    public class ConversionTests
    {

        [TestInitialize] 
        public void Initialize() 
        { 
        }

        /// <summary>
        /// Test the binary to decimal conversion method.
        /// This will always produce a positive value.
        /// </summary>
        [TestMethod]
        public void BinaryToDecimal_Tests()
        {
            int v = BinaryConverter.BinaryToDecimal(new BinaryValue("1001"));

            Assert.AreEqual(9,v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("1111"));

            Assert.AreEqual(15,v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("01100001"));

            Assert.AreEqual(97, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("001101111000"));

            Assert.AreEqual(888, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("001111111011"));

            Assert.AreEqual(1019, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("10011001100"));

            Assert.AreEqual(1228, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("010101110011110"));

            Assert.AreEqual(11166, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("0100110010101000"));

            Assert.AreEqual(19624, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("00100000010000010010"));

            Assert.AreEqual(132114, v);

            v = BinaryConverter.BinaryToDecimal(new BinaryValue("111111111111111100"));

            Assert.AreEqual(262140, v);
        }

        /// <summary>
        /// Test the decimal to binary method.
        /// In this test we only test positive values, but negative
        /// values are allowed
        /// </summary>
        [TestMethod]
        public void DecimalToBinary_Tests()
        {
            BinaryValue v =  BinaryConverter.DecimalToBinary(112);

            Assert.AreEqual("01110000", v.value);

            v = BinaryConverter.DecimalToBinary(779);

            Assert.AreEqual("001100001011", v.value);

            v = BinaryConverter.DecimalToBinary(1024);

            Assert.AreEqual("010000000000", v.value);
            
            v = BinaryConverter.DecimalToBinary(27585);

            Assert.AreEqual("0110101111000001", v.value);

            v = BinaryConverter.DecimalToBinary(47945);

            Assert.AreEqual("1011101101001001", v.value);
        }

        /// <summary>
        /// Like the one above, but testing with negative decimal
        /// values
        /// </summary>
        [TestMethod]
        public void DecimalToBinary_Negative_Tests()
        {
            BinaryValue v = BinaryConverter.DecimalToBinary(-5);

            Assert.AreEqual("1011", v.value);

            v = BinaryConverter.DecimalToBinary(-15);

            Assert.AreEqual("11110001", v.value);

            v = BinaryConverter.DecimalToBinary(-21);

            Assert.AreEqual("11101011", v.value);

            v = BinaryConverter.DecimalToBinary(-32);

            Assert.AreEqual("11100000", v.value);

            v = BinaryConverter.DecimalToBinary(-39);

            Assert.AreEqual("11011001", v.value);

            v = BinaryConverter.DecimalToBinary(-55);

            Assert.AreEqual("11001001", v.value);

            v = BinaryConverter.DecimalToBinary(-167);

            Assert.AreEqual("111101011001", v.value);

            v = BinaryConverter.DecimalToBinary(-1255);

            Assert.AreEqual("101100011001", v.value);

            v = BinaryConverter.DecimalToBinary(-9424);

            Assert.AreEqual("1101101100110000", v.value);

            v = BinaryConverter.DecimalToBinary(-19275);

            Assert.AreEqual("1011010010110101", v.value);
            
            v = BinaryConverter.DecimalToBinary(-32365);

            Assert.AreEqual("1000000110010011", v.value);
        }

        /// <summary>
        /// Test the binary to decimal conversion method.
        /// Unlike the above binary to decimal test, this only treats 
        /// the binary value as 2 complement, so signed values 
        /// are allowed.
        /// </summary>
        [TestMethod]
        public void BinaryToDecimal2Complement_Tests()
        {
            int v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("0111"));

            Assert.AreEqual(7, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("1111"));

            Assert.AreEqual(-1, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("01011"));

            Assert.AreEqual(11, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("100101"));

            Assert.AreEqual(-27, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("00110011"));

            Assert.AreEqual(51, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("10101101"));

            Assert.AreEqual(-83, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("11101101"));

            Assert.AreEqual(-19, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("01101100"));

            Assert.AreEqual(108, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("010100101100"));

            Assert.AreEqual(1324, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("10100101100"));

            Assert.AreEqual(-724, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("1010100100101101"));

            Assert.AreEqual(-22227, v);

            v = BinaryConverter.BinaryToDecimal2Complement(new BinaryValue("111010100100101101"));

            Assert.AreEqual(-22227, v);
        }

        [TestMethod]
        public void ConvertUtils_BitCount_Tests()
        {
            int b = ConvertUtils.BitCount(6);

            Assert.AreEqual(3, b);

            b = ConvertUtils.BitCount(8);

            Assert.AreEqual(4, b);

            b = ConvertUtils.BitCount(51);

            Assert.AreEqual(6, b);

            b = ConvertUtils.BitCount(124);

            Assert.AreEqual(7, b);

            b = ConvertUtils.BitCount(999);

            Assert.AreEqual(10, b);

            b = ConvertUtils.BitCount(4291);

            Assert.AreEqual(13, b);

            b = ConvertUtils.BitCount(8844);

            Assert.AreEqual(14, b);

            b = ConvertUtils.BitCount(-8844);

            Assert.AreEqual(14, b);
        }

        [TestMethod]
        public void BinaryToHex_Tests()
        {
            string str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("1"));

            Assert.AreEqual("1", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("1000"));

            Assert.AreEqual("8", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("1010"));

            Assert.AreEqual("A", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("111111"));

            Assert.AreEqual("3F", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("00100111"));

            Assert.AreEqual("27", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("010101011101"));

            Assert.AreEqual("55D", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("110000001010"));

            Assert.AreEqual("C0A", str);

            str = BinaryConverter.BinaryToHexadecimal(new BinaryValue("01101011010100010101"));

            Assert.AreEqual("6B515", str);
        }

        [TestMethod]
        public void HexToBinary_Tests()
        {
            BinaryValue b = BinaryConverter.HexadecimalToBinary("3");

            Assert.AreEqual("0011", b.value);

            b = BinaryConverter.HexadecimalToBinary("7F");

            Assert.AreEqual("01111111", b.value);

            b = BinaryConverter.HexadecimalToBinary("501");

            Assert.AreEqual("010100000001", b.value);

            b = BinaryConverter.HexadecimalToBinary("CC77");

            Assert.AreEqual("1100110001110111", b.value);

            b = BinaryConverter.HexadecimalToBinary("1234");

            Assert.AreEqual("0001001000110100", b.value);

            b = BinaryConverter.HexadecimalToBinary("ABCDEF");

            Assert.AreEqual("101010111100110111101111", b.value);
            
            b = BinaryConverter.HexadecimalToBinary("D7B250F4D6");

            Assert.AreEqual("1101011110110010010100001111010011010110", b.value);
        }

        [TestMethod]
        public void HexToDecimal_Tests()
        {
            int v = BinaryConverter.HexadecimalToDecimal("3");

            Assert.AreEqual(3,v);

            v = BinaryConverter.HexadecimalToDecimal("C1");

            Assert.AreEqual(193, v);

            v = BinaryConverter.HexadecimalToDecimal("102F");

            Assert.AreEqual(4143, v);

            v = BinaryConverter.HexadecimalToDecimal("7DB9");

            Assert.AreEqual(32185, v);

            v = BinaryConverter.HexadecimalToDecimal("7E06B");

            Assert.AreEqual(516203, v);

            v = BinaryConverter.HexadecimalToDecimal("2EF950");

            Assert.AreEqual(3078480, v);
        }
    }
}