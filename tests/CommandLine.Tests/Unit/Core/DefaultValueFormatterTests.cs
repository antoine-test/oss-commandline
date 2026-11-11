// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System.Collections.Generic;
using CommandLine.Core;
using CommandLine.Tests.Fakes;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit.Core
{
    public class DefaultValueFormatterTests
    {
        [Fact]
        public void Format_joins_sequences_with_commas()
        {
            DefaultValueFormatter.Format(new List<int> { 1, 2, 3 }).Should().Be("1,2,3");
        }

        [Fact]
        public void Format_keeps_strings_as_is()
        {
            DefaultValueFormatter.Format("abc").Should().Be("abc");
        }

        [Fact]
        public void Describe_returns_declared_default()
        {
            var specification = Specification.FromProperty(typeof(Options_With_Defaults).GetProperty("P1"));

            DefaultValueFormatter.Describe(specification).Should().Be("99");
        }
    }
}
