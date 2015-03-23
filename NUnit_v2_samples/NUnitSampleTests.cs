using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTests
    {
        [Test]
        public void AssertAreEqual()
        {
            int expected = 3;
            int actual = 2 + 1;
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void AssertIsNullOrEmpty()
        {
            const string CUSTOM_MESSAGE = "testing of 3 parameters";
            string actual = null;
            Assert.IsNullOrEmpty(actual);
            Assert.IsNullOrEmpty(actual, "error message");
            Assert.IsNullOrEmpty(actual, "custom message: {0}.", CUSTOM_MESSAGE);
        }

        [Test]
        [Repeat(10)]
        public void RepeatTest()
        {
            Assert.Pass("testing attribute 'Repeat' ");
        }

        [Test]
        [ExpectedException]
        public void TestShortExpectedException()
        {
            foo1();
            foo2();
            foo1();
        }
        [Test, ExpectedException]
        public void TestAndShortExpectedException()
        {
            foo1();
            foo2();
            foo1();
        }

        [Test]
        [NUnit.Framework.ExpectedException]
        public void TestFullExpectedException()
        {
            foo1();
            foo2();
            foo1();
        }

        [Test]
        [NUnit.Framework.ExpectedExceptionAttribute]
        public void TestFullExpectedExceptionAttribute()
        {
            foo1();
            foo2();
            foo1();
        }

        void foo1()
        {            
        }
        void foo2()
        {
            throw new NotImplementedException();
        }
    }
}
