using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day4
    {
        const string _inputFilename = @"Inputs\Day4.txt";

        const string _testInput = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";

        const string _testNumbers = @"";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(4512, RunSilver(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(11774, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(1924, RunGold(FileHelpers.ReadAllLinesFromString(_testInput)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(4495, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        int RunSilver(IEnumerable<string> input)
        {
            var e = input.GetEnumerator();

            e.MoveNext();
            var numbers = e.Current.Split(',').Select(int.Parse).ToArray();
            e.MoveNext();

            var cards = ReadCards(e);

            foreach (var number in numbers)
            {
                foreach (var card in cards)
                {
                    card.Mark(number);
                    if (card.IsBingo())
                    {
                        return card.Score() * number;
                    }
                }
            }

            return -1;
        }

        int RunGold(IEnumerable<string> input)
        {
            var e = input.GetEnumerator();

            e.MoveNext();
            var numbers = e.Current.Split(',').Select(int.Parse).ToArray();
            e.MoveNext();

            var cards = ReadCards(e);


            foreach (var number in numbers)
            {
                var nextCards = new List<BingoCard>();

                foreach (var card in cards)
                {
                    card.Mark(number);
                    if (cards.Count == 1 && card.IsBingo())
                    {
                        return card.Score() * number;
                    }

                    if (!card.IsBingo())
                    {
                        nextCards.Add(card);
                    }
                }
                cards = nextCards;
            }

            return -1;
        }

        static List<BingoCard> ReadCards(IEnumerator<string> input)
        {
            var cards = new List<BingoCard>();

            while (input.MoveNext())
            {
                while (string.IsNullOrEmpty(input.Current))
                {
                    if (!input.MoveNext())
                    {
                        return cards;
                    }
                }
                var card = new BingoCard(input);
                cards.Add(card);
            }

            return cards;
        }
    }

    class BingoCard
    {
        readonly int[,] _space = new int[5, 5];
        readonly bool[,] _marks = new bool[5, 5];

        public BingoCard(IEnumerator<string> input)
        {
            for (int row = 0; row < 5; row++)
            {
                var line = input.Current;
                input.MoveNext();
                var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                for (int col = 0; col < 5; col++)
                {
                    _space[col, row] = tokens[col];
                }
            }
        }

        public void Mark(int number)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (_space[col, row] == number)
                    {
                        _marks[col, row] = true;
                    }
                }
            }
        }

        public bool IsBingo()
        {
            for (int row = 0; row < 5; row++)
            {
                int colCount = 0;
                for (int col = 0; col < 5; col++)
                {
                    colCount += _marks[col, row] ? 1 : 0;
                }

                if (colCount == 5) return true;
            }

            for (int col = 0; col < 5; col++)
            {
                int rowCount = 0;
                for (int row = 0; row < 5; row++)
                {
                    rowCount += _marks[col, row] ? 1 : 0;
                }

                if (rowCount == 5) return true;
            }

            return false;
        }

        public int Score()
        {
            int score = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (!_marks[col, row])
                    {
                        score += _space[col, row];
                    }
                }
            }

            return score;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (_marks[col, row])
                    {
                        sb.Append($"({_space[col, row]}) ");
                    }
                    else
                    {
                        sb.Append($"{_space[col, row]} ");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
