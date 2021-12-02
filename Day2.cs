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
    internal class Day2
    {
        const string _inputFilename = @"inputs\Day2.txt";
        const string _testData = @"forward 5
down 5
forward 8
up 3
down 8
forward 2";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(150, RunSilver(FileHelpers.ReadAllLinesFromString(_testData)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(2117664, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(900, RunGold(FileHelpers.ReadAllLinesFromString(_testData)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(2073416724, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunGold(IEnumerable<string> input)
        {
            long aim = 0;
            long position = 0;
            long depth = 0;

            foreach (var line in input)
            {
                var tokens = line.Split(' ');
                var amount = Convert.ToInt64(tokens[1]);

                switch (tokens[0])
                {
                    case "forward":
                        position += amount;
                        depth += aim * amount;
                        break;
                    case "down":
                        aim += amount;
                        break;
                    case "up":
                        aim -= amount;
                        break;

                }
            }

            return position * depth;
        }

        static long RunSilver(IEnumerable<string> input)
        {
            long position = 0;
            long depth = 0;

            foreach (var line in input)
            {
                var tokens = line.Split(' ');
                var amount = Convert.ToInt64(tokens[1]);

                switch (tokens[0])
                {
                    case "forward":
                        position += amount;
                        break;
                    case "down":
                        depth += amount;
                        break;
                    case "up":
                        depth -= amount;
                        break;

                }
            }

            return position * depth;
        }
    }
}
