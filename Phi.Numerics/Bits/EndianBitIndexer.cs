using Phi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phi.Numerics.BitView {

    /// <summary>
    /// Provides bit indexing for a given bit/byte order.
    /// </summary>
    [Todo("Add support for serialization.")]
    public class EndianBitIndexer : IBitIndexer {
        public static readonly EndianBitIndexer BigByteBigBitIndexer = new EndianBitIndexer(Endianness.Big, Endianness.Big);
        public static readonly EndianBitIndexer BigByteLittleBitIndexer = new EndianBitIndexer(Endianness.Big, Endianness.Little);
        public static readonly EndianBitIndexer LittleByteBigBitIndexer = new EndianBitIndexer(Endianness.Little, Endianness.Big);
        public static readonly EndianBitIndexer LittleByteLittleBitIndexer = new EndianBitIndexer(Endianness.Little, Endianness.Little);
        public static readonly EndianBitIndexer NativeBitIndexer = LittleByteBigBitIndexer;

        public static IBitIndexer GetStandard(Endianness bitOrder, Endianness byteOrder) {
            if (bitOrder == Endianness.Big) {
                if (byteOrder == Endianness.Big) {
                    return BigByteBigBitIndexer;
                } else {
                    return LittleByteBigBitIndexer;
                }
            } else {
                if (byteOrder == Endianness.Big) {
                    return BigByteLittleBitIndexer;
                } else {
                    return LittleByteLittleBitIndexer;
                }
            }
        }

        private readonly Endianness ByteOrder;
        private readonly Endianness BitOrder;

        /// <summary>
        /// Create a bit indexer that will index bits for values stored in the given format.
        /// </summary>
        /// <param name="byteOrder">The byte order.</param>
        /// <param name="bitOrder">The bit order.</param>
        internal EndianBitIndexer(Endianness byteOrder, Endianness bitOrder) {
            ByteOrder = byteOrder;
            BitOrder = bitOrder;
        }

        [Todo("Reduce stack allocations?")]
        [Todo("Reduce branch complexity.")]
        [Todo("Add Caching behavior for this method.")]
        public Int32 GetIndexForBitWithPower(Int32 bitPower, Int32 valueBitWidth) {
            /**
             * THOUGHTS
             * DEFINE X=Computation Time.
             * What is the complexity of this method?
             *      Complexity is O(1).
             *      
             * What happens in this method.
             *      Exactly:
             *          4 Stack Allocations
             *          2 Branches.
             *          2 Comparisons.
             *          1 Modulus Operation
             *          1 Min Operation.
             *          
             * Can this method be parallelized?
             *      Yes, but the overhead for thread startup and delegates, even using the threadpool, would exceed the benfits.
             *      However, assuming non-overheaded threads, The value of byteOffset and bitOffset could be computed in parallel.
             * 
             * Can this method benifit from cacheing?
             *      Probably. Benifit is dependent on the implementation of the chaching mechanism. The benifit is worth it 
             *      if cache retreival<X 
             *  
             */

            Int32 valueByteWidth = valueBitWidth / 8; //stack
            Int32 bitOffset = 0; //stack
            Int32 byteOffset = 0; //stack
            Int32 maxBitOffset = Math.Min(7, valueBitWidth - 1); //stack

            if (ByteOrder == Endianness.Little) {
                byteOffset = (bitPower / 8);
                if (BitOrder == Endianness.Little) {
                    bitOffset = bitPower % 8;
                } else {
                    bitOffset = maxBitOffset - bitPower % (maxBitOffset + 1);
                }
            } else {
                byteOffset = (valueByteWidth - 1) - (bitPower / 8);
                if (BitOrder == Endianness.Little) {
                    bitOffset = bitPower % 8;
                } else {
                    bitOffset = maxBitOffset - bitPower % (maxBitOffset + 1);
                }
            }

            return byteOffset * 8 + bitOffset;
        }
    }
}
