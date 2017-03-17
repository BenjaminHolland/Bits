using Phi.Core;
using System;
using System.Collections.Generic;


namespace Phi.Numerics.BitView {

    /// <summary>
    /// Provides bit-level referential access to an array of bytes. 
    /// Allows enumeration as though underlying bytes were a bitset in little endian bit order. 
    /// </summary>
    /// 
    [Todo("Add IReadOnlyList<Boolean>")]
    public interface IBitView : IEnumerable<Boolean> {
       
        /// <summary>
        /// The underlying byte array.
        /// </summary>
        Byte[] Source { get; }

        /// <summary>
        /// The method used to map bit powers to bit indecies.
        /// </summary>
        IBitIndexer Indexer { get; }
        
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
        /// <param name="dst">The view to write to.</param>
        /// <param name="srcOffset">Where to start reading from.</param>
        /// <param name="dstOffset">Where to start writing to.</param>
        /// <param name="length">The number of bits to transfer.</param>
        void WriteTo(IBitView dst, Int32 srcOffset, Int32 dstOffset, Int32 length);

        
        /// <summary>
        /// Read from another <code>IBitView</code>
        /// </summary>
        /// <param name="src">The view to read from.</param>
        /// <param name="srcOffset">Where to start reading from.</param>
        /// <param name="dstOffset">Where to start writing to.</param>
        /// <param name="length">The number of bits to transfer.</param>
        void ReadFrom(IBitView src, Int32 srcOffset, Int32 dstOffset, Int32 length);


    }
}
