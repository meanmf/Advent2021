using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day17
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual((45, 112), RunSilver(20, 30, -10, -5));
        }

        [Test]
        public void Run()
        {
            Assert.AreEqual((5671, 4556), RunSilver(230, 283, -107, -57));
        }

        static (long peak, int hitCount) RunSilver(int minX, int maxX, int minY, int maxY)
        {
            int maxPeak = 0;
            int hitCount = 0;

            for (int dY = minY; dY < 300; dY++)
            {
                for (int dX = 0; dX <= maxX; dX++)
                {
                    var (hit, peak) = Fire(dX, dY, minX, maxX, minY, maxY);
                    if (peak > maxPeak) maxPeak = peak;
                    if (hit) hitCount++;
                }
            }

            return (maxPeak, hitCount);
        }

        static (bool hit, int peak) Fire(int dX, int dY, int minX, int maxX, int minY, int maxY)
        {
            int x = 0;
            int y = 0;
            int peak = 0;

            while (y > minY)
            {
                x += dX;
                y += dY;

                if (y > peak) peak = y;

                if (dX > 0)
                {
                    dX--;
                }
                else if (dX < 0)
                {
                    dX++;
                }

                dY--;

                if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                {
                    return (true, peak);
                }
            }

            return (false, int.MinValue);
        }
    }
}
