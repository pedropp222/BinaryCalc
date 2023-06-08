using binaries.Representation;
using binaries.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinariesTests
{
    [TestClass]
    public class BinaryValueTests
    {
        /// <summary>
        /// Test the creation of BinaryValue using literal string
        /// as the constructor parameter
        /// </summary>
        [TestMethod]
        public void BinaryValue_LiteralString_Tests()
        {
            BinaryValue bv = new BinaryValue("1110");

            Assert.AreEqual("1110", bv.value);

            bv = new BinaryValue("0001111");

            Assert.AreEqual("0001111", bv.value);

            bv = new BinaryValue("10101010101010111010110");

            Assert.AreEqual("10101010101010111010110", bv.value);     
        }

        /// <summary>
        /// Test the creation of BinaryValue using literal string
        /// as the constructor parameter. It should clean spaces.
        /// </summary>
        [TestMethod]
        public void BinaryValue_LiteralString_Clean_Tests()
        {
            BinaryValue bv = new BinaryValue("10 0011 0110");

            Assert.AreEqual("1000110110", bv.value);

            bv = new BinaryValue("10 0  011 0  110 ");

            Assert.AreEqual("1000110110", bv.value);
        }

        /// <summary>
        /// Test the creation of BinaryValue using literal string
        /// as the constructor parameter. It does not sanitize input beyond
        /// spaces
        /// </summary>
        [TestMethod]
        public void BinaryValue_LiteralString_Misc_Tests()
        {
            BinaryValue bv = new BinaryValue("12345");

            Assert.AreEqual("12345", bv.value);

            bv = new BinaryValue("ab8298fh29");

            Assert.AreEqual("ab8298fh29", bv.value);
        }

        /// <summary>
        /// Test the parsing method. It should correctly parse all
        /// valid values
        /// </summary>
        [TestMethod]
        public void BinaryValue_ParseCorrect_Tests()
        {
            Optional<BinaryValue> bv = BinaryValue.Parse("1001");

            Assert.IsTrue(bv.HasValue());

            Assert.AreEqual("1001",bv.Get().value);

            bv = BinaryValue.Parse("001010101");

            Assert.IsTrue(bv.HasValue());

            Assert.AreEqual("001010101",bv.Get().value);

            bv = BinaryValue.Parse("1110000001010010111101010101");

            Assert.IsTrue(bv.HasValue());

            Assert.AreEqual("1110000001010010111101010101", bv.Get().value);
        }

        /// <summary>
        /// Test parsing incorrect values. All should fail.
        /// </summary>
        [TestMethod]
        public void BinaryValue_ParseIncorrect_Tests()
        {
            Optional<BinaryValue> bv = BinaryValue.Parse("1   10");

            Assert.IsFalse(bv.HasValue());

            bv = BinaryValue.Parse("abcd");

            Assert.IsFalse(bv.HasValue());

            bv = BinaryValue.Parse("10100123");

            Assert.IsFalse(bv.HasValue());
        }

        /// <summary>
        /// Test the Add method that adds 2 binaty values together.
        /// This method does not test overflows, only simple adds.
        /// </summary>
        [TestMethod]
        public void BinaryValue_Add_Tests()
        {
            BinaryValue bv = new BinaryValue("1001");
            BinaryValue bv2 = new BinaryValue("0011");

            BinaryValue res = bv.Add(bv2);

            Assert.AreEqual("1100", res.value);

            bv = new BinaryValue("1010110");
            bv2 = new BinaryValue("1011");

            res = bv.Add(bv2);

            Assert.AreEqual("1100001",res.value);

            bv = new BinaryValue("1010001");
            bv2 = new BinaryValue("10101010");

            res = bv.Add(bv2);

            Assert.AreEqual("11111011", res.value);

            bv = new BinaryValue("0010101010101010111101110");
            bv2 = new BinaryValue("11100001001010101010");

            res = bv.Add(bv2);

            Assert.AreEqual("0011000110110100010011000", res.value);
        }

        /// <summary>
        /// Same as above, but all add results should result in incomplete
        /// representation due to bit overflow
        /// </summary>
        [TestMethod]
        public void BinaryValue_AddOverflow_Tests()
        {
            BinaryValue bv = new BinaryValue("1101");
            BinaryValue bv2 = new BinaryValue("0011");

            BinaryValue res = bv.Add(bv2);

            Assert.IsTrue(res.overflow);
            Assert.AreEqual("0000", res.value);


            bv = new BinaryValue("1110110");
            bv2 = new BinaryValue("1011");

            res = bv.Add(bv2);

            Assert.IsTrue(res.overflow);
            Assert.AreEqual("0000001", res.value);

            bv = new BinaryValue("100011011");
            bv2 = new BinaryValue("1111110000011");

            res = bv.Add(bv2);

            Assert.IsTrue(res.overflow);
            Assert.AreEqual("0000010011110", res.value);

            bv = new BinaryValue("10110110110110101");
            bv2 = new BinaryValue("1100101101011111");

            res = bv.Add(bv2);

            Assert.IsTrue(res.overflow);
            Assert.AreEqual("00011100100010100", res.value);

            bv = new BinaryValue("111111111111111");
            bv2 = new BinaryValue("1");

            res = bv.Add(bv2);

            Assert.IsTrue(res.overflow);
            Assert.AreEqual("000000000000000", res.value);
        }

        /// <summary>
        /// Test the 'prevent overflow' flag that, whenever an overflow 
        /// would occur, it instead grows the bit count to accomodate the
        /// new size. This grow will pad the binary to a length of a 
        /// multiple of 4.
        /// </summary>
        [TestMethod]
        public void BinaryValue_AddPreventOverflow_Tests()
        {
            BinaryValue bv = new BinaryValue("1101",false,true);
            BinaryValue bv2 = new BinaryValue("0011",false,true);

            BinaryValue res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("00010000", res.value);

            bv = new BinaryValue("1110110",false,true);
            bv2 = new BinaryValue("1011",false,true);

            res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("10000001", res.value);

            bv = new BinaryValue("100011011",false,true);
            bv2 = new BinaryValue("1111110000011", false, true);

            res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("0010000010011110", res.value);

            bv = new BinaryValue("10110110110110101", false, true);
            bv2 = new BinaryValue("1100101101011111", false, true);

            res = bv.Add(bv2);

            Assert.IsFalse (res.overflow);
            Assert.AreEqual("00100011100100010100", res.value);

            bv = new BinaryValue("111111111111111", false, true);
            bv2 = new BinaryValue("1", false, true);

            res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("1000000000000000", res.value);
        }

        /// <summary>
        /// Test the add method with 'prevent overflow' flag set.
        /// But this time the add calculations do not need to add new
        /// bits because there are no overflows. Therefore the final
        /// result should reflect that
        /// </summary>
        [TestMethod]
        public void BinaryValue_AddPreventOverflow_Unneeded_Tests()
        {
            BinaryValue bv = new BinaryValue("1001", false, true);
            BinaryValue bv2 = new BinaryValue("1", false, true);

            BinaryValue res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("1010", res.value);

            bv = new BinaryValue("00101011", false, true);
            bv2 = new BinaryValue("111", false, true);

            res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("00110010", res.value);

            bv = new BinaryValue("1100001110", false, true);
            bv2 = new BinaryValue("01110101", false, true);

            res = bv.Add(bv2);

            Assert.IsFalse(res.overflow);
            Assert.AreEqual("1110000011", res.value);
        }

        /// <summary>
        /// Test the method that converts a binary value to it's 2 complement
        /// value. The overflow bit should be set to false, even though
        /// there could be a left bit after the add 1 to the negated value.
        /// </summary>
        [TestMethod]
        public void BinaryValue_Negative2Complement_Tests()
        {
            BinaryValue bv = new BinaryValue("110011");
            BinaryValue res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("11001101", res.value);

            bv = new BinaryValue("11111");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("11100001", res.value);

            bv = new BinaryValue("10001101");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("111101110011", res.value);

            bv = new BinaryValue("01011");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("10101", res.value);

            bv = new BinaryValue("11111111111");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("100000000001", res.value);

            bv = new BinaryValue("00101011");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("11010101", res.value);

            bv = new BinaryValue("00000001");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("11111111", res.value);

            bv = new BinaryValue("00000000");
            res = bv.Negative2Complement();
            Assert.IsFalse(res.overflow);
            Assert.AreEqual("00000000", res.value);
        }

        [TestMethod]
        public void BinaryValue_Subtract_Tests()
        {
            BinaryValue bv = new BinaryValue("010011");
            BinaryValue bv2 = new BinaryValue("110");

            BinaryValue res = bv.Subtract(bv2);

            bv = new BinaryValue("00110010");
            bv2 = new BinaryValue("0110");

            res = bv.Subtract(bv2);

            //Assert.IsFalse(res.overflow);
            Assert.AreEqual("00101100", res.value);

            bv = new BinaryValue("0000111");
            bv2 = new BinaryValue("001011");

            res = bv.Subtract(bv2);

            //Assert.IsFalse(res.overflow);
            Assert.AreEqual("1111100", res.value);

            bv = new BinaryValue("10110100");
            bv2 = new BinaryValue("11001");

            res = bv.Subtract(bv2);

            //Assert.IsFalse(res.overflow);
            Assert.AreEqual("10011011", res.value);

            bv = new BinaryValue("101010101111");
            bv2 = new BinaryValue("000101010");

            res = bv.Subtract(bv2);

            //Assert.IsFalse(res.overflow);
            Assert.AreEqual("101010000101", res.value);

            bv = new BinaryValue("1110001");
            bv2 = new BinaryValue("101110101010000");

            res = bv.Subtract(bv2);

            //Assert.IsTrue(res.overflow);
            Assert.AreEqual("1010001100100001", res.value);

            bv = new BinaryValue("0101");
            bv2 = new BinaryValue("1101100100000011");

            res = bv.Subtract(bv2);

            Assert.AreEqual("11110010011100000010", res.value);
        }
    }
}
