﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace AdventOfCode2020.Common
{
    class Manager
    {
        private static readonly List<string> startupMessages = new List<string>
        {
            "Enter day followed by 1 or 2 for solution 1 or 2.",
            "Leave blank to use latest solution.",
            "Type 'q' to quit.",
            "Input for solution is copied from the clipboard."
        };

        public void Init()
        {
            var solutions = this.GetSolutions();

            foreach (var item in startupMessages)
            {
                Console.WriteLine(item);
            }

            var enteredOption = Console.ReadLine();

            while (enteredOption.ToLower() != "q")
            {
                if (string.IsNullOrWhiteSpace(enteredOption))
                {
                    var latestSolution = solutions.OrderByDescending(it => it.Day).ThenByDescending(it => it.Problem).First();
                    this.RunInstanceOfSolution(latestSolution);
                }
                else
                {
                    var inputs = enteredOption.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        var day = Convert.ToInt32(inputs[0]);
                        var solution = Convert.ToInt32(inputs[1]);

                        this.RunInstanceOfSolution(solutions.Single(it => it.Day == day && it.Problem == solution));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to find a solution from the entered options.");
                    }
                }

                enteredOption = Console.ReadLine();
            }
        }

        private List<SolutionMapping> GetSolutions()
        {
            var targets = Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(it => it.GetMethods()
                    .Where(m => m.GetCustomAttribute<SolutionAttribute>() != null)
                    .Select(m => new { Type = it, Method = m, Info = m.GetCustomAttribute<SolutionAttribute>() }));

            return targets.Select(it => new SolutionMapping
            {
                Day = it.Info.Day,
                Problem = it.Info.Problem,
                ClassType = it.Type,
                Method = it.Method,
            }).ToList();
        }

        private void RunInstanceOfSolution(SolutionMapping solutionMapping)
        {
            var input = this.ReadDataFromClipboard();
            var instance = Activator.CreateInstance(solutionMapping.ClassType);

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = solutionMapping.Method.Invoke(instance, new object[] { input });
                stopwatch.Stop();
                Console.WriteLine($"Solution for day {solutionMapping.Day}, problem {solutionMapping.Problem}:");
                Console.WriteLine(result);
                Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalSeconds} seconds");
                this.WriteDataToClipboard(result?.ToString() ?? string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Generate answer: " + e.Message);
            }
        }


        public string ReadDataFromClipboard()
        {
            var text = (string)null;

            var clipboardAction = new Action(() =>
            {
                if (!Clipboard.ContainsText())
                {
                    text = string.Empty;
                }

                text = Clipboard.GetText();
            });

            var thread = new Thread(new ThreadStart(clipboardAction));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join(TimeSpan.FromSeconds(5));

            return text;
        }

        public void WriteDataToClipboard(string data)
        {
            var clipboardAction = new Action(() =>
            {
                Clipboard.SetText(data);
                Console.WriteLine("Result copied to clipboard.");
            });

            var thread = new Thread(new ThreadStart(clipboardAction));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private class SolutionMapping
        {
            public int Day { get; set; }

            public int Problem { get; set; }

            public Type ClassType { get; set; }

            public MethodInfo Method { get; set; }
        }
    }
}
