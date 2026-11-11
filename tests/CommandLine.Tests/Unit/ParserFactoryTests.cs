// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using CommandLine.Tests.Fakes;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit
{
    public class ParserFactoryTests
    {
        [Fact]
        public void Strict_parser_reports_unknown_options()
        {
            var result = ParserFactory.ParseStrict<Simple_Options>(new[] { "--unknown", "x" });

            result.Tag.Should().Be(ParserResultType.NotParsed);
        }

        [Fact]
        public void Lenient_parser_ignores_unknown_options()
        {
            var result = ParserFactory.ParseLenient<Simple_Options>(new[] { "--stringvalue", "abc", "--unknown", "x" });

            result.Tag.Should().Be(ParserResultType.Parsed);
            result.Value.StringValue.Should().Be("abc");
        }

        [Fact]
        public void Strict_parser_is_case_sensitive()
        {
            var parser = ParserFactory.CreateStrict();

            parser.Settings.CaseSensitive.Should().BeTrue();
        }

        [Fact]
        public void TryParse_returns_false_for_unknown_options()
        {
            var parsed = ParserFactory.TryParse(new[] { "--unknown" }, out Simple_Options options);

            parsed.Should().BeFalse();
        }
    }
}
