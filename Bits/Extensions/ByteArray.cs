using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phi.Assert;
namespace Phi.Numerics.Bits.Extensions {
    public static class ByteArrayExtensions {
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
