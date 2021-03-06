﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phi.Numerics.Bits {
    
    /// <summary>
    /// Defines an interface for an object that can map form bit powers to bit indecies.
    /// </summary>
    public interface IBitIndexer {
        /// <summary>
        /// Determines the bit index of a bit with a given power.
        /// </summary>
        /// <param name="bitPower">The power for which to find the bit index for.</param>
        /// <param name="valueBitWidth">The width of the underlying value</param>
        /// <returns>The index of the bit.</returns>
        Int32 GetIndexForBitWithPower(Int32 bitPower, Int32 valueBitWidth);
    }
}
