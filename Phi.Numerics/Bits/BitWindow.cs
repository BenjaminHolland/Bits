//using Phi.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace Phi.Numerics.Bits {

    /// <summary>
    /// Provides bit-level referential access to an array of bytes. 
    /// Allows enumeration as though underlying bytes were a bitset in little endian bit order. 
    /// </summary>
    //[Todo("Add support for serialization.")]
    public class BitWindow : IBitWindow {

        private static void EnsureProperConstruction(Int32 length, IBitIndexer indexer) {
            if (length <= 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (indexer == null) {
                throw new ArgumentNullException(nameof(length));
            }
        }

        private static void EnsureProperConstruction(Byte[] source, Int32 offset, Int32 length, IBitIndexer indexer) {
            if (indexer == null) {
                throw new ArgumentNullException(nameof(indexer));
            }
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (source.Length <= 0) {
                throw new ArgumentException($"{nameof(source)} has no elements.", nameof(source));
            }
            if (offset < 0) {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            Int32 requiredBits = offset + length;
            Int32 bitsInBytes = source.Length * 8;

            if (bitsInBytes < requiredBits) {
                throw new ArgumentOutOfRangeException("length");
            }
        }

        private static void EnsureProperConstruction(Byte[] source, IBitIndexer indexer) {
            if (indexer == null) {
                throw new ArgumentNullException(nameof(indexer));
            }
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (source.Length <= 0) {
                throw new ArgumentException($"{nameof(source)} has no elements.", nameof(source));
            }
            EnsureProperConstruction(source, 0, source.Length, indexer);
        }

        private void Initialize(Int32 length, IBitIndexer indexer) {
            Initialize(new byte[(length + 7) / 8], 0, length, indexer);
        }

        private void Initialize(Byte[] source, IBitIndexer indexer) {
            Initialize(source, 0, source.Length * 8, indexer);
        }

        private void Initialize(Byte[] source, Int32 offset, Int32 length, IBitIndexer indexer) {
            Indexer = indexer;
            Source = source;
            Length = length;
            Offset = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean GetBit(int bit) {
            bit += Offset;
            return (Source[bit / 8] & unchecked((byte)(1 << (7 - (bit % 8))))) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetBit(int bit) {
            bit += Offset;
            Source[bit / 8] |= unchecked((byte)(1 << (7 - (bit % 8))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearBit(int bit) {
            bit += Offset;
            Source[bit / 8] &= unchecked((byte)~(1 << (7 - (bit % 8))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AssignBit(int bit, bool value) {
            if (value) SetBit(bit); else ClearBit(bit);
        }

        /// <summary>
        /// Access the value of the bit with the given power.
        /// </summary>
        /// <param name="bitPower">The power of the bit to access.</param>
        /// <returns>Whether or not the bit is set.</returns>
        public Boolean this[Int32 bitPower] {
            get {
                return GetBit(Indexer.GetIndexForBitWithPower(bitPower, Length));
            }

            set {
                AssignBit(Indexer.GetIndexForBitWithPower(bitPower, Length), value);
            }
        }
        /// <summary>
        /// The method used to map bit powers to bit indecies.
        /// </summary>
        public IBitIndexer Indexer { get; private set; }

        /// <summary>
        /// The underlying byte array.
        /// </summary>
        public Byte[] Source { get; internal set; }

        /// <summary>
        /// The length of the view in bits.
        /// </summary>
        public Int32 Length {
            get;
            private set;
        }
        /// <summary>
        /// The bit offset of the view.
        /// </summary>
        public Int32 Offset {
            get;
            private set;
        }

        /// <summary>
        /// Construct a view of bits.
        /// </summary>
        /// <param name="length">The number of bits in the view.</param>
        public BitWindow(Int32 length)
            : this(length, EndianBitIndexer.NativeBitIndexer) {
        }

        /// <summary>
        /// Construct a view of bits.
        /// </summary>
        /// <param name="length">The number of bits in the view.</param>
        /// <param name="indexer">The <see cref="IBitIndexer"/> to use.</param>
        public BitWindow(Int32 length, IBitIndexer indexer) {
            EnsureProperConstruction(length, indexer);
            Initialize(length, indexer);
        }

        /// <summary>
        /// Construct a view over a sequence of bytes.
        /// </summary>
        /// <param name="source">The sequence of bytes.</param>
        /// <param name="offset">The length of the view.</param>
        /// <param name="length">The offset of the view.</param>
        public BitWindow(Byte[] source, Int32 offset, Int32 length)
            : this(source, offset, length, EndianBitIndexer.NativeBitIndexer) {
        }

        /// <summary>
        /// Construct a view over a sequence of bytes.
        /// </summary>
        /// <param name="source">The sequence of bytes.</param>
        /// <param name="length">The length of the view.</param>
        /// <param name="offset">The offset of the view.</param>
        /// <param name="indexer">An <see cref="IBitIndexer"/> to use to order the bits.</param>
        public BitWindow(Byte[] source, Int32 offset, Int32 length, IBitIndexer indexer) {
            EnsureProperConstruction(source, offset, length, indexer);
            Initialize(source, offset, length, indexer);
        }

        /// <summary>
        /// Construct a view over a sequence of bytes.
        /// </summary>
        /// <param name="source">The sequence of bytes.</param>
        /// <param name="indexer">An <see cref="IBitIndexer"/> to use to order the bits.</param>
        public BitWindow(Byte[] source, IBitIndexer indexer) {
            EnsureProperConstruction(source, indexer);
            Initialize(source, indexer);
        }

        /// <summary>
        /// Construct a view over a sequence of bytes.
        /// </summary>
        /// <param name="source">The sequence of bytes.</param>
        public BitWindow(Byte[] source) : this(source, EndianBitIndexer.NativeBitIndexer) {

        }

        /// <summary>
        /// Gets an enumerator.
        /// </summary>
        /// <returns>An enumerator that will return the bits in this view in little-endian order.</returns>
        public IEnumerator<Boolean> GetEnumerator() {
            for (int idx = 0; idx < Length; idx++) {
                yield return this[idx];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<Boolean>)this).GetEnumerator();
        }

        /// <summary>
        /// Writes to another <code>IBitView</code>.
        /// </summary>
        /// <param name="dst">The view to write to.</param>
        /// <param name="srcOffset">Where to start reading from.</param>
        /// <param name="dstOffset">Where to start writing to.</param>
        /// <param name="length">The number of bits to transfer.</param>
        public void WriteTo(IBitWindow dst, Int32 srcOffset, Int32 dstOffset, Int32 length) {
            if (srcOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }
            if (dstOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }

            Int32 bitsRequiredSrc = srcOffset + length;
            Int32 bitsRequiredDst = dstOffset + length;
            Int32 bitsInDst = dst.Length;

            if (bitsInDst < bitsRequiredDst) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }
            if (Length < bitsRequiredSrc) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }


            for (Int32 offset = 0; offset < length; offset++) {
                dst[offset + dstOffset] = this[offset + srcOffset];
            }
        }


        /// <summary>
        /// Read from another <code>IBitView</code>
        /// </summary>
        /// <param name="src">The view to read from.</param>
        /// <param name="srcOffset">Where to start reading from.</param>
        /// <param name="dstOffset">Where to start writing to.</param>
        /// <param name="length">The number of bits to transfer.</param>
        public void ReadFrom(IBitWindow src, Int32 srcOffset, Int32 dstOffset, Int32 length) {

            if (srcOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }
            if (dstOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }

            Int32 bitsRequiredSrc = srcOffset + length;
            Int32 bitsRequiredDst = dstOffset + length;
            Int32 bitsInSrc = src.Length;

            if (bitsInSrc < bitsRequiredSrc) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }
            if (Length < bitsRequiredDst) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }

            for (Int32 offset = 0; offset < length; offset++) {
                this[offset + dstOffset] = src[offset + srcOffset];
            }
        }

        /// <summary>
        /// Convert to a string.
        /// </summary>
        /// <returns>The sequence of bits in this view in big-endian order.</returns>
        public override String ToString() {

            Char[] str = new Char[Length];
            for (int i = Length - 1; i >= 0; i++) {
                str[Length - 1 - i] = this[i] ? '1' : '0';
            }
            return new string(str);


        }
    }
}
