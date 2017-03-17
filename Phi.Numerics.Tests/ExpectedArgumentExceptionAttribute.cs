using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Phi.Numerics.Testing {
    public class ExpectedArgumentExceptionAttribute:ExpectedExceptionBaseAttribute {
        public String ParamName {
            get;
            private set;
        }
        public Type ExceptionType {
            get;
            private set;
        }
        public ExpectedArgumentExceptionAttribute(Type exceptionType,String paramName)
            : base() {

            if(!(exceptionType==typeof(ArgumentException)||exceptionType.IsSubclassOf(typeof(ArgumentException)))) {
                throw new InvalidOperationException();
            }
            ParamName=paramName;
            ExceptionType=exceptionType;
        }
        public ExpectedArgumentExceptionAttribute(Type exceptionType,String noExceptionMessage,String paramName)
            : base(noExceptionMessage) { }
        protected override void Verify(Exception exception) {
            if(!(exception.GetType()==ExceptionType||exception.GetType().IsSubclassOf(ExceptionType))) {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Exception is not of expected type");
            }else {
                var argEx = exception as ArgumentException;
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(ParamName, argEx.ParamName);
            }
        }
    }
}
