using AdventOfCode2020.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day22
    {
        [Solution(22, 1)]
        public long Solution1(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var player1 = new Queue<int>(Parser.ToArrayOfString(sections[0]).Skip(1).Select(it => int.Parse(it)));
            var player2 = new Queue<int>(Parser.ToArrayOfString(sections[1]).Skip(1).Select(it => int.Parse(it)));

            while (player1.Any() && player2.Any())
            {
                var card1 = player1.Dequeue();
                var card2 = player2.Dequeue();

                if(card1 > card2)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);
                }
                else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }

            var endQueue = player1.Any() ? player1 : player2;

            var multiplier = endQueue.Count();
            var output = 0L;

            while (endQueue.Any())
            {
                output += (endQueue.Dequeue() * multiplier);
                multiplier--;
            }

            return output;
        }

        [Solution(22, 2)]
        public long Solution2(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var player1 = Parser.ToArrayOfString(sections[0]).Skip(1).Select(it => int.Parse(it)).ToArray();
            var player2 = Parser.ToArrayOfString(sections[1]).Skip(1).Select(it => int.Parse(it)).ToArray();

            var finalResult = PlayGame(player1, player2);
            var endQueue = finalResult.winningCards;

            var multiplier = endQueue.Count();
            var output = 0L;

            while (endQueue.Any())
            {
                output += (endQueue.Dequeue() * multiplier);
                multiplier--;
            }

            return output;
        }

        private (bool isPlayer1, Queue<int> winningCards) PlayGame(int[] player1Cards, int[] player2Cards)
        {
            var seenStates = new HashSet<GameState>();

            var player1 = new Queue<int>(player1Cards);
            var player2 = new Queue<int>(player2Cards);
            
            while(player1.Any() && player2.Any())
            {
                var currentState = new GameState(player1.ToArray(), player2.ToArray());
                if (seenStates.Contains(currentState))
                    return (true, player1);

                seenStates.Add(currentState);

                var card1 = player1.Dequeue();
                var card2 = player2.Dequeue();

                bool isPlayer1Winner;
                if (card1 <= player1.Count && card2 <= player2.Count)
                {
                    var subGameResult = PlayGame(player1.Take(card1).ToArray(), player2.Take(card2).ToArray());
                    isPlayer1Winner = subGameResult.isPlayer1;
                }
                else
                {
                    isPlayer1Winner = card1 > card2;
                }

                if (isPlayer1Winner)
                {
                    player1.Enqueue(card1);
                    player1.Enqueue(card2);
                }
                else
                {
                    player2.Enqueue(card2);
                    player2.Enqueue(card1);
                }
            }

            return player1.Any() ? (true, player1) : (false, player2);

        }

        private class GameState
        {
            public int[] Player1 { get; }
            public int[] Player2 { get; }

            public GameState(int[] player1, int[] player2)
            {
                Player1 = player1;
                Player2 = player2;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is GameState))
                    return false;

                var castState = obj as GameState;

                return AreArraysEqual(Player1, castState.Player1) && AreArraysEqual(Player2, castState.Player2);
            }

            public override int GetHashCode()
            {
                return GetArrayHashCode(Player1) ^ (GetArrayHashCode(Player2) << 16);
            }

            private bool AreArraysEqual(int[] first, int[] second)
            {
                return first.Length == second.Length && first.Zip(second, (a, b) => a - b).All(it => it == 0);
            }

            private int GetArrayHashCode(int[] array)
            {
                return array.Select((it, i) => it * i).Sum();
            }
        }
    }
}
