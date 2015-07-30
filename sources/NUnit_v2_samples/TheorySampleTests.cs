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
}
