using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day11
    {
        const string _inputFilename = @"Inputs\Day11.txt";

        const string _testInput = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(1656, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(1705, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(195, RunGold(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(0, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunGold(IEnumerable<string> inputs)
        {
            var grid = new int[10, 10];

            int row = 0;
            foreach (var line in inputs)
            {
                int col = 0;
                foreach (var ch in line.Select(c => int.Parse("" + c)))
                {
                    grid[row, col] = ch;
                    col++;
                }

                row++;
            }

            for (int stepCount = 1;; stepCount++)
            {
                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    for (int x = 0; x < grid.GetLength(1); x++)
                    {
                        grid[y, x]++;
                    }
                }

                bool didFlash;
                long flashCount = 0;

                do
                {
                    didFlash = false;
                    for (int y = 0; y < grid.GetLength(0); y++)
                    {
                        for (int x = 0; x < grid.GetLength(1); x++)
                        {
                            if (grid[y, x] > 9)
                            {
                                flashCount++;
                                didFlash = true;
                                grid[y, x] = int.MinValue;

                                if (y > 0)
                                {
                                    if (x > 0) grid[y - 1, x - 1]++;
                                    grid[y - 1, x]++;
                                    if (x < grid.GetLength(1) - 1) grid[y - 1, x + 1]++;
                                }

                                if (x > 0) grid[y, x - 1]++;
                                if (x < grid.GetLength(1) - 1) grid[y, x + 1]++;

                                if (y < grid.GetLength(0) - 1)
                                {
                                    if (x > 0) grid[y + 1, x - 1]++;
                                    grid[y + 1, x]++;
                                    if (x < grid.GetLength(1) - 1) grid[y + 1, x + 1]++;
                                }
                            }
                        }
                    }
                } while (didFlash);

                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    for (int x = 0; x < grid.GetLength(1); x++)
                    {
                        if (grid[y, x] < 0) grid[y, x] = 0;
                    }
                }

                if (flashCount == grid.Length)
                {
                    return stepCount;
                }
            }
        }

        static long RunSilver(IEnumerable<string> inputs)
        {
            var grid = new int[10, 10];

            int row = 0;
            foreach (var line in inputs)
            {
                int col = 0;
                foreach (var ch in line.Select(c => int.Parse("" + c)))
                {
                    grid[row, col] = ch;
                    col++;
                }

                row++;
            }

            long flashCount = 0;

            for (int steps = 0; steps < 100; steps++)
            {
                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    for (int x = 0; x < grid.GetLength(1); x++)
                    {
                        grid[y, x]++;
                    }
                }

                bool didFlash;

                do
                {
                    didFlash = false;
                    for (int y = 0; y < grid.GetLength(0); y++)
                    {
                        for (int x = 0; x < grid.GetLength(1); x++)
                        {
                            if (grid[y, x] > 9)
                            {
                                flashCount++;
                                didFlash = true;
                                grid[y, x] = int.MinValue;

                                if (y > 0)
                                {
                                    if (x > 0) grid[y - 1, x - 1]++;
                                    grid[y - 1, x]++;
                                    if (x < grid.GetLength(1) - 1) grid[y - 1, x + 1]++;
                                }

                                if (x > 0) grid[y, x - 1]++;
                                if (x < grid.GetLength(1) - 1) grid[y, x + 1]++;

                                if (y < grid.GetLength(0) - 1)
                                {
                                    if (x > 0) grid[y + 1, x - 1]++;
                                    grid[y + 1, x]++;
                                    if (x < grid.GetLength(1) - 1) grid[y + 1, x + 1]++;
                                }
                            }
                        }
                    }
                } while (didFlash);

                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    for (int x = 0; x < grid.GetLength(1); x++)
                    {
                        if (grid[y, x] < 0) grid[y, x] = 0;
                    }
                }
            }

            return flashCount;
        }
    }
}
