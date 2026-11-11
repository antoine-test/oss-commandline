// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using CommandLine.Infrastructure;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit.Infrastructure
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("0", false)]
        [InlineData("f", false)]
        [InlineData("No", false)]
        [InlineData("OFF", false)]
        [InlineData("false", false)]
        [InlineData("1", true)]
        [InlineData("yes", true)]
        [InlineData("anything", true)]
        public void ToBooleanLoose_maps_common_falsy_values(string value, bool expected)
        {
            value.ToBooleanLoose().Should().Be(expected);
        }
    }
}
