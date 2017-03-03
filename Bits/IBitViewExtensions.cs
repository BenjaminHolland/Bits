using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phi.Assert;
namespace Phi.Numerics.Bits {
    public static class IBitViewExtensions {

        public static IEnumerable<IBitView> Shatter(this IBitView @this,IEnumerable<Int32> sizes) {
            Int32 idx = 0;
            foreach (var size in sizes) {
                yield return @this.CreateSubview(idx, size);
                idx += size;
            }
        }

        static readonly Int32[] AcceptableIntegerSizes = new Int32[]{1,2,4,8}; 
        
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
        public static Byte[] ToIntegerBytes(this IBitView @this,Int32 integerSize,Boolean signed) {
            if (@this == null) {
                throw new ArgumentNullException(nameof(@this));
            }
            if (!AcceptableIntegerSizes.Contains(integerSize)) {
                throw new ArgumentException("Invalid integer size.", nameof(integerSize));
            }
            if (integerSize < (@this.Length + 7) / 8) {
                throw new ArgumentOutOfRangeException(nameof(integerSize));
            }

            Byte[] integerBuffer = new Byte[integerSize];
            BitView integerWindow = new BitView(integerBuffer);
            @this.WriteTo(integerWindow,0,0,@this.Length);

            if(signed) {
                if(@this[@this.Length-1]) {
                    for(int bitPow = @this.Length;bitPow<integerWindow.Length;bitPow++) {
                        integerWindow[bitPow]=true;
                    }
                }
            }
            return integerBuffer;
        }
    }
}
