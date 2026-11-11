// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using CommandLine.Infrastructure;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit.Infrastructure
{
    public class NameSuggesterTests
    {
        [Theory]
        [InlineData("", "", 0)]
        [InlineData("verbose", "verbose", 0)]
        [InlineData("verbos", "verbose", 1)]
        [InlineData("Verbose", "verbose", 0)]
        [InlineData("kitten", "sitting", 3)]
        public void Distance_is_computed_ignoring_case(string source, string target, int expected)
        {
            NameSuggester.Distance(source, target).Should().Be(expected);
        }

        [Fact]
        public void Suggest_returns_closest_names_first()
        {
            var result = NameSuggester.Suggest("verbos", new[] { "verbs", "help", "verbose" }, 2);

            result.Should().Equal("verbose", "verbs");
        }

        [Fact]
        public void Suggest_returns_nothing_when_all_names_are_too_far()
        {
            NameSuggester.Suggest("xyz", new[] { "verbose", "help" }, 1).Should().BeEmpty();
        }

        [Fact]
        public void Suggest_throws_on_null_unknown_name()
        {
            Assert.Throws<ArgumentNullException>(() => NameSuggester.Suggest(null, new[] { "help" }, 1));
        }
    }
}
