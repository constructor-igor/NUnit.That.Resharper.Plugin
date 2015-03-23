using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleAttributesTests
    {
        [Test]
        [ExpectedException]
        public void Test1Exception()
        {
            foo1();
            {caret}foo2();
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
