using Advent2020;
using BitStreams;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day16
    {
        const string _inputFilename = @"Inputs\Day16.txt";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(16, RunSilver("8A004A801A8002F478"));
            Assert.AreEqual(12, RunSilver("620080001611562C8802118E34"));
            Assert.AreEqual(23, RunSilver("C0015000016115A2E0802F182340"));
            Assert.AreEqual(31, RunSilver("A0016C880162017C3686B18A3D4780"));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(984, RunSilver(FileHelpers.GetSingle(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(3, RunGold("C200B40A82"));
            Assert.AreEqual(54, RunGold("04005AC33890"));
            Assert.AreEqual(7, RunGold("880086C3E88112"));
            Assert.AreEqual(9, RunGold("CE00C43D881120"));
            Assert.AreEqual(1, RunGold("D8005AC2A8F0"));
            Assert.AreEqual(0, RunGold("F600BC2D8F"));
            Assert.AreEqual(0, RunGold("9C005AC2F8F0"));
            Assert.AreEqual(1, RunGold("9C0141080250320F1802104A08"));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(1015320896946, RunGold(FileHelpers.GetSingle(_inputFilename)));
        }

        static long RunSilver(string input)
        {
            var (ver, _) = Run(input);
            return ver;
        }

        static long RunGold(string input)
        {
            var (_, value) = Run(input);
            return value;
        }

        static (long, long) Run(string input)
        {
            var bytes = new byte[input.Length >> 1];
            for (int i = 0; i < input.Length; i += 2)
            {
                bytes[i >> 1] = Convert.ToByte(input.Substring(i, 2), 16);
            }

            var bits = new BitStream(bytes, true);

            var (ver, value, _) = ReadPacket(bits);

            return (ver, value);
        }

        static (long, long, int) ReadPacket(BitStream bits)
        {
            int bitsConsumed = 0;
            long value = 0;
            long version = bits.ReadLong(3);
            var packetType = bits.ReadLong(3);
            bitsConsumed += 6;

            if (packetType == 4)
            {
                (value, var size) = bits.ReadLiteral();
                bitsConsumed += size;
            }
            else
            {
                var lengthType = bits.ReadBit().AsBool();
                bitsConsumed++;

                var subValues = new List<long>();

                if (lengthType)
                {
                    var packetCount = bits.ReadLong(11);
                    bitsConsumed += 11;

                    for (int i = 0; i < packetCount; i++)
                    {
                        var (ver, val, size) = ReadPacket(bits);
                        version += ver;
                        bitsConsumed += size;
                        subValues.Add(val);
                    }
                }
                else
                {
                    var bitsLeft = bits.ReadLong(15);
                    bitsConsumed += 15;
                    while (bitsLeft > 0)
                    {
                        var (ver, val, size) = ReadPacket(bits);
                        version += ver;
                        bitsConsumed += size;
                        bitsLeft -= size;
                        subValues.Add(val);
                    }
                }

                switch (packetType)
                {
                    case 0: // sum
                        value = subValues.Sum();
                        break;
                    case 1: // product
                        value = 1;
                        foreach (var v in subValues)
                        {
                            value *= v;
                        }

                        break;
                    case 2: // minimum
                        value = subValues.Min();
                        break;
                    case 3: // maximum
                        value = subValues.Max();
                        break;
                    case 5: // greater than
                        value = subValues[0] > subValues[1] ? 1 : 0;
                        break;
                    case 6: // less than
                        value = subValues[0] < subValues[1] ? 1 : 0;
                        break;
                    case 7: // equal to
                        value = subValues[0] == subValues[1] ? 1 : 0;
                        break;
                }
            }

            return (version, value, bitsConsumed);
        }
    }

    static class BitStreamExtensions
    {
        public static long ReadLong(this BitStream bits, int length)
        {
            long result = 0;

            for (int i = 0; i < length; i++)
            {
                result <<= 1;
                var bit = bits.ReadBit();
                result += bit.AsInt();
            }

            return result;
        }

        public static (long, int) ReadLiteral(this BitStream bits)
        {
            long result = 0;
            int bitsConsumed = 0;

            for (;;)
            {
                var continued = bits.ReadBit().AsBool();
                bitsConsumed++;

                for (int i = 0; i < 4; i++)
                {
                    result <<= 1;
                    result += bits.ReadBit().AsInt();
                    bitsConsumed++;
                }

                if (!continued) break;
            }

            return (result, bitsConsumed);
        }
    }
}
