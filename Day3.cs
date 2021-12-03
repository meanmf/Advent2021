using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day3
    {
        const string _inputFilename = @"Inputs\Day3.txt";

        const string _testInput = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(198, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(3009600, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        long RunSilver(IEnumerable<string> inputs)
        {
            var inputArray = inputs.ToArray();
            string gamma = string.Empty;
            string epsilon = string.Empty;

            for (int position = 0; position < inputArray[0].Length; position++)
            {
                var count = inputArray.Sum(i => i[position] == '1' ? 1 : 0);
                if (count > inputArray.Length / 2)
                {
                    gamma += '1';
                    epsilon += '0';
                }
                else
                {
                    gamma += '0';
                    epsilon += '1';
                }
            }

            var g = Convert.ToInt64(gamma, 2);
            var e = Convert.ToInt64(epsilon, 2);

            return g * e;
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(230, RunGold(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(6940518, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunGold(IEnumerable<string> inputs)
        {
            var o2 = Find(inputs, true);
            var co2 = Find(inputs, false);

            return o2 * co2;
        }

        static long Find(IEnumerable<string> inputs, bool findMost)
        {
            IReadOnlyList<string> current = inputs.ToList();

            for (int position = 0; position < current[0].Length; position++)
            {
                if (current.Count == 1)
                {
                    return Convert.ToInt64(current[0], 2);
                }

                var count = current.Sum(i => i[position] == '1' ? 1 : 0);
                
                char findChar;
                if (count >= current.Count / 2f)
                {
                    findChar = findMost ? '1' : '0';
                }
                else
                {
                    findChar = findMost ? '0' : '1';
                }

                current = current.Where(row => row[position] == findChar).ToList();
            }

            Assert.AreEqual(1, current.Count);
            return Convert.ToInt64(current[0], 2);
        }
    }
}
