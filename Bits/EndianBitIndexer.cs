using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phi.Numerics.Bits {

    /// <summary>
    /// Provides bit indexing for a given bit/byte order.
    /// </summary>
    public class EndianBitIndexer : IBitIndexer {
        
        public static readonly EndianBitIndexer BigByteBigBitIndexer = new EndianBitIndexer(Endianness.Big, Endianness.Big);
        public static readonly EndianBitIndexer BigByteLittleBitIndexer = new EndianBitIndexer(Endianness.Big, Endianness.Little);
        public static readonly EndianBitIndexer LittleByteBigBitIndexer = new EndianBitIndexer(Endianness.Little, Endianness.Big);
        public static readonly EndianBitIndexer LittleByteLittleBitIndexer = new EndianBitIndexer(Endianness.Little, Endianness.Little);
        public static readonly EndianBitIndexer NativeBitIndexer = LittleByteBigBitIndexer;
        readonly Endianness ByteOrder;
        readonly Endianness BitOrder;
 
        Boolean BytesAreLittleEndian {
            get {
                return ByteOrder == Endianness.Little;
            }
        }

        Boolean BitsAreLittleEndian {
            get {
                return BitOrder == Endianness.Little;
            }
        }
        
        /// <summary>
        /// Create a bit indexer that will index bits for values stored in the given format.
        /// </summary>
        /// <param name="byteOrder">The byte order.</param>
        /// <param name="bitOrder">The bit order.</param>
        public EndianBitIndexer(Endianness byteOrder, Endianness bitOrder) {
            ByteOrder = byteOrder;
            BitOrder = bitOrder;
        }
        public Int32 GetIndexForBitWithPower(Int32 bitPower, Int32 valueBitWidth) {
            Int32 valueByteWidth = valueBitWidth / 8;
            Int32 bitOffset = 0;
            Int32 byteOffset = 0;
            Int32 maxBitOffset = Math.Min(7, valueBitWidth-1);
            if (BytesAreLittleEndian) {
                byteOffset = (bitPower / 8);
                if (BitsAreLittleEndian) {
                    bitOffset = bitPower % 8;
                } else {
                    bitOffset = maxBitOffset- bitPower % (maxBitOffset+1);
                }
            } else {
                byteOffset = (valueByteWidth - 1) - (bitPower / 8);
                if (BitsAreLittleEndian) {
                    bitOffset = bitPower % 8;
                } else {
                    bitOffset = maxBitOffset - bitPower % (maxBitOffset+1);

                }
            }
            return byteOffset * 8 + bitOffset;
        }
    }
}
