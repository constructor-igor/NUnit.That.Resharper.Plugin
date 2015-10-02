using System;
using System.IO;
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
            Assert.That(actual, Is.EqualTo(expected));      // was in nunit v2 Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AssertIsNullOrEmpty()
        {
            const string CUSTOM_MESSAGE = "testing of 3 parameters";
            string actual = null;
            Assert.That(actual, Is.Null.Or.Empty);          // was in nunit v2. Assert.IsNullOrEmpty(actual);
            Assert.That(actual, Is.Null.Or.Empty, "error message");
            Assert.That(actual, Is.Null.Or.Empty, "custom message: {0}.", CUSTOM_MESSAGE);
            Assert.That(actual, Is.Null.Or.Empty, "custom message: {0} and {1}.", CUSTOM_MESSAGE, 10);
        }
        [Test]
        public void AssertIsNotNullOrEmpty()
        {
            const string CUSTOM_MESSAGE = "testing of 3 parameters";
            const string actual = "not null or empty";
            Assert.That(actual, Is.Not.Null.Or.Empty);
            Assert.That(actual, Is.Not.Null.Or.Empty, "error message");
            Assert.That(actual, Is.Not.Null.Or.Empty, "custom message: {0}.", CUSTOM_MESSAGE);
            Assert.That(actual, Is.Not.Null.Or.Empty, "custom message: {0} and {1}.", CUSTOM_MESSAGE, 10);
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
            Assert.Throws<AssertionException>(() => Assert.That(foo2, Throws.TypeOf(typeof(Exception))));
            foo1();
        }

        [Test]
        public void TestExpectedExceptionWithExpressions()
        {
            Assert.That(() => { double i = 2 + getNumber(); }, Throws.Exception);
        }

        [Test]
        public void TestExpectedExceptionWithCustomerMessage()
        {
            Assert.That(() => { foo4("customer message"); }, Throws.TypeOf(typeof(NotImplementedException)).And.Message.EqualTo("customer message"));
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

        [Test]
        public void Test_OpenFolder_Navigation()
        {
            string fileName1 = @"d:\file.txt";
            const string FILE_NAME2 = "d:\\file.txt";

            Foo(@"d:\file.txt");

            Assert.That(File.Exists(fileName1), Is.True);
            Assert.That(File.Exists(FILE_NAME2), Is.True);
        }

        void Foo(string fileName)
        {
            
        }
    }
}
