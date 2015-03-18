using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture]
    public class NUnitSampleTests
    {
        [Test]
        public void AssertIsNullOrEmpty()
        {
            string actual = null;
            Assert.IsNullOrEmpty(actual);
        }
    }
}
