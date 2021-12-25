using System.Text;
using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day25
    {
        const string _inputFilename = @"Inputs\Day25.txt";
        const string _testInput = @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(58, Run(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(419, Run(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long Run(IEnumerable<string> inputs)
        {
            var lines = inputs.ToArray();
            int width = lines[0].Length;
            int height = lines.Length;

            var grid = new char[height, width];

            int row = 0;
            foreach (var line in lines)
            {
                int col = 0;
                foreach (var ch in line)
                {
                    grid[row, col++] = ch;
                }

                row++;
            }

            var lastGrid = "";
            for (int moves = 1;; moves++)
            {
                var newGrid = new char[height, width];
                for (row = 0; row < lines.Length; row++)
                {
                    for (int col = 0; col < lines[0].Length; col++)
                    {
                        if (newGrid[row, col] != (char)0) continue;
                        switch (grid[row, col])
                        {
                            case '.':
                            case 'v':
                                newGrid[row, col] = grid[row, col];
                                break;
                            case '>':
                                if (grid[row, (col + 1) % width] == '.')
                                {
                                    newGrid[row, (col + 1) % width] = '>';
                                    newGrid[row, col] = '.';
                                }
                                else
                                {
                                    newGrid[row, col] = '>';
                                }

                                break;
                        }
                    }
                }

                grid = newGrid;
                newGrid = new char[height, width];
                for (row = 0; row < lines.Length; row++)
                {
                    for (int col = 0; col < lines[0].Length; col++)
                    {
                        if (newGrid[row, col] != (char)0) continue;
                        switch (grid[row, col])
                        {
                            case '.':
                            case '>':
                                newGrid[row, col] = grid[row, col];
                                break;
                            case 'v':
                                if (grid[(row + 1) % height, col] == '.')
                                {
                                    newGrid[(row + 1) % height, col] = 'v';
                                    newGrid[row, col] = '.';
                                }
                                else
                                {
                                    newGrid[row, col] = 'v';
                                }

                                break;
                        }
                    }
                }

                grid = newGrid;

                var gridString = GridToString();
                if (gridString == lastGrid)
                {
                    return moves;
                }

                lastGrid = gridString;
            }

            string GridToString()
            {
                var sb = new StringBuilder();
                for (int r = 0; r < height; r++)
                {
                    for (int c = 0; c < width; c++)
                    {
                        sb.Append(grid[r, c]);
                    }
                }

                return sb.ToString();
            }
        }
    }
}
