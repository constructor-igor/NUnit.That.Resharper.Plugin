using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnit_v3_samples
{
    [TestFixture]
    public class NUnitIssue755
    {
        // all in ms
        const int TIME_OUT_TIME = 2000;
        const int NOT_TIMEOUTED_TIME = 1000;
        const int TIMEOUTED_TIME = 3000;

        // it works!
        [Test, Timeout(TIME_OUT_TIME), Parallelizable(ParallelScope.Fixtures)]
        public void TestTimeOutElapsed()
        {
            TestTimeOutCase(TIMEOUTED_TIME);
        }

        // it works!
        [Test, Timeout(TIME_OUT_TIME), Parallelizable(ParallelScope.Fixtures)]
        public void TestTimeOutNotElapsed()
        {
            TestTimeOutCase(NOT_TIMEOUTED_TIME);
        }

        // it doesn't work (both tests end successfully)
        static List<object[]> Delays = new List<object[]>() { new object[] { NOT_TIMEOUTED_TIME }, new object[] { TIMEOUTED_TIME } };

        [Test, Timeout(TIME_OUT_TIME), TestCaseSource("Delays"), Parallelizable(ParallelScope.Fixtures)]
        public void TestTimeOutCase(int delay)
        {
            Task.Delay(delay);
        }

    }
}