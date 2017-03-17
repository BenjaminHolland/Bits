
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phi.Numerics.BitView
{
    public static class IntegerFormatExtensions
    {
        /// <summary>
        /// Gets a bit indexer for this integer format.
        /// </summary>
        /// <param name="fmt">The existing format</param>
        /// <returns>An indexer for reading an integer in the specified format</returns>
        public static IBitIndexer GetIndexer(this IntegerFormat fmt) {
            return EndianBitIndexer.GetStandard(fmt.BitOrder, fmt.ByteOrder);
        }
    }
}
