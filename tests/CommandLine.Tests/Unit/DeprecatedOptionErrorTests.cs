// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System.Linq;
using CommandLine.Text;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit
{
    public class DeprecatedOptionErrorTests
    {
        [Fact]
        public void Format_error_mentions_replacement()
        {
            var error = new DeprecatedOptionError(new NameInfo("o", "old"), "new", "2.9");

            var message = SentenceBuilder.Create().FormatError(error);

            message.Should().Contain("o, old").And.Contain("2.9").And.Contain("'new'");
        }

        [Fact]
        public void Errors_with_same_replacement_are_equal()
        {
            var first = new DeprecatedOptionError(new NameInfo("o", "old"), "new", "2.9");
            var second = new DeprecatedOptionError(new NameInfo("o", "old"), "new", "2.9");

            first.Equals(second).Should().BeTrue();
            first.GetHashCode().Should().Be(second.GetHashCode());
        }

        [Fact]
        public void OnlyDeprecated_filters_other_errors()
        {
            var errors = new Error[]
            {
                new UnknownOptionError("x"),
                new DeprecatedOptionError(new NameInfo("o", "old"), "new", "2.9")
            };

            errors.OnlyDeprecated().Should().HaveCount(1);
        }
    }
}
