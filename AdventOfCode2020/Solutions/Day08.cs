using AdventOfCode2020.Common;
using System;
using System.Linq;

namespace AdventOfCode2020.Solutions
{
    class Day08
    {
        [Solution(8, 1)]
        public int Solution1(string input)
        {
            var code = Parser.ToArrayOf(input, it => new CodeLine(it));

            var acc = 0;
            var line = 0;

            while (true)
            {
                if (code[line].HasRun)
                    return acc;

                code[line].HasRun = true;
                switch (code[line].Name)
                {
                    case "acc":
                        acc += code[line].Value;
                        line++;
                        break;
                    case "jmp":
                        line += code[line].Value;
                        break;
                    case "nop":
                        line++;
                        break;
                }
            }
        }

        [Solution(8, 2)]
        public int Solution2(string input)
        {
            var code = Parser.ToArrayOf(input, it => new CodeLine(it));
            var linesToToggle = Enumerable.Range(0, code.Length).Zip(code, (a, b) => new { a, b }).Where(it => it.b.Name != "acc").Select(it => it.a).ToArray();

            foreach(var item in linesToToggle)
            {
                code[item].Toggle();
                var result = RunUntilEnd(code);
                if (result != null)
                    return result.Value;

                code[item].Toggle();
                foreach (var line in code)
                    line.HasRun = false;
            }

            throw new Exception("No solution found");
        }

        private int? RunUntilEnd(CodeLine[] code)
        {
            var acc = 0;
            var line = 0;

            while (line < code.Length)
            {
                if (code[line].HasRun)
                    return null;

                code[line].HasRun = true;
                switch (code[line].Name)
                {
                    case "acc":
                        acc += code[line].Value;
                        line++;
                        break;
                    case "jmp":
                        line += code[line].Value;
                        break;
                    case "nop":
                        line++;
                        break;
                }
            }

            return acc;
        }

        private class CodeLine
        {
            public string Name { get; private set; }

            public int Value { get; }

            public bool HasRun { get; set; }

            public CodeLine(string input)
            {
                var items = input.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                Name = items[0];
                Value = int.Parse(items[1]);
            }

            private CodeLine(string name, int value)
            {
                this.Name = name;
                this.Value = value;
            }

            public void Toggle()
            {
                if (Name == "jmp")
                    Name = "nop";
                else if (Name == "nop")
                    Name = "jmp";
            }
        }
    }
}
