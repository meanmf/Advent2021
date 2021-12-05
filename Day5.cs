using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day5
    {
        const string _inputFilename = @"inputs\Day5.txt";

        const string _testInput = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";

        [Test]
        public void TestSilver()
        {
            Assert.AreEqual(5, Run(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void RunSilver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(6311, Run(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void TestGold()
        {
            Assert.AreEqual(12, Run(FileHelpers.ReadAllLinesFromString(_testInput), true));
        }

        [Test]
        public void RunGold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(0, Run(FileHelpers.EnumerateLines(_inputFilename), true));
        }

        int Run(IEnumerable<string> inputs, bool diagonals = false)
        {
            int[,] grid = new int[1000, 1000];

            foreach (var line in inputs)
            {
                var tokens = line.Split(new[] { ',', ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray();

                var x1 = tokens[0];
                var y1 = tokens[1];
                var x2 = tokens[2];
                var y2 = tokens[3];

                var dx = (x2 > x1) ? 1 : x2 == x1 ? 0 : -1;
                var dy = (y2 > y1) ? 1 : y2 == y1 ? 0 : -1;

                if (diagonals || (x1 == x2 || y1 == y2))
                {
                    var x = x1;
                    var y = y1;

                    for (;;)
                    {
                        grid[x, y]++;

                        if (x == x2 && y == y2)
                        {
                            break;
                        }

                        x += dx;
                        y += dy;
                    }
                }
            }

            int points = 0;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y] > 1)
                    {
                        points++;
                    }
                }
            }

            return points;
        }
    }

}
