using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day21
    {
        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(739_785, RunSilver(4, 8));
        }

        [Test]
        public void Silver()
        {
            Assert.AreEqual(742_257, RunSilver(10, 3));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(444_356_092_776_315, RunGold(4, 8));
        }

        [Test]
        public void Gold()
        {
            Assert.AreEqual(93_726_416_205_179, RunGold(10, 3));
        }

        static long RunSilver(int player1Start, int player2Start)
        {
            var score = new int[2];
            var position = new int[2];

            position[0] = player1Start - 1;
            position[1] = player2Start - 1;

            int die = 1;
            int currentPlayer = 0;
            int rollCount = 0;

            for (;;)
            {
                for (int roll = 0; roll < 3; roll++)
                {
                    position[currentPlayer] += die;
                    die++;
                    rollCount++;
                    if (die > 100) die = 1;
                }

                position[currentPlayer] %= 10;
                score[currentPlayer] += position[currentPlayer] + 1;
                if (score[currentPlayer] >= 1000)
                {
                    break;
                }

                currentPlayer = (currentPlayer + 1) % 2;
            }

            return rollCount * score.Min();
        }

        static long RunGold(int player1Start, int player2Start)
        {
            var odds = new Dictionary<int, List<(int nextPosition, int count)>>();

            for (int startPosition = 1; startPosition <= 10; startPosition++)
            {
                var nextPositions = new List<(int nextPosition, int count)>
                {
                    ((startPosition + 2) % 10 + 1, 1),
                    ((startPosition + 3) % 10 + 1, 3),
                    ((startPosition + 4) % 10 + 1, 6),
                    ((startPosition + 5) % 10 + 1, 7),
                    ((startPosition + 6) % 10 + 1, 6),
                    ((startPosition + 7) % 10 + 1, 3),
                    ((startPosition + 8) % 10 + 1, 1)
                };

                odds.Add(startPosition, nextPositions);
            }

            var player1 = CalcTurns(player1Start);
            var player2 = CalcTurns(player2Start);

            long player1Wins = 0;
            long player2Wins = 0;
            for (int turn = 1; turn < player1.wins.Length; turn++)
            {
                player1Wins += player1.wins[turn] * player2.gamesRemaining[turn - 1];
                player2Wins += player2.wins[turn] * player1.gamesRemaining[turn];
            }

            return Math.Max(player1Wins, player2Wins);

            (long[] wins, long[] gamesRemaining) CalcTurns(int start)
            {
                const int boardSize = 10;
                const int maxScore = 21;
                const int maxTurns = 10;

                var scoreGrid = new long[boardSize + 1, maxScore];
                var winTurns = new long[maxTurns];
                var gameTurns = new long[maxTurns];
                scoreGrid[start, 0] = 1;

                for (int turns = 0; turns < maxTurns; turns++)
                {
                    var scoreGridNext = new long[boardSize + 1, maxScore];

                    for (int position = 1; position <= boardSize; position++)
                    {
                        for (int score = 0; score < maxScore; score++)
                        {
                            if (scoreGrid[position, score] > 0)
                            {
                                foreach (var next in odds[position])
                                {
                                    var newScore = score + next.nextPosition;
                                    if (newScore >= maxScore)
                                    {
                                        winTurns[turns] += scoreGrid[position, score] * next.count;
                                    }
                                    else
                                    {
                                        scoreGridNext[next.nextPosition, score + next.nextPosition] +=
                                            scoreGrid[position, score] * next.count;
                                    }
                                }
                            }
                        }
                    }

                    long total = 0;
                    for (int a = 0; a < scoreGridNext.GetLength(0); a++)
                    {
                        for (int b = 0; b < scoreGridNext.GetLength(1); b++)
                        {
                            total += scoreGridNext[a, b];
                        }
                    }

                    gameTurns[turns] = total;

                    scoreGrid = scoreGridNext;
                }

                return (winTurns, gameTurns);
            }
        }
    }
}
