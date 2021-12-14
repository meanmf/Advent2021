using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day14
    {
        const string _inputFilename = @"Inputs\Day14.txt";

        const string _testInput = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(1588, Run(FileHelpers.ReadAllLinesFromString(_testInput), 10));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(3048, Run(FileHelpers.EnumerateLines(_inputFilename), 10));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(2188189693529, Run(FileHelpers.ReadAllLinesFromString(_testInput), 40));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(3288891573057, Run(FileHelpers.EnumerateLines(_inputFilename), 40));
        }

        static long Run(IEnumerable<string> inputs, int steps)
        {
            using var e = inputs.GetEnumerator();
            
            e.MoveNext();
            var polymer = e.Current;
            e.MoveNext();

            var key = new Dictionary<string, string>();
            while (e.MoveNext())
            {
                var tokens = e.Current.Split(new[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);

                key.Add(tokens[0], tokens[1]);
            }

            var counts = key.Keys.ToDictionary(k => k, _ => 0L);

            for (int i = 0; i < polymer.Length - 1; i++)
            {
                counts[polymer.Substring(i, 2)]++;
            }

            for (int step = 0; step < steps; step++)
            {
                var newCounts = counts.ToDictionary(k => k.Key, _ => 0L);
                foreach (var item in counts.Where(c => c.Value > 0))
                {
                    var ch = key[item.Key];
                    newCounts[item.Key[0] + ch] += item.Value;
                    newCounts[ch + item.Key[1]] += item.Value;
                }

                counts = newCounts;
            }

            var letters = new Dictionary<char, long>();
            foreach (var item in counts)
            {
                if (!letters.ContainsKey(item.Key[0])) letters[item.Key[0]] = 0;
                if (!letters.ContainsKey(item.Key[1])) letters[item.Key[1]] = 0;
                letters[item.Key[0]] += item.Value;
                letters[item.Key[1]] += item.Value;
            }

            letters[polymer[0]]++;
            letters[polymer[^1]]++;

            var sorted = letters.Values.Where(v => v > 0).OrderBy(v => v).ToArray();
            return (sorted[^1] >> 1) - (sorted[0] >> 1);
        }
    }
}
