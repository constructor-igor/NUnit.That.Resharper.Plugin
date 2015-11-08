using JetBrains.ReSharper.FeaturesTestFramework.Intentions;
using NUnit.Framework;
using NUnit.That.Resharper_v10.Plugin;

namespace ReSharperPluginTests
{
    [TestFixture]
    public class ContextActionExecutionTest1 : CSharpContextActionExecuteTestBase<ExpectedExceptionToConstrainBased>
    {
        protected override string ExtraPath
        {
            get { return "ExpectedExceptionToConstrainBased"; }
        }

        protected override string RelativeTestDataPath
        {
            get { return "ExpectedExceptionToConstrainBased"; }
        }

        [TestCase("executionAttribute.cs")]
        [TestCase("executionTypeOfException.cs")]
        [TestCase("executionExpressionCase.cs")]
        [TestCase("executionCustomerMessage.cs")]
        [TestCase("executionExpectedException.cs")]
        [TestCase("executionAttribute_SelectedAttribute.cs")]
        [TestCase("executionAttribute_Issue-029.cs")]
        [TestCase("executionAttribute_Issue-030.cs")]
        public void TestCases(string testSrc)
        {
            DoTestFiles(testSrc);
        }

    }
}
