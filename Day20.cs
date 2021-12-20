using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day20
    {
        const string _inputFilename = @"Inputs\Day20.txt";

        const string _testInput = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(35, Run(FileHelpers.ReadAllLinesFromString(_testInput), 2));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(5306, Run(FileHelpers.EnumerateLines(_inputFilename), 2));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(3351, Run(FileHelpers.ReadAllLinesFromString(_testInput), 50));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(17497, Run(FileHelpers.EnumerateLines(_inputFilename), 50));
        }

        static long Run(IEnumerable<string> inputs, int iterations)
        {
            var lines = inputs.ToArray();

            var algo = lines[0];

            var grid = new HashSet<(int x, int y)>();

            int row = 0;

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            for (int i = 2; i < lines.Length; i++)
            {
                for (int col = 0; col < lines[i].Length; col++)
                {
                    if (lines[i][col] == '#')
                    {
                        grid.Add((row, col));
                        if (row < minY) minY = row;
                        if (row > maxY) maxY = row;
                        if (col < minX) minX = col;
                        if (col > maxX) maxX = col;
                    }
                }

                row++;
            }

            for (int i = 0; i < iterations; i++)
            {
                var nextGrid = new HashSet<(int y, int x)>();

                for (int y = minY - 1; y <= maxY + 1; y++)
                {
                    for (int x = minX - 1; x <= maxX + 1; x++)
                    {
                        var val = GetPixelValue(y, x, i);
                        if (algo[val] == '#')
                        {
                            nextGrid.Add((y, x));
                        }
                    }
                }

                grid = nextGrid;
                minY--;
                maxY++;
                minX--;
                maxX++;
            }

            return grid.Count;

            int GetPixelValue(int rr, int cc, int i)
            {
                int val = 0;

                for (int y = rr - 1; y <= rr + 1; y++)
                {
                    for (int x = cc - 1; x <= cc + 1; x++)
                    {
                        val <<= 1;

                        if (y < minY || x < minX || y > maxY || x > maxX)
                        {
                            if (algo[0] == '#' && (i % 2) == 1)
                            {
                                val++;
                            }
                        }
                        else
                        {
                            if (grid.Contains((y, x))) val++;
                        }
                    }
                }

                return val;
            }
        }
    }
}
