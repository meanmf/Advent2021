using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day24
    {
        [Test]
        public void Silver()
        {
            Assert.AreEqual(51983999947999, RunSilver());
        }

        [Test]
        public void Gold()
        {
            Assert.AreEqual(11211791111365, RunGold());
        }

        static long RunSilver()
        {
            for (int a = 999; a >= 111; a--)
            for (int b = 9; b >= 1; b--)
            for (int c = 9; c >= 1; c--)
            for (int d = 99; d >= 1; d--)
            {
                var result = Test(a, b, c, d);
                if (result > 0)
                {
                    return result;
                }
            }

            return -1;
        }

        static long RunGold()
        {
            for (int a = 111; a <= 999; a++)
            for (int b = 1; b <= 9; b++)
            for (int c = 1; c <= 9; c++)
            for (int d = 11; d <= 99; d++)
            {
                var result = Test(a, b, c, d);
                if (result > 0)
                {
                    return result;
                }
            }

            return -1;
        }

        static long Test(int a, int b, int c, int d)
        {
            long w = a / 100;
            long z = w + 6;

            w = a % 100 / 10;
            z = z * 26 + w + 14;

            w = a % 10;
            z = z * 26 + w + 13;

            var check1 = z % 26 - 14;
            if (check1 == 0 || check1 > 9) return -1;
            z /= 26;
            
            w = b;
            z = z * 26 + w + 6;

            var check2 = z % 26;
            if (check2 == 0 || check2 > 9) return -1;
            z /= 26;

            var check3 = z % 26 - 6;
            if (check3 == 0 || check3 > 9) return -1; 
            z /= 26;
            
            w = c;
            z = z * 26 + w + 3;

            var check4 = z % 26 - 3;
            if (check4 == 0 || check4 > 9) return -1;
            z /= 26;

            w = d / 10;
            z = z * 26 + w + 14;

            w = d % 10;
            z = z * 26 + w + 4;

            var check5 = z % 26 - 2;
            if (check5 == 0 || check5 > 9) return -1;
            z /= 26;

            var check6 = z % 26 - 9;
            if (check6 == 0 || check6 > 9) return -1;
            z /= 26;

            var check7 = z % 26 - 2;
            if (check7 == 0 || check7 > 9) return -1;
            z /= 26;

            return long.Parse($"{a}{check1}{b}{check2}{check3}{c}{check4}{d}{check5}{check6}{check7}");
        }
    }
}
