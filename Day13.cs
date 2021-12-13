using System.Text;
using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day13
    {
        const string _inputFilename = @"Inputs\Day13.txt";

        const string _testInput = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

        [Test]
        public void RunTest()
        {
            var (count, output) = Run(FileHelpers.ReadAllLinesFromString(_testInput));
            Assert.AreEqual(17, count);
            Assert.AreEqual(@"#####
#   #
#   #
#   #
#####
     
     
", output);
        }

        [Test]
        public void Solve()
        {
            FileHelpers.CheckInputs(_inputFilename);

            var (count, output) = Run(FileHelpers.EnumerateLines(_inputFilename));
            Assert.AreEqual(631, count);
            Assert.AreEqual(@"#### #### #    ####   ##  ##  ###  #### 
#    #    #    #       # #  # #  # #    
###  ###  #    ###     # #    #  # ###  
#    #    #    #       # # ## ###  #    
#    #    #    #    #  # #  # # #  #    
#### #    #### #     ##   ### #  # #    
", output);
        }

        static (long,string) Run(IEnumerable<string> inputs)
        {
            var map = new bool[1400, 1400];
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            var folds = new List<(char Axis, int Location)>();

            foreach (var line in inputs)
            {
                if (line.StartsWith("fold along"))
                {
                    var tokens = line.Split(' ', '=');
                    char axis = tokens[2][0];
                    int location = int.Parse(tokens[3]);
                    folds.Add((axis, location));
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    var tokens = line.Split(",").Select(int.Parse).ToArray();

                    map[tokens[1], tokens[0]] = true;
                    if (tokens[1] > maxY) maxY = tokens[1];
                    if (tokens[0] > maxX) maxX = tokens[0];
                }
            }

            long count = 0;
            foreach (var fold in folds)
            {
                switch (fold.Axis)
                {
                    case 'y':
                        for (int y = maxY; y > fold.Location; y--)
                        {
                            for (int x = 0; x <= maxX; x++)
                            {
                                map[maxY - y, x] |= map[y, x];
                            }
                        }

                        maxY = fold.Location - 1;
                        break;
                    case 'x':
                        for (int y = 0; y <= maxY; y++)
                        {
                            for (int x = maxX; x > fold.Location; x--)
                            {
                                map[y, maxX - x] |= map[y, x];
                            }
                        }

                        maxX = fold.Location - 1;
                        break;

                }

                if (count == 0)
                {
                    for (int y = 0; y <= maxY; y++)
                    {
                        for (int x = 0; x <= maxX; x++)
                        {
                            if (map[y, x]) count++;
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    sb.Append(map[y, x] ? '#' : ' ');
                }

                sb.AppendLine();
            }

            return (count, sb.ToString());
        }
    }
}
