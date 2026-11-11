// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Infrastructure
{
    static class NameSuggester
    {
        public static IEnumerable<string> Suggest(string unknownName, IEnumerable<string> knownNames, int maxDistance)
        {
            if (unknownName == null) throw new ArgumentNullException(nameof(unknownName));
            if (knownNames == null) throw new ArgumentNullException(nameof(knownNames));

            return knownNames
                .Where(name => !string.IsNullOrEmpty(name))
                .Select(name => new { Name = name, Distance = Distance(unknownName, name) })
                .Where(candidate => candidate.Distance <= maxDistance)
                .OrderBy(candidate => candidate.Distance)
                .ThenBy(candidate => candidate.Name, StringComparer.Ordinal)
                .Select(candidate => candidate.Name)
                .ToArray();
        }

        public static int Distance(string source, string target)
        {
            var previous = new int[target.Length + 1];
            var current = new int[target.Length + 1];

            for (var j = 0; j <= target.Length; j++)
            {
                previous[j] = j;
            }

            for (var i = 1; i <= source.Length; i++)
            {
                current[0] = i;
                for (var j = 1; j <= target.Length; j++)
                {
                    var cost = char.ToLowerInvariant(source[i - 1]) == char.ToLowerInvariant(target[j - 1]) ? 0 : 1;
                    current[j] = Math.Min(Math.Min(current[j - 1] + 1, previous[j] + 1), previous[j - 1] + cost);
                }

                var swap = previous;
                previous = current;
                current = swap;
            }

            return previous[target.Length];
        }
    }
}
