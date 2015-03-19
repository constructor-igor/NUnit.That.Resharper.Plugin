using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using NUnit.That.Resharper_v8.Plugin;

namespace ReSharperPluginTests
{
    [TestFixture]
    public class ContextActionAvailabilityTest : CSharpContextActionAvailabilityTestBase<ExpectedExceptionToConstrainBased>
    {
        protected override string ExtraPath
        {
            get { return "ExpectedExceptionToConstrainBased"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return "ExpectedExceptionToConstrainBased"; }
        }

        [TestCase("availability01.cs")]
        [TestCase("availability02.cs")]
        public void TestCases(string testSrc)
        {
            DoTestFiles(testSrc);
        }
    }
}
