using Advent2020;
using NUnit.Framework;

namespace Advent2021
{
    [TestFixture]
    internal class Day18
    {
        const string _inputFilename = @"Inputs\Day18.txt";

        const string _example1 = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]";

        const string _example2 = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";

        [Test]
        public void SilverTest()
        {
            Assert.AreEqual(3488, RunSilver(FileHelpers.ReadAllLinesFromString(_example1)));
            Assert.AreEqual(4140, RunSilver(FileHelpers.ReadAllLinesFromString(_example2)));
        }

        [Test]
        public void Silver()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(3734, RunSilver(FileHelpers.EnumerateLines(_inputFilename)));
        }

        [Test]
        public void GoldTest()
        {
            Assert.AreEqual(3993, RunGold(FileHelpers.ReadAllLinesFromString(_example2)));
        }

        [Test]
        public void Gold()
        {
            FileHelpers.CheckInputs(_inputFilename);

            Assert.AreEqual(4837, RunGold(FileHelpers.EnumerateLines(_inputFilename)));
        }

        static long RunGold(IEnumerable<string> inputs)
        {
            var inputArray = inputs.ToArray();
            long max = long.MinValue;

            for (int a = 0; a < inputArray.Length; a++)
            {
                for (int b = 0; b < inputArray.Length; b++)
                {
                    if (a == b) continue;

                    var fish = new Fish(inputArray[a]);
                    fish.Add(new Fish(inputArray[b]));

                    if (fish.Magnitude() > max)
                    {
                        max = fish.Magnitude();
                    }
                }
            }

            return max;
        }

        static long RunSilver(IEnumerable<string> inputs)
        {
            Fish? rootFish = null;

            foreach (var line in inputs)
            {
                var fish = new Fish(line);
                if (rootFish == null)
                {
                    rootFish = fish;
                }
                else
                {
                    rootFish.Add(fish);
                }
            }

            return rootFish.Magnitude();
        }

        class Fish
        {
            public Node Head { get; set; }
            public Node Tail { get; set; }

            Node Root { get; set; }

            public Fish(string input)
            {
                var nodes = new Stack<Node>();
                Node? head = null;
                Node? tail = null;

                foreach (var ch in input)
                {
                    switch (ch)
                    {
                        case '[':
                        case ',':
                            break;
                        case ']':
                            var right = nodes.Pop();
                            var left = nodes.Pop();

                            var newNode = new Node(left, right);

                            nodes.Push(newNode);

                            break;
                        default:
                            var valueNode = new Node(ch);
                            head ??= valueNode;
                            if (tail != null)
                            {
                                tail.Next = valueNode;
                                valueNode.Previous = tail;
                            }
                            tail = valueNode;
                            nodes.Push(valueNode);
                            break;
                    }
                }

                Root = nodes.Pop();
                Head = head!;
                Tail = tail!;
            }

            public void Add(Fish other)
            {
                var newRoot = new Node(Root, other.Root)
                {
                    Left = Root,
                    Right = other.Root
                };

                Root = newRoot;
                Tail.Next = other.Head;
                other.Head.Previous = Tail;
                Tail = other.Tail;

                Reduce();
            }

            void Reduce()
            {
                for (;;)
                {
                    var explodeNode = Root.FindAtDepth(4);
                    if (explodeNode != null)
                    {
                        explodeNode.Explode(this);
                        continue;
                    }

                    var splitNode = Head;
                    while (splitNode != null && splitNode.Value < 10)
                    {
                        Assert.IsTrue(splitNode.IsValue);
                        splitNode = splitNode.Next;
                    }

                    if (splitNode == null) break;

                    splitNode.Split(this);
                }
            }

            public long Magnitude()
            {
                return Root.Magnitude();
            }
        }

        class Node
        {
            public bool IsValue { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public int Value { get; set; }

            public Node? Next { get; set; }
            public Node? Previous { get; set; }

            public Node(char ch)
            {
                IsValue = true;
                Value = int.Parse("" + ch);
            }

            Node(int value)
            {
                IsValue = true;
                Value = value;
            }

            public Node(Node left, Node right)
            {
                IsValue = false;
                Left = left;
                Right = right;
            }

            public Node? FindAtDepth(int depth)
            {
                if (IsValue) return null;
                if (depth == 0) return this;

                var leftNode = Left?.FindAtDepth(depth - 1);
                if (leftNode != null) return leftNode;

                return Right?.FindAtDepth(depth - 1);
            }

            public void Explode(Fish parentFish)
            {
                Assert.IsTrue(Left!.IsValue);
                Assert.IsTrue(Right!.IsValue);

                if (Left.Previous != null)
                {
                    Left.Previous.Value += Left.Value;
                    Left.Previous.Next = this;
                }

                if (Right.Next != null)
                {
                    Right.Next.Value += Right.Value;
                    Right.Next.Previous = this;
                }

                if (parentFish.Head == Left)
                {
                    parentFish.Head = this;
                }

                if (parentFish.Tail == Right)
                {
                    parentFish.Tail = this;
                }

                Previous = Left.Previous;
                Next = Right.Next;
                IsValue = true;
                Value = 0;
                Left = Right = null;
            }

            public void Split(Fish parentFish)
            {
                Left = new Node((int)Math.Floor(Value / 2f));
                Left.Previous = Previous;
                if (Previous != null) Previous.Next = Left;

                Right = new Node((int)Math.Ceiling(Value / 2f));
                Right.Next = Next;
                if (Next != null) Next.Previous = Right;

                Right.Previous = Left;
                Left.Next = Right;

                Previous = Next = null;
                IsValue = false;

                if (parentFish.Head == this)
                {
                    parentFish.Head = Left;
                }

                if (parentFish.Tail == this)
                {
                    parentFish.Tail = Right;
                }
            }

            public long Magnitude()
            {
                if (IsValue) return Value;

                return Left!.Magnitude() * 3 + Right!.Magnitude() * 2;
            }
        }
    }
}
