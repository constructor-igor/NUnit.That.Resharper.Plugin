using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTests
    {
        [Test]
        [ExpectedException]
        public void Test1Exception()
        {
            foo1();
            {on}foo2();
            foo1();
        }

        [Test, ExpectedException]
        public void Test2Exception()
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
    }
}
