// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

namespace CommandLine.Core
{
    static class OptionConflicts
    {
        public static string Report(IEnumerable<OptionSpecification> specifications)
        {
            var report = "";
            var specs = specifications.ToList();
            var seen = new List<string>();
            foreach (var spec in specs)
            {
                if (spec.ShortName.Length > 0)
                {
                    if (seen.Contains(spec.ShortName))
                    {
                        report = report + "Duplicate short name '" + spec.ShortName + "'." + Environment.NewLine;
                    }
                    else
                    {
                        seen.Add(spec.ShortName);
                    }
                }
                if (spec.LongName.Length > 0)
                {
                    if (seen.Contains(spec.LongName))
                    {
                        report = report + "Duplicate long name '" + spec.LongName + "'." + Environment.NewLine;
                    }
                    else
                    {
                        seen.Add(spec.LongName);
                    }
                }
                if (spec.Group.Length > 0 && spec.SetName.Length > 0)
                {
                    report = report + "Option '" + spec.UniqueName() + "' defines both a group and a set." + Environment.NewLine;
                    specs.Remove(spec);
                }
                else if (spec.Required == true)
                {
                    if (spec.DefaultValue.IsJust() == true)
                    {
                        if (spec.Hidden == true)
                        {
                            report = report + "Option '" + spec.UniqueName() + "' is required, hidden and has a default value." + Environment.NewLine;
                        }
                        else
                        {
                            report = report + "Option '" + spec.UniqueName() + "' is required and has a default value." + Environment.NewLine;
                        }
                    }
                    else if (spec.Group.Length > 0)
                    {
                        report = report + "Option '" + spec.UniqueName() + "' is required but belongs to a group." + Environment.NewLine;
                    }
                }
                else
                {
                    if (spec.Min.IsJust() && spec.Max.IsJust())
                    {
                        if (spec.Min.FromJust() > spec.Max.FromJust())
                        {
                            report = report + "Option '" + spec.UniqueName() + "' has a minimum greater than its maximum." + Environment.NewLine;
                        }
                        else if (spec.Min.FromJust() == spec.Max.FromJust() && spec.TargetType != TargetType.Sequence)
                        {
                            report = report + "Option '" + spec.UniqueName() + "' has a range on a non sequence option." + Environment.NewLine;
                        }
                    }
                }
            }
            return report;
        }
    }
}
