using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTypeOfExceptionTests
    {
        [Test]
        [ExpectedException(typeof(NotImplementedException), ExpectedMessage = "customer message")]
        public void TestExpectedExceptionWithExpressions()
        {
            {caret}foo4("customer message");
        }

        void foo4(string customerExceptionMessage)
        {
            throw new NotImplementedException(customerExceptionMessage);
        }
    }
}
