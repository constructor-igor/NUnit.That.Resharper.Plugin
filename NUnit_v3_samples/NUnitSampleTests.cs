using System;
using NUnit.Framework;

namespace NUnit_v3_samples
{
    [TestFixture]
    public class NUnitSampleTests
    {
        [Test]
        public void AssertAreEqual()
        {
            int expected = 3;
            int actual = 2 + 1;
            Assert.That(actual, Is.EqualTo(expected));      // was in nunit v2 Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AssertIsNullOrEmpty()
        {
            string actual = null;
            Assert.That(actual, Is.Null.Or.Empty);          // was in nunit v2. Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void TestException()
        {
            foo1();
            Assert.That(foo2, Throws.Exception);
            foo1();
        }
        [Test]
        public void TestConcreteException()
        {
            foo1();
            Assert.That(foo2, Throws.TypeOf<NotImplementedException>());
            Assert.That(foo2, Throws.TypeOf(typeof(NotImplementedException)));
            foo1();
        }
        [Test]
        public void TestConcreteByGeneralException()
        {
            foo1();
            Assert.That(foo2, Throws.InstanceOf(typeof(Exception)));
            Assert.Throws<AssertionException>(() => Assert.That(foo2, Throws.TypeOf(typeof (Exception))));
            foo1();
        }

        [Test]
        public void TestExpectedExceptionWithExpressions()
        {
            Assert.That(() => { double i = 2 + getNumber(); }, Throws.Exception);
        }

        void foo1()
        {
        }
        void foo2()
        {
            throw new NotImplementedException();
        }

        double getNumber()
        {
            throw new NotImplementedException();
        }
    }
}
