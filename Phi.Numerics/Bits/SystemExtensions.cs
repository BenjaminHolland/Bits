//using Phi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Phi.Numerics.BitView {

    /// <summary>
    /// Provides IBitView Related extensions for the standard library.
    /// </summary>
  //  [Todo("Add Compatibility With System.Collections.BitArray")]
   // [Todo("Add Compatibility with System.Numerics.BigInteger")]
    public static class SystemExtensions {

     //   [Todo("Make sure this is the right indexer to use.")]
        public static IBitView ToBitView(this String @this,Encoding encoding) {
            return encoding.GetBytes(@this).AsBitView(EndianBitIndexer.BigByteBigBitIndexer);
        }

       // [Todo("Make sure this is the right indexer to use.")]
        public static IBitView ToBitView(this Char @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this UInt64 @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this Int64 @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this Int32 @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this UInt32 @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this Int16 @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this UInt16 @this) {
            return BitConverter.GetBytes(@this).AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this Byte @this) {
            return new byte[] { @this }.AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Create an IBitView from an object.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IBitView ToBitView(this SByte @this) {
            return new byte[] { (byte)@this}.AsBitView(EndianBitIndexer.NativeBitIndexer);
        }

        /// <summary>
        /// Provides bit-level access to the array using the given indexer.
        /// </summary>
        /// <param name="this">The byte array to be transformed.</param>
        /// <param name="indexer">The indexer to use for byte indexing.</param>
        /// <returns></returns>
        public static IBitView AsBitView(this Byte[] @this,IBitIndexer indexer) {
            if (@this == null) {
                throw new ArgumentNullException(nameof(@this));
            }
            if (indexer == null) {
                throw new ArgumentNullException(nameof(indexer));
            }
            return new BitView(@this,0,@this.Length*8,indexer);
        }
    }
}
