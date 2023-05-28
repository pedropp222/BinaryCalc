using binaries.Representation;
using binaries.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
