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
    internal class Day6
    {
        const string _inputFilename = @"Inputs\Day6.txt";
        const string _testInput = @"3,4,3,1,2";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(26, Run(_testInput, 18));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(366057, Run(FileHelpers.GetSingle(_inputFilename), 80));
        }
        
        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(1653559299811, Run(FileHelpers.GetSingle(_inputFilename), 256));
        }

        static long Run(string input, int days)
        {
            const int maxDay = 9;
            var tokens = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

            var today = new long[maxDay];
            foreach (var token in tokens)
            {
                today[token]++;
            }

            for (int i = 0; i < days; i++)
            {
                var nextDay = new long[maxDay];
                for (int a = 1; a < maxDay; a++)
                {
                    nextDay[a - 1] = today[a];
                }

                nextDay[6] += today[0];
                nextDay[8] += today[0];

                today = nextDay;
            }

            return today.Sum(t => t);
        }
    }
}
