using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Phi.Assert;
using Phi.Numerics.Bits.Extensions;
namespace Phi.Numerics.Bits {
    public class BitView : IBitView {
        IBitIndexer Indexer;
        Byte[] Source;
        Int32 InternalLength;
        Int32 InternalOffset;

        void Initialize(Int32 length,IBitIndexer indexer) {
            Initialize(new byte[(length+7)/8],0,length,indexer);
        }

        void Initialize(Byte[] source,IBitIndexer indexer) {
            Initialize(source,0,source.Length*8,indexer);
        }

        void Initialize(Byte[] source,Int32 offset,Int32 length,IBitIndexer indexer) {
            Indexer=indexer;
            Source=source;
            InternalLength=length;
            InternalOffset=offset;
        }

        static void EnsureProperConstruction(Int32 length,IBitIndexer indexer) {
            if (length <= 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (indexer == null) {
                throw new ArgumentNullException(nameof(length));
            }
        }

        static void EnsureProperConstruction(Byte[] source,Int32 offset,Int32 length,IBitIndexer indexer) {
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
            
            Int32 requiredBits = offset+length;
            Int32 bitsInBytes = source.Length*8;

            if (bitsInBytes < requiredBits) {
                throw new ArgumentOutOfRangeException("length");
            }
        }

        static void EnsureProperConstruction(Byte[] source,IBitIndexer indexer) {
            if (indexer == null) {
                throw new ArgumentNullException(nameof(indexer));
            }
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (source.Length <= 0) {
                throw new ArgumentException($"{nameof(source)} has no elements.",nameof(source));
            }
            EnsureProperConstruction(source,0,source.Length,indexer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Boolean GetBit(int bit) {
            bit += InternalOffset;
            return (Source[bit / 8] & unchecked((byte)(1 << (7 - (bit % 8))))) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SetBit(int bit) {
            bit += InternalOffset;
            Source[bit / 8] |= unchecked((byte)(1 << (7 - (bit % 8))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ClearBit(int bit) {
            bit += InternalOffset;
            Source[bit / 8] &= unchecked((byte)~(1 << (7 - (bit % 8))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AssignBit(int bit, bool value) {
            if (value) SetBit(bit); else ClearBit(bit);
        }

        public Boolean this[Int32 bitPower] {
            get {
                return GetBit(Indexer.GetIndexForBitWithPower(bitPower, InternalLength));
            }

            set {
                AssignBit(Indexer.GetIndexForBitWithPower(bitPower, InternalLength), value);
            }
        }
        
        public Int32 Length {
            get { return InternalLength; }
        }
        
        
        public Int32 Offset {
            get { return InternalOffset; }
        }
        
        public IBitView CreateSubview(Int32 offset, Int32 length) {
            return CreateSubview(offset, length, Indexer);
        }

        public IBitView CreateSubview(Int32 offset, Int32 length, IBitIndexer subviewIndexer) {
            return new BitView(Source, InternalOffset + offset, length, subviewIndexer);
        }

        public IEnumerator<Boolean> GetEnumerator() {
            return Enumerable.Range(0, InternalLength).Select(bitPower => this[bitPower]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<Boolean>)this).GetEnumerator();
        }

        public void WriteTo(Byte[] dst,Int32 srcOffset,Int32 dstOffset,Int32 length,IBitIndexer indexer) {
            IBitView dstWindow =dst.AsBitView(indexer);
            WriteTo(dstWindow,srcOffset,dstOffset,length);
        }

        public void WriteTo(IBitView dst, Int32 srcOffset, Int32 dstOffset, Int32 length) {
            if (srcOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }
            if (dstOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }

            Int32 bitsRequiredSrc = srcOffset+length;
            Int32 bitsRequiredDst = dstOffset+length;
            Int32 bitsInDst = dst.Length;

            if (bitsInDst < bitsRequiredDst) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }
            if (InternalLength < bitsRequiredSrc) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }
          

            for(Int32 offset = 0; offset < length; offset++) {
                dst[offset + dstOffset] = this[offset + srcOffset];
            }
        }

        public void ReadFrom(IBitView src, Int32 srcOffset, Int32 dstOffset, Int32 length) {

            if (srcOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }
            if (dstOffset < 0) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }

            Int32 bitsRequiredSrc = srcOffset+length;
            Int32 bitsRequiredDst = dstOffset+length;
            Int32 bitsInSrc = src.Length;


            if (bitsInSrc < bitsRequiredSrc) {
                throw new ArgumentOutOfRangeException(nameof(dstOffset));
            }
            if (InternalLength < bitsRequiredDst) {
                throw new ArgumentOutOfRangeException(nameof(srcOffset));
            }
        
            for (Int32 offset = 0; offset < length; offset++) {
                this[offset + dstOffset] = src[offset + srcOffset];
            }
        }

        public void ReadFrom(Byte[] src,Int32 srcOffset,Int32 dstOffset,Int32 length,IBitIndexer indexer) {
            IBitView srcWindow = src.AsBitView(indexer);
            WriteTo(srcWindow,srcOffset,dstOffset,length);
        }
        
       
        public BitView(Int32 length) 
            : this(length,EndianBitIndexer.NativeBitIndexer) {

        }
        public BitView(Int32 length,IBitIndexer indexer) {
            EnsureProperConstruction(length,indexer);
            Initialize(length,indexer);
        }

        public BitView(Byte[] source,Int32 offset,Int32 length) 
            : this(source,offset,length,EndianBitIndexer.NativeBitIndexer) {

        }

        public BitView(Byte[] source, Int32 offset, Int32 length, IBitIndexer indexer) {
            EnsureProperConstruction(source,offset,length,indexer);
            Initialize(source,offset,length,indexer);
            
            
        }

        public BitView(Byte[] source,IBitIndexer indexer) {
            
            EnsureProperConstruction(source,indexer);
            Initialize(source,indexer);
        }
        public BitView(Byte[] source):this(source,EndianBitIndexer.NativeBitIndexer) {
            
        }
        public override String ToString() {
            return new string(this.Select(b => b ? '1' : '0').ToArray());
        }
    }
}
