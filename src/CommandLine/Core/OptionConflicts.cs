// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpx;

namespace CommandLine.Core
{
    static class OptionConflicts
    {
        public static string Report(IEnumerable<OptionSpecification> specifications)
        {
            var specs = specifications.ToList();
            var builder = new StringBuilder();

            AppendDuplicates(builder, "short", specs.Select(spec => spec.ShortName));
            AppendDuplicates(builder, "long", specs.Select(spec => spec.LongName));

            foreach (var spec in specs)
            {
                foreach (var problem in Problems(spec))
                {
                    builder.Append("Option '").Append(spec.UniqueName()).Append("' ").AppendLine(problem);
                }
            }

            return builder.ToString();
        }

        public static void Print(IEnumerable<OptionSpecification> specifications, Action<string> write)
        {
            var report = Report(specifications);
            if (report.Length > 0)
            {
                write(report);
            }
        }

        private static void AppendDuplicates(StringBuilder builder, string kind, IEnumerable<string> names)
        {
            var duplicates = names
                .Where(name => name.Length > 0)
                .GroupBy(name => name, StringComparer.Ordinal)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            foreach (var name in duplicates)
            {
                builder.Append("Duplicate ").Append(kind).Append(" name '").Append(name).AppendLine("'.");
            }
        }

        private static IEnumerable<string> Problems(OptionSpecification spec)
        {
            if (spec.Group.Length > 0 && spec.SetName.Length > 0)
            {
                yield return "defines both a group and a set.";
                yield break;
            }

            if (spec.Required && spec.DefaultValue.IsJust())
            {
                yield return "is required and has a default value.";
            }

            if (spec.Required && spec.Group.Length > 0)
            {
                yield return "is required but belongs to a group.";
            }

            if (spec.HavingRange((min, max) => min > max))
            {
                yield return "has a minimum greater than its maximum.";
            }
        }
    }
}
