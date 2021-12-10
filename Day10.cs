using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day10
    {
        const string _inputFilename = @"Inputs\Day10.txt";

        const string _testInput = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(26397, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(311949, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(288957, RunGold(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(3042730309, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static readonly IReadOnlyDictionary<char,char> _matchTable = new Dictionary<char, char>
        {
            ['('] = ')',
            ['['] = ']',
            ['{'] = '}',
            ['<'] = '>',
        };

        static long RunSilver(IEnumerable<string> inputs)
        {
            var scoreTable = new Dictionary<char, int>
            {
                [')'] = 3,
                [']'] = 57,
                ['}'] = 1197,
                ['>'] = 25137,
            };

            long score = 0;

            foreach (var line in inputs)
            {
                var stack = new Stack<char>();
                foreach (var ch in line)
                {
                    if (_matchTable.ContainsKey(ch))
                    {
                        stack.Push(ch);
                    }
                    else if (ch != _matchTable[stack.Pop()])
                    {
                        score += scoreTable[ch];
                        break;
                    }
                }
            }

            return score;
        }

        static long RunGold(IEnumerable<string> inputs)
        {
            var scoreTable = new Dictionary<char, int>
            {
                ['('] = 1,
                ['['] = 2,
                ['{'] = 3,
                ['<'] = 4,
            };

            var scores = new List<long>();

            foreach (var line in inputs)
            {
                var stack = new Stack<char>();
                bool isValid = true;

                foreach (var ch in line)
                {
                    if (_matchTable.ContainsKey(ch))
                    {
                        stack.Push(ch);
                    }
                    else if (ch != _matchTable[stack.Pop()])
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    long score = 0;
                    while (stack.Count > 0)
                    {
                        score *= 5;
                        score += scoreTable[stack.Pop()];
                    }

                    scores.Add(score);
                }
            }

            return scores.OrderBy(score => score).Skip((int)Math.Floor(scores.Count / 2d)).First();
        }

    }
}
