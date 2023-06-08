using binaries.Representation;
using binaries.Utils;

namespace BinariesTests
{
    [TestClass]
    public class BinaryUtilsTests
    {
        [TestMethod]
        public void BinaryUtils_Negate_Tests()
        {
            BinaryValue bv = new BinaryValue("10010");
            BinaryValue res = BinaryUtils.Negate(bv);
            Assert.AreEqual("01101", res.value);

            bv = new BinaryValue("00101011");
            res = BinaryUtils.Negate(bv);
            Assert.AreEqual("11010100", res.value);

            bv = new BinaryValue("0000");
            res = BinaryUtils.Negate(bv);
            Assert.AreEqual("1111", res.value);

            bv = new BinaryValue("1111");
            res = BinaryUtils.Negate(bv);
            Assert.AreEqual("0000", res.value);

            bv = new BinaryValue("101110110101010001");
            res = BinaryUtils.Negate(bv);
            Assert.AreEqual("010001001010101110", res.value);
        }

        [TestMethod]
        public void BinaryUtils_And_Tests()
        {
            BinaryValue bv = new BinaryValue("10010");
            BinaryValue bv2 = new BinaryValue("11");

            BinaryValue res = BinaryUtils.And(bv, bv2);

            Assert.AreEqual("00010", res.value);

            bv = new BinaryValue("1100110");
            bv2 = new BinaryValue("0100100");

            res = BinaryUtils.And(bv, bv2);

            Assert.AreEqual("0100100", res.value);

            bv = new BinaryValue("11110000");
            bv2 = new BinaryValue("11100011");

            res = BinaryUtils.And(bv, bv2);

            Assert.AreEqual("11100000", res.value);
        }

        [TestMethod]
        public void BinaryUtils_Or_Tests()
        {
            BinaryValue bv = new BinaryValue("10010");
            BinaryValue bv2 = new BinaryValue("11");

            BinaryValue res = BinaryUtils.Or(bv, bv2);

            Assert.AreEqual("10011", res.value);

            bv = new BinaryValue("10000110");
            bv2 = new BinaryValue("00000000");

            res = BinaryUtils.Or(bv, bv2);

            Assert.AreEqual("10000110", res.value);

            bv = new BinaryValue("01011010");
            bv2 = new BinaryValue("11000011");

            res = BinaryUtils.Or(bv, bv2);

            Assert.AreEqual("11011011", res.value);

            bv = new BinaryValue("00101100");
            bv2 = new BinaryValue("11010011");

            res = BinaryUtils.Or(bv, bv2);

            Assert.AreEqual("11111111", res.value);
        }

        [TestMethod]
        public void BinaryUtils_Xor_Tests()
        {
            BinaryValue bv = new BinaryValue("10010");
            BinaryValue bv2 = new BinaryValue("11");

            BinaryValue res = BinaryUtils.Xor(bv, bv2);

            Assert.AreEqual("10001", res.value);

            bv = new BinaryValue("0110");
            bv2 = new BinaryValue("0011");

            res = BinaryUtils.Xor(bv, bv2);

            Assert.AreEqual("0101", res.value);

            bv = new BinaryValue("011011010101");
            bv2 = new BinaryValue("110101001001");

            res = BinaryUtils.Xor(bv, bv2);

            Assert.AreEqual("101110011100", res.value);
        }

        [TestMethod]
        public void BinaryUtils_Nand_Tests()
        {
            BinaryValue bv = new BinaryValue("10011");
            BinaryValue bv2 = new BinaryValue("110");

            BinaryValue res = BinaryUtils.Nand(bv, bv2);

            Assert.AreEqual("11101", res.value);

            bv = new BinaryValue("1100");
            bv2 = new BinaryValue("0101");

            res = BinaryUtils.Nand(bv, bv2);

            Assert.AreEqual("1011", res.value);

            bv = new BinaryValue ("1001010101");
            bv2 = new BinaryValue("1010101011");

            res = BinaryUtils.Nand(bv, bv2);

            Assert.AreEqual("0111111110", res.value);
        }

        [TestMethod]
        public void BinaryUtils_ShiftRight_Tests()
        {
            BinaryValue bv = BinaryUtils.Shift(new BinaryValue("10000"), 6);

            Assert.AreEqual("00000", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("10010"), 1);

            Assert.AreEqual("01001", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("10010"), 3);

            Assert.AreEqual("00010", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("10101101"), 5);

            Assert.AreEqual("00000101", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("111010111001"), 7);

            Assert.AreEqual("000000011101", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("111010111001"), 15);

            Assert.AreEqual("000000000000", bv.value);
        }

        [TestMethod]
        public void BinaryUtils_ShiftLeft_Tests()
        {
            BinaryValue bv = BinaryUtils.Shift(new BinaryValue("100101"), -7);

            Assert.AreEqual("000000", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("10010"), -1);

            Assert.AreEqual("00100", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("1111000"), -3);

            Assert.AreEqual("1000000", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("10010110"), -5);

            Assert.AreEqual("11000000", bv.value);

            bv = BinaryUtils.Shift(new BinaryValue("001100111001"), -7);

            Assert.AreEqual("110010000000", bv.value);
        }


    }
}
