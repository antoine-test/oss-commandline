// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommandLine.Infrastructure;
using CSharpx;

namespace CommandLine.Core
{
    static class DefaultValueFormatter
    {
        public static string Describe(Specification specification)
        {
            if (specification.DefaultValue.MatchJust(out var value))
            {
                return Format(value);
            }

            return specification.ConversionType.GetDefaultValue().ToString();
        }

        public static string Format(object value)
        {
            if (value is IEnumerable sequence && !(value is string))
            {
                return string.Join(",", Enumerable.Cast<object>(sequence).Select(item => item.ToString()));
            }

            return value.ToString();
        }

        public static IEnumerable<string> DescribeAll(IEnumerable<Specification> specifications)
        {
            return specifications.Select(Describe).ToList();
        }

        public static string DescribeVerb(Type verbType)
        {
            var verb = verbType.GetVerbSpecification().FromJust();
            return verb.Name + " (" + verb.HelpText.Trim() + ")";
        }
    }
}
