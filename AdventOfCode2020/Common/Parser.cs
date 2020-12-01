﻿using System;
using System.Linq;

namespace AdventOfCode2020.Common
{
    static class Parser
    {
        public static T[] ToArrayOf<T>(string input, Func<string, T> converter)
        {
            return input
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(it => converter.Invoke(it.Trim()))
                .ToArray();
        }

        public static int[] ToArrayOfInt(string input)
        {
            return ToArrayOf(input, it => Convert.ToInt32(it));
        }

        public static string[] ToArrayOfString(string input)
        {
            return ToArrayOf(input, it => it);
        }
    }
}
