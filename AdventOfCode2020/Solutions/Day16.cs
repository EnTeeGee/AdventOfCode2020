using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Solutions
{
    class Day16
    {
        [Solution(16, 1)]
        public int Solution1(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var rules = Parser.ToArrayOfString(sections[0]).Select(it => Parser.Split(it, ": ")[1]).Select(it => new Rule(it)).ToArray();

            var tickets = Parser.ToArrayOfString(sections[2]).Skip(1).ToArray();

            var output = 0;

            foreach(var item in tickets)
            {
                output += Parser.Split(item, ",").Select(it => int.Parse(it)).Where(it => !rules.Any(r => r.MatchesRule(it))).Sum();
            }

            return output;
        }

        [Solution(16, 2)]
        public long Solution2(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var rules = Parser.ToArrayOfString(sections[0]).Select(it => Parser.Split(it, ": ")).Select((it, i) => new Rule(it[1], it[0])).ToArray();
            var tickets = Parser.ToArrayOfString(sections[2]).Skip(1).Select(it => Parser.Split(it, ",").Select(val => int.Parse(val)).ToArray()).ToArray();
            var ownTicket = Parser.Split(Parser.ToArrayOfString(sections[1]).Skip(1).First(), ",").Select(it => int.Parse(it)).ToArray();

            var validRules = Enumerable.Range(0, ownTicket.Length).Select(it => new List<Rule>(rules)).ToArray();

            foreach(var ticket in tickets)
            {
                var validForEachColumn = ticket.Select(it => rules.Where(r => r.MatchesRule(it)).ToArray()).ToArray();

                if (validForEachColumn.Any(it => !it.Any()))
                    continue;

                validRules = validRules.Zip(validForEachColumn, (a, b) => a.Intersect(b).ToList()).ToArray();
            }

            var finalRules = new Rule[ownTicket.Length];

            while (finalRules.Any(it => it == null))
            {
                for (var i = 0; i < validRules.Length; i++)
                {
                    if(validRules[i].Count == 1)
                    {
                        finalRules[i] = validRules[i].First();
                        foreach (var ruleList in validRules)
                            ruleList.Remove(finalRules[i]);
                    }
                }
            }

            var fieldIds = finalRules.Select((it, i) => (ruleName: it.Name, index: i)).Where(it => it.ruleName.StartsWith("departure")).ToArray();

            var product = 1L;
            foreach (var item in fieldIds)
                product *= ownTicket[item.index];

            return product;
        }

        private class Rule
        {
            public string Name { get; }
            private int range1Start;
            private int range1End;
            private int range2Start;
            private int range2End;

            public Rule(string input)
            {
                var items = Parser.Split(input, "-", " or ").Select(it => int.Parse(it)).ToArray();

                range1Start = items[0];
                range1End = items[1];
                range2Start = items[2];
                range2End = items[3];
            }

            public Rule(string input, string name)
            {
                var items = Parser.Split(input, "-", " or ").Select(it => int.Parse(it)).ToArray();

                Name = name;
                range1Start = items[0];
                range1End = items[1];
                range2Start = items[2];
                range2End = items[3];
            }

            public bool MatchesRule(int input)
            {
                return (input >= range1Start && input <= range1End) || (input >= range2Start && input <= range2End);
            }
        }
    }
}
