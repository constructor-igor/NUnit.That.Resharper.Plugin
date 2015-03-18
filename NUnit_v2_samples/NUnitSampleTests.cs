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
            string actual = null;
            Assert.IsNullOrEmpty(actual);
        }
    }
}
