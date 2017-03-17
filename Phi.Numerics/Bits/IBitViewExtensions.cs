
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phi.Numerics.BitView {

    /// <summary>
    /// Provides Extended IBitView Capabilities.
    /// </summary>
    /// 

    //[Todo("Add Shatter(IBitView,[MARSHALABLESTRUCTURE])")]
    public static class IBitViewExtensions {
        private static readonly HashSet<IntegerFormat> SupportedIntegerFormats = new HashSet<IntegerFormat>() {
            IntegerFormat.S8,
            IntegerFormat.S16,
            IntegerFormat.S32,
            IntegerFormat.S64,
            IntegerFormat.U8,
            IntegerFormat.U16,
            IntegerFormat.U32,
            IntegerFormat.U64
        };
        
        public static void WriteTo(this IBitView view,Byte[] dst, Int32 srcOffset, Int32 dstOffset, Int32 length, IBitIndexer indexer) {
            IBitView dstWindow = new BitView(dst, indexer);
            view.WriteTo(dstWindow, srcOffset, dstOffset, length);
        }
        
         public static void ReadFrom(this IBitView view,Byte[] src,Int32 srcOffset,Int32 dstOffset,Int32 length,IBitIndexer indexer) {
            IBitView srcWindow = new BitView(src,indexer);
            view.WriteTo(srcWindow,srcOffset,dstOffset,length);
        }

        /// <summary>
        /// Creates a view into an existing view.
        /// </summary>
        /// <param name="this">The existing view.</param>
        /// <param name="offset">The bit offset relative to the start of the existing view.</param>
        /// <param name="format">A format to follow.</param>
        /// <returns></returns>
        public static IBitView Crack(this IBitView @this,Int32 offset,IntegerFormat format) {
            return @this.Crack(offset, format.BitWidth, format.GetIndexer());
        }

        /// <summary>
        /// Creates a view into an existing view.
        /// </summary>
        /// <param name="view">The existing view.</param>
        /// <param name="offset">The bit offset relative to the start of the existing view.</param>
        /// <param name="length">The length of the new view.</param>
        /// <param name="indexer">The bit indexer to use.</param>
        /// <returns></returns>
        public static IBitView Crack(this IBitView view, Int32 offset, Int32 length, IBitIndexer indexer) {
            return new BitView(view.Source, view.Offset + offset, length, indexer);
        }

        /// <summary>
        /// Creates a sequence of formatted views into an existing view.
        /// </summary>
        /// <param name="this">The existing view.</param>
        /// <param name="formats">The sequence of formats that the new views will have.</param>
        /// <returns></returns>
        public static IEnumerable<IBitView> Shatter(this IBitView @this,IEnumerable<IntegerFormat> formats) {
            Int32 pos = 0;
            foreach(var fmt in formats) {
                yield return @this.Crack(pos, fmt.BitWidth, fmt.GetIndexer());
                pos += fmt.BitWidth;
            }   
        }

        /// <summary>
        /// Creates a sequence of views into an existing view.
        /// </summary>
        /// <param name="this">The existing view.</param>
        /// <param name="sizes">The sequence of view sizes.</param>
        /// <returns></returns>
        public static IEnumerable<IBitView> Shatter(this IBitView @this,IEnumerable<Int32> sizes) {
           Int32 pos = 0;
            foreach (var size in sizes) {
                yield return @this.Crack(pos, size,@this.Indexer);
                pos += size;
            }
        }
        
                
        /// <summary>
        /// Get the bytes of this IBitWindow as though the bits in the window were a single N-bit integer with a layout that matches
        /// that of the current processor. 
        /// Currently restricted to 1,2,4, and 8 byte sized integers. 
        /// Currently restricted to unsigned or 2's complement format.  
        /// </summary>
        /// <param name="this"></param>
        /// <param name="integerSize">The byte width of the resulting integer.</param>
        /// <param name="signed">Whether or not the resulting integer is signed.</param>
        /// <returns></returns>
        public static Byte[] Crystalize(this IBitView @this,IntegerFormat format) {
            if (@this == null) {
                throw new ArgumentNullException(nameof(@this));
            }
            if (!SupportedIntegerFormats.Contains(format)) {
                throw new NotSupportedException();
            }
            
            if (format.BitWidth / 8 < (@this.Length + 7) / 8) {
                throw new InvalidOperationException();
            }
            Byte[] integerBuffer = new Byte[format.BitWidth / 8];
            BitView integerView = new BitView(integerBuffer, new EndianBitIndexer(format.ByteOrder, format.BitOrder));
            @this.WriteTo(integerView, 0, 0, @this.Length);
            if (format.Signed) {
                if (@this[@this.Length - 1]) {
                    for (int bitPow = @this.Length; bitPow < integerView.Length; bitPow++) {
                        integerView[bitPow] = true;
                    }
                }
            }
            return integerBuffer;
        }
    }
}
