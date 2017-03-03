using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phi.Numerics.Bits;
namespace Phi.Numerics {
    public static class Integer {

        public static Int32 GetDigit(this Int32 @this,UInt32 digitPower) {
            return ((Int32)Math.Floor(@this / Math.Pow(10, digitPower))) % 10;
        }

        public static String GetBits(this Byte @this) {
            Byte[] bits = new byte[] { @this };
            return (new BitView(bits)).ToString();
        } 
    }
}
