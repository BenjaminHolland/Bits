using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using System.Diagnostics;
//using Phi.Structure;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Phi.Numerics.Bits;
namespace Phi.Numerics.Testing {
    [TestClass]
    public class BitWidnowTests {

        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentOutOfRangeException), "length")]
        public void Ctor_NegativeBitLength() {
            BitWindow window = new BitWindow(-1);
        }
        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentOutOfRangeException), "length")]
        public void Ctor_ZeroBitLength() {
            BitWindow window = new BitWindow(0);
        }
        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentNullException), "source")]
        public void Ctor_NullBytes() {
            BitWindow window = new BitWindow(null);
        }
        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentException), "source")]
        public void Ctor_ZeroBytes() {
            BitWindow window = new BitWindow(new byte[] { });
        }
        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentOutOfRangeException), "offset")]
        public void Ctor_NegativeOffset() {
            BitWindow window = new BitWindow(new byte[1], -1, 8, EndianBitIndexer.LittleByteBigBitIndexer);
        }

        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentOutOfRangeException), "length")]
        public void Ctor_InsufficientBitsInSourceBecauseOfOffset() {
            BitWindow window = new BitWindow(new byte[1], 7, 8, EndianBitIndexer.LittleByteBigBitIndexer);
        }
        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentOutOfRangeException), "length")]
        public void Ctor_InsufficientBitsInSource() {
            BitWindow window = new BitWindow(new byte[1], 0, 16, EndianBitIndexer.LittleByteBigBitIndexer);
        }

        [TestMethod]
        [ExpectedArgumentException(typeof(ArgumentNullException), "indexer")]
        public void Ctor_NullIndexer() {
            BitWindow window = new BitWindow(new byte[1], 0, 8, null);
        }

        [TestMethod]
        public void Read_BitWindow_In32_NegativeOne() {
            BitWindow window = new BitWindow(32);
            for (int bitIdx = 0; bitIdx < window.Length; bitIdx++) {
                window[bitIdx] = true;
            }
            byte[] intBuffer = new byte[4];
            BitWindow intWindow = new BitWindow(intBuffer, 0, 32, EndianBitIndexer.LittleByteBigBitIndexer);

            window.WriteTo(intWindow, 0, 0, window.Length);
            Int32 result = BitConverter.ToInt32(intBuffer, 0);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(-1, result);

        }

        [TestMethod]
        public void Read_BitWindow_Int32_EnsureByteOrder() {
            Int32 expectedValue = 0xff00;
            BitWindow window = new BitWindow(32);

            for (Int32 bitIdx = 8; bitIdx < 16; bitIdx++) {
                window[bitIdx] = true;
            }

            byte[] intBuffer = new byte[4];
            BitWindow intWindow = new BitWindow(intBuffer, 0, 32, EndianBitIndexer.LittleByteBigBitIndexer);

            window.WriteTo(intWindow, 0, 0, window.Length);
            Int32 result = BitConverter.ToInt32(intBuffer, 0);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Read_BitWindow_In32_NegativeTwo() {
            BitWindow window = new BitWindow(32);
            for (int bitIdx = 0; bitIdx < window.Length; bitIdx++) {
                window[bitIdx] = true;
            }
            window[0] = false;
            byte[] intBuffer = new byte[4];
            BitWindow intWindow = new BitWindow(intBuffer, 0, 32, EndianBitIndexer.LittleByteBigBitIndexer);

            window.WriteTo(intWindow, 0, 0, window.Length);
            Int32 result = BitConverter.ToInt32(intBuffer, 0);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(-2, result);

        }

        [TestMethod]
        public void Read_BitWindow_In32_One() {
            BitWindow window = new BitWindow(8);
            window[0] = true;
            byte[] intBuffer = new byte[4];
            BitWindow intWindow = new BitWindow(intBuffer, 0, 32, EndianBitIndexer.LittleByteBigBitIndexer);

            window.WriteTo(intWindow, 0, 0, window.Length);
            Int32 result = BitConverter.ToInt32(intBuffer, 0);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, result);

        }

        [TestMethod]
        public void Extension_Crystalize_Int32_NegativeTwo() {
            Byte[] source = new byte[3];
            BitWindow window = new BitWindow(source, 3, 5, EndianBitIndexer.NativeBitIndexer);
            for (int bitIdx = 0; bitIdx < window.Length; bitIdx++) {
                window[bitIdx] = true;
            }
            window[0] = false;
            Assert.AreEqual(-2, BitConverter.ToInt32(window.Crystalize(IntegerFormat.S32), 0));

        }
        [TestMethod]
        public void Extension_Crystalize_Int32_EnsureByteOrder() {
            byte[] source = new byte[5];
            BitWindow window = new BitWindow(source, 3, 16, EndianBitIndexer.NativeBitIndexer);

            for (Int32 bitIdx = 8; bitIdx < 16; bitIdx++) {
                window[bitIdx] = true;
            }

            byte[] intBuffer = window.Crystalize(IntegerFormat.S32);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(unchecked((Int32)0xffffff00), BitConverter.ToInt32(intBuffer, 0));
        }

        [TestMethod]
        public void Extension_Shatter() {
            byte[] source = new byte[] { 18, 52 };
            UInt32[] results = new uint[] { 1, 2, 3, 4 };
            var view = new BitWindow(source);
            var values = view.Shatter(Enumerable.Repeat(4, 4)).Select(window => BitConverter.ToUInt32(window.Crystalize(IntegerFormat.U32), 0)).ToArray();
            for (int i = 0; i < 4; i++) {
                Assert.AreEqual(results[i], values[i]);
            }
        }


    }
}
