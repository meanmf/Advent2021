using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    public class Day1
    {
        const string _inputFilename = @"Inputs\Day1.txt";
        const string _testData = @"199
200
208
210
200
207
240
269
260
263";

        [Test]
        public void SilverTest()
        {
            var inputs = FileHelpers.ReadAllLinesFromString(_testData).Select(i => Convert.ToInt64(i)).ToArray();

            Assert.AreEqual(7, Run(1, inputs));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            var inputs = FileHelpers.EnumerateLines(_inputFilename).Select(i => Convert.ToInt64(i)).ToArray();

            Assert.AreEqual(1655, Run(1, inputs));
        }

        [Test]
        public void GoldTest()
        {
            var inputs = FileHelpers.ReadAllLinesFromString(_testData).Select(i => Convert.ToInt64(i)).ToArray();

            Assert.AreEqual(5, Run(3, inputs));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            var inputs = FileHelpers.EnumerateLines(_inputFilename).Select(i => Convert.ToInt64(i)).ToArray();

            Assert.AreEqual(1683, Run(3, inputs));
        }

        static int Run(int windowSize, IList<long> inputs)
        {
            int total = 0;

            for (int index = windowSize; index < inputs.Count; index++)
            {
                if (inputs[index] > inputs[index - windowSize])
                {
                    total++;
                }
            }

            return total;
        }
    }
}
