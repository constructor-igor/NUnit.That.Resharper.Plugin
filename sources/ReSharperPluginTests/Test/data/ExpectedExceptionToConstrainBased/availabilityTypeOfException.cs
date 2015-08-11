using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTypeOfExceptionTests
    {
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test1Exception()
        {
            foo1();
            {on}foo2();
            foo1();
        }

        [Test]
        [ExpectedException({on}typeof( NotImplementedException))]
        public void Test2Exception()
        {
            foo1();
        }

        [Test]
        [ExpectedException({on}typeof (NotImplementedException))]
        public void Test3Exception()
        {
            foo1();
        }

        void foo1()
        {
            {off}
        }
        void foo2()
        {
            throw new NotImplementedException();
        }

        [Test]
        [{off}ExpectedException(typeof(ProductDllException), ExpectedMessage = "Export requires 2 persons, but found 1.")]
        public void Export_ExpectedException()
        {
            using (TemporaryDisposableClass temporaryFile = new TemporaryDisposableClass())
            {
                foo1();
            }
        }

    }
}
