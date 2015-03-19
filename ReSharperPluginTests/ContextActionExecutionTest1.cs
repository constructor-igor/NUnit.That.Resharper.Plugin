using JetBrains.ReSharper.Intentions.CSharp.Test;
using JetBrains.ReSharper.Intentions.Test;
using NUnit.Framework;
using NUnit.That.Resharper_v8.Plugin;

namespace ReSharperPluginTests
{
    [TestFixture]
    public class ContextActionExecutionTest1 : CSharpContextActionAvailabilityTestBase<ExpectedExceptionToConstrainBased>
    {
        protected override string ExtraPath
        {
            get { return "ClassName"; }
        }

        [Test, TestCase("execution01")]
        public void Test(string testSrc)
        {
            DoOneTest(testSrc);
        }
    }
}
