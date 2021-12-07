using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day7
    {
        const string _inputFilename = @"Inputs\Day7.txt";
        const string _testInput = "16,1,2,0,4,2,7,1,2,14";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(37, RunSilver(_testInput));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(355592, RunSilver(FileHelpers.GetSingle(_inputFilename)));
        }
        
        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(168, RunGold(_testInput));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(101618069, RunGold(FileHelpers.GetSingle(_inputFilename)));
        }
        
        static long RunSilver(string inputs)
        {
            return Eval(inputs, t => t);
        }

        static long RunGold(string inputs)
        {
            return Eval(inputs, t => t * (t + 1) / 2);
        }

        static long Eval(string inputs, Func<long, long> costFunc)
        {
            var tokens = inputs.Split(',').Select(int.Parse).ToArray();
            var max = tokens.Max();

            long min = long.MaxValue;

            for (int i = 0; i < max; i++)
            {
                long total = tokens.Sum(t => costFunc(Math.Abs(t - i)));
                if (total < min) min = total;
            }

            return min;
        }
    }
}
