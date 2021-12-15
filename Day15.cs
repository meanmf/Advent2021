using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day15
    {
        const string _inputFilename = @"Inputs\Day15.txt";

        const string _testInput = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(40, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput), 10));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(696, RunSilver(FileHelpers.EnumerateLines(_inputFilename), 100));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(315, RunGold(FileHelpers.ReadAllLinesFromString(_testInput), 10));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(2952, RunGold(FileHelpers.EnumerateLines(_inputFilename), 100));
        }

        static long RunSilver(IEnumerable<string> inputs, int size)
        {
            var grid = new byte[size, size];

            int row = 0;

            foreach (var line in inputs)
            {
                for (int col = 0; col < size; col++)
                {
                    grid[row, col] = byte.Parse(line.Substring(col, 1));
                }

                row++;
            }

            return Run(grid, size);
        }

        static long RunGold(IEnumerable<string> inputs, int size)
        {
            var grid = new byte[size * 5, size * 5];

            int row = 0;
            foreach (var line in inputs)
            {
                for (int col = 0; col < size; col++)
                {
                    var val = byte.Parse(line.Substring(col, 1));
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            grid[row + (y * size), col + (x * size)] = (byte)((val - 1 + x + y) % 9 + 1);
                        }
                    }
                }

                row++;
            }

            return Run(grid, size * 5);
        }

        static long Run(byte[,] grid, int size)
        {
            var max = size - 1;
            var scores = new int[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    scores[y, x] = 1000000;
                }
            }

            for (int pass = 0; pass < 2; pass++)
            {
                for (int a = 0; a < size; a++)
                {
                    for (int y = max; y >= max - a; y--)
                    {
                        for (int x = max; x >= max - a; x--)
                        {
                            scores[y, x] = FindMin(y, x, grid, scores);
                        }
                    }
                }
            }

            return scores[0, 0] - grid[0, 0];
        }

        static int FindMin(int y, int x, byte[,] grid, int[,] scores)
        {
            int minValue = int.MaxValue;
            var max = grid.GetLength(0) - 1;

            if (x == max && y == max)
            {
                return grid[y, x];
            }

            if (x < max)
            {
                minValue = grid[y, x] + scores[y, x + 1];
            }

            if (y < max)
            {
                minValue = Math.Min(minValue, grid[y, x] + scores[y + 1, x]);
            }

            if (x > 0)
            {
                minValue = Math.Min(minValue, grid[y, x] + scores[y, x - 1]);
            }

            if (y > 0)
            {
                minValue = Math.Min(minValue, grid[y, x] + scores[y - 1, x]);
            }

            return minValue;
        }
    }
}
