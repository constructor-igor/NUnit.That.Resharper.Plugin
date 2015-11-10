using System;
using Xunit;

namespace xunit_samples
{
    public class XUnitSampleTests
    {
        [Fact]
        public void EqualTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void ExpectedExceptionTest()
        {
            Assert.ThrowsAny<Exception>(() => Div(2, 0));
            Assert.Throws<ArgumentException>(() => Div(2, 0));
        }

        [Fact]
        //[ExpectedException]
        public void ExpectedExceptionTestV1()
        {
            double r = DivException(2, 0);
        }
        [Fact]
        //[ExpectedException(typeof(ArgumentException))]
        public void ExpectedExceptionTestV1_with_type()
        {
            double r = DivArgumentException(2, 0);
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        double Div(double x, double y)
        {
            if (y==0)
                throw new ArgumentException();
            return x/y;
        }
        double DivException(double x, double y)
        {
            if (y == 0)
                throw new Exception();
            return x / y;
        }
        double DivArgumentException(double x, double y)
        {
            if (y == 0)
                throw new ArgumentException();
            return x / y;
        }
    }
}
