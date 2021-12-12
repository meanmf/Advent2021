using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day12
    {
        const string _inputFilename = @"Inputs\Day12.txt";

        #region TestData
        const string _testData = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

        const string _testData2 = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

        const string _testData3 = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";
        #endregion

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(10, Run(FileHelpers.ReadAllLinesFromString(_testData), false));
            Assert.AreEqual(19, Run(FileHelpers.ReadAllLinesFromString(_testData2), false));
            Assert.AreEqual(226, Run(FileHelpers.ReadAllLinesFromString(_testData3), false));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(4707, Run(FileHelpers.EnumerateLines(_inputFilename), false));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(36, Run(FileHelpers.ReadAllLinesFromString(_testData), true));
            Assert.AreEqual(103, Run(FileHelpers.ReadAllLinesFromString(_testData2), true));
            Assert.AreEqual(3509, Run(FileHelpers.ReadAllLinesFromString(_testData3), true));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(130493, Run(FileHelpers.EnumerateLines(_inputFilename), true));
        }

        static long Run(IEnumerable<string> inputs, bool canVisitTwice)
        {
            var map = BuildMap(inputs);
            var visited = new HashSet<string> { "start" };

            return Navigate(map["start"], map, visited, canVisitTwice);
        }

        static Dictionary<string, Node> BuildMap(IEnumerable<string> inputs)
        {
            var map = new Dictionary<string, Node>();

            foreach (var line in inputs)
            {
                var tokens = line.Split('-');

                if (!map.TryGetValue(tokens[0], out var node))
                {
                    node = new Node(tokens[0]);
                    map.Add(tokens[0], node);
                }

                node.Next.Add(tokens[1]);

                if (!map.TryGetValue(tokens[1], out node))
                {
                    node = new Node(tokens[1]);
                    map.Add(tokens[1], node);
                }

                node.Next.Add(tokens[0]);
            }

            return map;
        }

        static long Navigate(Node current, IReadOnlyDictionary<string, Node> map, HashSet<string> visited, bool canVisitTwice)
        {
            long total = 0;
            foreach (var next in current.Next)
            {
                if (next == "end")
                {
                    total++;
                    continue;
                }

                if (!visited.Contains(next))
                {
                    if (char.IsLower(next[0]))
                    {
                        visited.Add(next);
                    }

                    total += Navigate(map[next], map, visited, canVisitTwice);

                    if (char.IsLower(next[0]))
                    {
                        visited.Remove(next);
                    }
                }
                else if (canVisitTwice && next != "start")
                {
                    total += Navigate(map[next], map, visited, false);
                }
            }

            return total;
        }

        class Node
        {
            string Name { get; }
            public List<string> Next { get; } = new();

            public Node(string name)
            {
                Name = name;
            }
        }
    }
}
