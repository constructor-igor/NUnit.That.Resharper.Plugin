using System;
using NUnit.Framework;

namespace NUnit_v2_samples
{
    [TestFixture(typeof(int))]
    [TestFixture(typeof(double))]
    public class TheorySampleTestsGeneric<T>
    {
        [Datapoint]
        public double[] ArrayDouble1 = { 0, 1, 2, 3 };
        [Datapoint]
        public double[] ArrayDouble2 = { 4, 5, 6, 7 };
        [Datapoint]
        public int[] ArrayInt = { 0, 1, 2, 3 };
        [Theory]
        public void TestGenericForArbitraryArray(T[] array)
        {
            Console.WriteLine("TestGenericForArbitraryArray()");
        }
    }

    [TestFixture]
    [Explicit]
    public class ExpectedExceptionTestSamples
    {
        [Test]
        [ExpectedException]
        public void Sample1()
        {
            foo1();
        }

        private void foo1()
        {
            
        }

        [Test]
        [ExpectedException(typeof(ProductDllException), ExpectedMessage = "Export requires 2 persons, but found 1.")]
        public void Export_ExpectedException()
        {
            using (TemporaryDisposableClass temporaryFile = new TemporaryDisposableClass())
            {
                foo1();
            }
        }

        private class ProductDllException : Exception
        {            
        }
        private class TemporaryDisposableClass : IDisposable
        {
            #region Implementation of IDisposable
            public void Dispose()
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}
