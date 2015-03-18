using NUnit.Framework;

namespace NUnit_v3_samples
{
    [TestFixture]
    public class NUnitSampleTests
    {
        [Test]
        public void AssertIsNullOrEmpty()
        {
            string actual = null;
            Assert.That(actual, Is.Null.Or.Empty);          // was in nunit v2. Assert.IsNullOrEmpty(actual);
        }
    }
}
