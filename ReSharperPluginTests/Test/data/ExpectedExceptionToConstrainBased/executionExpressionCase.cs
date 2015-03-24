using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTypeOfExceptionTests
    {
        [Test]
        [ExpectedException]
        public void TestExpectedExceptionWithExpressions()
        {
            double i = {caret}2 + getNumber();
        }

        double getNumber()
        {
            throw new NotImplementedException();
        }
    }
}
