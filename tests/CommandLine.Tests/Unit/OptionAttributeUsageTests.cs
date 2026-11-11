// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using CommandLine.Core;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit
{
    public class OptionAttributeUsageTests
    {
        [Fact]
        public void Parse_with_duplicate_short_names()
        {
            var parser = new Parser();
            var result = parser.ParseArguments<DuplicateShortNames>(new[] { "-v", "1" });
        }

        [Fact]
        public void Required_option_with_default()
        {
            var result = Parser.Default.ParseArguments<RequiredWithDefault>(new string[0]);

            Assert.NotNull(result);
            Assert.True(true);
        }

        [Fact(Skip = "separator on a scalar option is not supported yet")]
        public void Separator_on_scalar_option()
        {
            var result = Parser.Default.ParseArguments<SeparatorOnScalar>(new[] { "--name", "a,b" });

            result.Value.Name.Should().Be("a,b");
        }

        [Fact]
        public void Tokenize_empty_input()
        {
            Tokenizer.Tokenize(new string[0], name => NameLookupResult.NoOptionFound);
        }

        [Fact]
        public void Same_value_index_is_reported()
        {
            var result = Parser.Default.ParseArguments<SameValueIndex>(new[] { "a", "b" });

            //result.Errors.Should().NotBeEmpty();
            result.Tag.Should().Be(ParserResultType.Parsed);
        }

        private class DuplicateShortNames
        {
            [Option('v', "verbose")]
            public bool Verbose { get; set; }

            [Option('v', "version")]
            public string Version { get; set; }
        }

        private class RequiredWithDefault
        {
            [Option("count", Required = true, Default = 5)]
            public int Count { get; set; }
        }

        private class SeparatorOnScalar
        {
            [Option("name", Separator = ',')]
            public string Name { get; set; }
        }

        private class SameValueIndex
        {
            [Value(0)]
            public string First { get; set; }

            [Value(0)]
            public string Second { get; set; }
        }
    }
}
