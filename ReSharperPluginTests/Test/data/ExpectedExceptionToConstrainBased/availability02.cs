using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTests
    {
        [Test]
        [ExpectedException]
        public void TestException()
        {
            foo1();
            foo2();
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
    }
}
