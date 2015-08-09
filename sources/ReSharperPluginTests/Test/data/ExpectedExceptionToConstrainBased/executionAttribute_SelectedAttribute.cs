using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleAttributesTests
    {
        [Test]
        [ExpectedException{caret}]
        public void Test1Exception()
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
