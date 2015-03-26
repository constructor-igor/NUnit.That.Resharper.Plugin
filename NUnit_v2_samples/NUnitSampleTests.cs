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
            Assert.IsNullOrEmpty(actual, "custom message: {0} and {1}.", CUSTOM_MESSAGE, 10);
        }
        [Test]
        public void AssertIsNotNullOrEmpty()
        {
            const string CUSTOM_MESSAGE = "testing of 3 parameters";
            const string actual = "not null or empty";
            Assert.IsNotNullOrEmpty(actual);
            Assert.IsNotNullOrEmpty(actual, "error message");
            Assert.IsNotNullOrEmpty(actual, "custom message: {0}.", CUSTOM_MESSAGE);
            Assert.IsNotNullOrEmpty(actual, "custom message: {0} and {1}.", CUSTOM_MESSAGE, 10);
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

        [Test]
        [ExpectedException(typeof(NotImplementedException), ExpectedException = typeof(NotImplementedException))]
        public void TestExpectedExceptionWithType()
        {
            foo1();
            foo2();
            foo1();
        }
//        [Test]
//        [ExpectedException(typeof(Exception))]
//        public void TestExpectedExceptionWithBasicType()
//        {
//            foo1();
//            foo2();
//            foo1();
//        }

        [Test]
        [ExpectedException]
        public void TestExpectedExceptionWithExpressions()
        {
            double i = 2 + getNumber();
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException), ExpectedMessage = "customer message")]
        public void TestExpectedExceptionWithCustomerMessage()
        {
            foo4("customer message");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedException = typeof(NotImplementedException))]
        public void TestExpectedExceptionWithExpectedExceptionProperty()
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
        void foo4(string customerExceptionMessage)
        {
            throw new NotImplementedException(customerExceptionMessage);
        }

        double getNumber()
        {
            throw new NotImplementedException();
        }
    }
}
