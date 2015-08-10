using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleAttributesTests
    {
        [Test]
        [ExpectedException{on}]
        public void Test1Exception()
        {
            foo1();
            {on}foo2();
            foo1();
        }

        {off}
        [Test, {on}ExpectedException]
        public void Test2Exception(){off}
        {
            foo1();
            {on}foo2();
            foo1();
        }

        [Test]
        [Framework.ExpectedException]
        public void Test3Exception()
        {
            foo1();
            {on}foo2();
            foo1();
        }

        [Test]
        [NUnit.Framework.ExpectedException]
        public void Test4Exception()
        {
            foo1();
            {on}foo2();
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

        [{off}ExpectedException]
        void foo3()
        {            
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
