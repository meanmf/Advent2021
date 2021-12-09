using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day9
    {
        const string _inputFilename = @"Inputs\Day9.txt";

        const string _testInput = @"2199943210
3987894921
9856789892
8767896789
9899965678";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(15, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(562, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(1134, RunGold(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(1076922, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunSilver(IEnumerable<string> inputs)
        {
            var grid = inputs.Select(line => line.Select(c => byte.Parse("" + c)).ToArray()).ToArray();

            long total = 0;

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[0].Length; x++)
                {
                    if (y > 0 && grid[y][x] >= grid[y - 1][x]) continue;

                    if (x > 0 && grid[y][x] >= grid[y][x - 1]) continue;

                    if (y < grid.Length - 1 && grid[y][x] >= grid[y + 1][x]) continue;

                    if (x < grid[0].Length - 1 && grid[y][x] >= grid[y][x + 1]) continue;

                    total += grid[y][x] + 1;
                }
            }

            return total;
        }

        static long RunGold(IEnumerable<string> inputs)
        {
            var grid = inputs.Select(line => line.Select(c => byte.Parse("" + c)).ToArray()).ToArray();

            var visited = new bool[grid.Length, grid[0].Length];

            var sizes = new List<int>();

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[0].Length; x++)
                {
                    if (visited[y, x]) continue;
                    visited[y, x] = true;

                    if (grid[y][x] == 9) continue;

                    int size = 1;

                    var nextMoves = new List<(int y, int x)>();
                    if (nextMoves == null) throw new ArgumentNullException(nameof(nextMoves));
                    AddMove(x, y, grid, nextMoves);

                    for (;;)
                    {
                        var moves = nextMoves;
                        nextMoves = new List<(int, int)>();

                        if (moves.Count == 0) break;
                        foreach (var move in moves)
                        {
                            if (visited[move.y, move.x]) continue;
                            visited[move.y, move.x] = true;
                            size++;
                            AddMove(move.x, move.y, grid, nextMoves);
                        }
                    }

                    sizes.Add(size);
                }
            }

            long total = 1;
            foreach (var a in sizes.OrderByDescending(s => s).Take(3))
            {
                total *= a;
            }

            return total;
        }

        static void AddMove(int x, int y, byte[][] grid, List<(int, int)> moves)
        {
            if (y > 0 && grid[y - 1][x] != 9)
            {
                moves.Add((y - 1, x));
            }

            if (x > 0 && grid[y][x - 1] != 9)
            {
                moves.Add((y, x - 1));
            }

            if (y < grid.Length - 1 && grid[y + 1][x] != 9)
            {
                moves.Add((y + 1, x));
            }

            if (x < grid[0].Length - 1 && grid[y][x + 1] != 9)
            {
                moves.Add((y, x + 1));
            }
        }
    }
}
