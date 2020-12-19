using AdventOfCode2020.Common;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day19
    {
        [Solution(19, 1)]
        public int Solution1(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var ruleInfo = Parser.ToArrayOf(sections[0], it => new RuleParser(it)).ToList();

            var generatedRules = new Dictionary<int, Rule>();
            while (ruleInfo.Any())
            {
                var toRemove = new List<RuleParser>();
                foreach(var item in ruleInfo)
                {
                    var outputRule = item.GetRule(generatedRules);
                    if (outputRule == null)
                        continue;

                    generatedRules.Add(item.Id, outputRule);
                    toRemove.Add(item);
                }

                foreach (var item in toRemove)
                    ruleInfo.Remove(item);
            }

            var topRule = generatedRules[0];
            var linesToCheck = Parser.ToArrayOfString(sections[1]);
            var sum = 0;
            foreach(var line in linesToCheck)
            {
                var matching = topRule.GetValidMatches(line);

                if (matching.Length == 1 && matching[0] == line)
                    sum++;
            }

            return sum;
        }

        [Solution(19, 2)]
        public int Solution2(string input)
        {
            var sections = Parser.ToArrayOfGroups(input);
            var ruleInfo = Parser.ToArrayOf(sections[0], it => new RuleParser(it)).ToList();

            var generatedRules = new Dictionary<int, Rule>();
            while (ruleInfo.Any())
            {
                var toRemove = new List<RuleParser>();
                foreach (var item in ruleInfo)
                {
                    var outputRule = item.GetRule2(generatedRules);
                    if (outputRule == null)
                        continue;

                    generatedRules.Add(item.Id, outputRule);
                    toRemove.Add(item);
                }

                foreach (var item in toRemove)
                    ruleInfo.Remove(item);
            }

            var linesToCheck = Parser.ToArrayOfString(sections[1]);
            var fourtyTwo = generatedRules[42];
            var thirtyOne = generatedRules[31];
            var sum = 0;

            foreach(var line in linesToCheck)
            {
                var ftMatches = fourtyTwo.GetValidMatches(input).Distinct().ToArray();
                var toMatches = thirtyOne.GetValidMatches(input).Distinct().ToArray();
                var optionPool = new List<string>();

                optionPool.AddRange(ApplyRuleEight(line, ftMatches));

                foreach(var match in optionPool)
                {
                    if(ApplyRuleEleven(match, ftMatches, toMatches))
                    {
                        sum++;
                        break;
                    }
                }
            }

            return sum;
        }

        private string[] ApplyRuleEight(string input, string[] ft)
        {
            var output = new List<string>();

            foreach(var item in ft)
            {
                if (input.StartsWith(item))
                {
                    var substring = input.Substring(item.Length);
                    output.AddRange(new[] { substring }.Concat(ApplyRuleEight(substring, ft)));
                }
            }

            return output.ToArray();
        }

        private bool ApplyRuleEleven(string input, string[] ft, string[] to)
        {
            var pairs = ft.SelectMany(it => to.Select(t => (start: it, end: t))).ToArray();

            foreach(var item in pairs)
            {
                if(input.Length == item.start.Length + item.end.Length && input.StartsWith(item.start) && input.EndsWith(item.end))
                    return true;

                if (input.Length > item.start.Length + item.end.Length && input.StartsWith(item.start) && input.EndsWith(item.end))
                {
                    var newInput = input.Substring(item.start.Length, input.Length - item.start.Length - item.end.Length);

                    if (ApplyRuleEleven(newInput, ft, to))
                        return true;
                }
            }

            return false;
        }

        private class Rule
        {
            private Rule[][] subRules;
            private string baseRule;

            public Rule(Rule[][] subRules)
            {
                this.subRules = subRules;
            }

            public Rule(string baseRule)
            {
                this.baseRule = baseRule;
            }

            public void OverwriteRules(Rule[][] newRules)
            {
                subRules = newRules;
            }

            public string[] GetValidMatches(string input)
            {
                if(subRules != null)
                {
                    var output = new List<string>();
                    foreach(var ruleSet in subRules)
                    {
                        var options = ruleSet.Select(it => it.GetValidMatches(input));
                        var permutations = new[] { string.Empty };
                        foreach(var item in options)
                        {
                            permutations = item.SelectMany(it => permutations.Select(p => p + it)).ToArray();
                        }

                        output.AddRange(permutations.Where(it => input.Contains(it)));
                    }

                    return output.ToArray();
                }
                else if (baseRule != null && input.Contains(baseRule))
                {
                    return new[] { baseRule };
                }

                return new string[0];
            }
        }

        private class RuleParser
        {
            public int Id { get; }
            private int[] neededSubRules;
            private string baseRule;
            private string originalText;

            public RuleParser(string input)
            {
                originalText = input;
                var sections = Parser.Split(input, ": ", " ", "|", "\"");
                Id = int.Parse(sections[0]);
                if (sections[1] == "a" || sections[1] == "b")
                    baseRule = sections[1];
                else
                    neededSubRules = sections.Skip(1).Select(it => int.Parse(it)).ToArray();
            }

            public Rule GetRule(Dictionary<int, Rule> existing)
            {
                if (baseRule != null)
                    return new Rule(baseRule);

                if (neededSubRules.Any(it => !existing.ContainsKey(it)))
                    return null;

                var ruleSections = Parser.Split(Parser.Split(originalText, ": ")[1], " | ");

                var rules = ruleSections.Select(it => Parser.Split(it, " ").Select(n => existing[int.Parse(n)]).ToArray()).ToArray();

                return new Rule(rules);
            }

            public Rule GetRule2(Dictionary<int, Rule> existing)
            {
                if (baseRule != null)
                    return new Rule(baseRule);

                if(originalText == "8: 42" && existing.ContainsKey(42))
                {
                    var output = new Rule(new Rule[0][]);
                    var newRules = new Rule[][] { new[] { existing[42] }, new[] { existing[42], output } };
                    output.OverwriteRules(newRules);

                    return output;
                }

                if(originalText == "11: 42 31" && existing.ContainsKey(42) && existing.ContainsKey(31))
                {
                    var output = new Rule(new Rule[0][]);
                    var newRules = new Rule[][] { new[] { existing[42], existing[31] }, new[] { existing[42], output, existing[31] } };
                    output.OverwriteRules(newRules);

                    return output;
                }

                if (neededSubRules.Any(it => !existing.ContainsKey(it)))
                    return null;

                var ruleSections = Parser.Split(Parser.Split(originalText, ": ")[1], " | ");

                var rules = ruleSections.Select(it => Parser.Split(it, " ").Select(n => existing[int.Parse(n)]).ToArray()).ToArray();

                return new Rule(rules);
            }
        }
    }
}
