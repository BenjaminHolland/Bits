using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phi.Numerics.Bits {

    /// <summary>
    /// Provides bit-level referential access to an array of bytes. 
    /// Allows enumeration as though underlying bytes were a bitset in little endian bit order. 
    /// </summary>
    public interface IBitView : IEnumerable<Boolean> {
        
        /// <summary>
        /// The bit offset of the view.
        /// </summary>
        Int32 Offset { get; }

        /// <summary>
        /// The length of the view in bits.
        /// </summary>
        Int32 Length { get; }

        /// <summary>
        /// Access the value of the bit with the given power.
        /// </summary>
        /// <param name="bitPower">The power of the bit to access.</param>
        /// <returns>Whether or not the bit is set.</returns>
        Boolean this[Int32 bitPower] {
            get;
            set;
        }
        
        /// <summary>
        /// Writes to another <code>IBitView</code>.
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="srcOffset"></param>
        /// <param name="dstOffset"></param>
        /// <param name="length"></param>
        void WriteTo(IBitView dst, Int32 srcOffset, Int32 dstOffset, Int32 length);

        /// <summary>
        /// Writes bits to the given byte array.
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="srcOffset"></param>
        /// <param name="dstOffset"></param>
        /// <param name="length"></param>
        /// <param name="indexer"></param>
        void WriteTo(Byte[] dst,Int32 srcOffset,Int32 dstOffset,Int32 length,IBitIndexer indexer);

        /// <summary>
        /// Read from another <code>IBitView</code>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcOffset"></param>
        /// <param name="dstOffset"></param>
        /// <param name="length"></param>
        void ReadFrom(IBitView src, Int32 srcOffset, Int32 dstOffset, Int32 length);

        void ReadFrom(Byte[] src,Int32 srcOffset,Int32 dstOffset,Int32 length,IBitIndexer indexer);

        /// <summary>
        /// Creates a subwindow into the same underlying byte array.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        IBitView CreateSubview(Int32 offset, Int32 length);

        
        /// <summary>
        /// Creates a subwindow into the same underlying array, but uses a different indexer to map bit power values to bit indecies.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="indexer"></param>
        /// <returns></returns>
        IBitView CreateSubview(Int32 offset, Int32 length, IBitIndexer indexer);
    }
}
