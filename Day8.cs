using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day8
    {
        const string _inputFilename = @"Inputs\Day8.txt";

        const string _testInput = @"acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf";

        const string _testInput2 = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(0, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput)));
            Assert.AreEqual(26, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput2)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(303, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunSilver(IEnumerable<string> inputs)
        {
            long total = 0;

            foreach (var line in inputs)
            {
                var tokens = line.Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = tokens.Length - 4; i < tokens.Length; i++)
                {
                    if (tokens[i].Length == 2 ||
                        tokens[i].Length == 4 ||
                        tokens[i].Length == 3 ||
                        tokens[i].Length == 7)
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(5353, RunGold(FileHelpers.ReadAllLinesFromString(_testInput)));
            Assert.AreEqual(61229, RunGold(FileHelpers.ReadAllLinesFromString(_testInput2)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(961734, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunGold(IEnumerable<string> inputs)
        {
            long total = 0;

            foreach (var line in inputs)
            {
                var tokens = line.Split(new[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => string.Concat(s.OrderBy(ch => ch))).ToArray();

                var digits = new ArraySegment<string>(tokens, 0, 10);
                var outputs = new ArraySegment<string>(tokens, 10, 4);

                var key = MakeKey(digits);

                int val = 0;
                foreach (var output in outputs)
                {
                    val *= 10;
                    var index = Array.IndexOf(key, output);
                    Assert.GreaterOrEqual(index, 0);
                    val += index;
                }

                total += val;
            }

            return total;
        }

        static string[] MakeKey(ArraySegment<string> digits)
        {
            var key = new string[10];

            key[1] = digits.Single(t => t.Length == 2);
            key[7] = digits.Single(t => t.Length == 3);
            key[4] = digits.Single(t => t.Length == 4);
            key[8] = digits.Single(t => t.Length == 7);

            var bd = string.Concat(key[4].Except(key[1]));

            char b = 'x';
            char e = 'x';

            foreach (var digit in digits.Where(t => t.Length == 6))
            {
                if (!digit.Contains(bd[0]))
                {
                    b = bd[1];
                    key[0] = digit;
                }
                else if (!digit.Contains(bd[1]))
                {
                    b = bd[0];
                    key[0] = digit;
                }
                else if (!digit.Contains(key[1][0]))
                {
                    key[6] = digit;
                }
                else if (!digit.Contains(key[1][1]))
                {
                    key[6] = digit;
                }
                else
                {
                    e = key[8].Except(digit).Single();
                    key[9] = digit;
                }
            }

            Assert.AreNotEqual('x', b);
            Assert.AreNotEqual('x', e);

            foreach (var digit in digits.Where(t => t.Length == 5))
            {
                if (digit.Contains(e))
                {
                    key[2] = digit;
                }
                else if (digit.Contains(b))
                {
                    key[5] = digit;
                }
                else
                {
                    key[3] = digit;
                }
            }

            foreach (var s in key)
            {
                Assert.NotNull(s);
            }

            return key;
        }
    }
}
