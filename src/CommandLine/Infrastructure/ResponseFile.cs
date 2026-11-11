// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace CommandLine.Infrastructure
{
    static class ResponseFile
    {
        public static IEnumerable<string> Expand(IEnumerable<string> arguments, string baseDirectory)
        {
            var expanded = new List<string>();
            foreach (var argument in arguments)
            {
                if (argument.StartsWith("@") && argument.Length > 1)
                {
                    var path = Path.Combine(baseDirectory, argument.Substring(1));
                    expanded.AddRange(Read(path));
                }
                else
                {
                    expanded.Add(argument);
                }
            }
            return expanded;
        }

        public static IEnumerable<string> Read(string path)
        {
            var reader = new StreamReader(path);
            var lines = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Trim().Length == 0 || line.StartsWith("#")) continue;
                lines.Add(line.Trim());
            }
            return lines;
        }
    }
}
