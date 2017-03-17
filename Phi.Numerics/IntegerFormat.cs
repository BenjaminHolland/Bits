//using Phi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Phi.Numerics {
    /// <summary>
    /// Describes a type of integer.
    /// </summary>
    public class IntegerFormat : IEquatable<IntegerFormat> {
        /// <summary>
        /// Format for an Unsigned 8-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat U8 = new IntegerFormat(Endianness.Little, Endianness.Big, 8, false);

        /// <summary>
        /// Format for a Signed 8-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat S8 = new IntegerFormat(Endianness.Little, Endianness.Big, 8, true);

        /// <summary>
        /// Format for an Unsigned 16-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat U16 = new IntegerFormat(Endianness.Little, Endianness.Big, 16, false);


        /// <summary>
        /// Format for a Signed 16-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat S16 = new IntegerFormat(Endianness.Little, Endianness.Big, 16, true);

        /// <summary>
        /// Format for an Unsigned 32-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat U32 = new IntegerFormat(Endianness.Little, Endianness.Big, 32, false);


        /// <summary>
        /// Format for a Signed 32-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat S32 = new IntegerFormat(Endianness.Little, Endianness.Big, 32, true);

        /// <summary>
        /// Format for an Unsigned 64-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat U64 = new IntegerFormat(Endianness.Little, Endianness.Big, 64, false);


        /// <summary>
        /// Format for a Signed 64-Bit Integer.
        /// </summary>
        public static readonly IntegerFormat S64 = new IntegerFormat(Endianness.Little, Endianness.Big, 64, true);


        /// <summary>
        /// The byte order of the integer.
        /// </summary>
        public Endianness ByteOrder { get; }

        /// <summary>
        /// The bit order of the integer.
        /// </summary>
        public Endianness BitOrder { get; }

        /// <summary>
        /// The width of the integer in bits.
        /// </summary>
        public Int32 BitWidth { get; }

        /// <summary>
        /// Whether the integer is signed.
        /// </summary>
        public Boolean Signed { get; }

        /// <summary>
        /// Creates a new integer format.
        /// </summary>
        /// <param name="byteOrder"></param>
        /// <param name="bitOrder"></param>
        /// <param name="bitWidth"></param>
        /// <param name="signed"></param>
        public IntegerFormat(Endianness byteOrder, Endianness bitOrder, Int32 bitWidth, Boolean signed) {
            if (bitWidth <= 0) {
                throw new ArgumentOutOfRangeException(nameof(bitWidth));
            }
            ByteOrder = byteOrder;
            BitOrder = bitOrder;
            BitWidth = bitWidth;
            Signed = signed;
        }
        
        public override Int32 GetHashCode() {
            int value = 31415;
            value = value * 7 + ByteOrder.GetHashCode();
            value = value * 7 + BitOrder.GetHashCode();
            value = value * 7 + BitWidth.GetHashCode();
            value = value * 7 + Signed.GetHashCode();
            return value;
        }

        public override Boolean Equals(Object obj) {
            if (obj == null) {
                return false;
            }
            var cobj = obj as IntegerFormat;
            return Equals(cobj);
        }

        public Boolean Equals(IntegerFormat other) {
            if (other == null) {
                return false;
            }
            return BitOrder == other.BitOrder && ByteOrder == other.ByteOrder && BitWidth == other.BitWidth && (Signed == other.Signed);
        }
    }
}
