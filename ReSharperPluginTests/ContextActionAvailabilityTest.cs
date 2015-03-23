using JetBrains.ReSharper.Intentions.CSharp.Test;
using NUnit.Framework;
using NUnit.That.Resharper_v8.Plugin;

namespace ReSharperPluginTests
{
    //
    //  {on}    - ExpectedExceptionToConstrainBased.IsAvailable - True
    //  {off}   - ExpectedExceptionToConstrainBased.IsAvailable - False
    //
    //  https://www.jetbrains.com/resharper/devguide/Plugins/Testing.html
    //

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

        [TestCase("availabilityAttribute.cs")]
        [TestCase("availabilityTypeOfException.cs")]
        public void TestCases(string testSrc)
        {
            DoTestFiles(testSrc);
        }
    }
}
