// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.IO;
using CommandLine.Infrastructure;
using CommandLine.Tests.Fakes;
using FluentAssertions;
using Xunit;

namespace CommandLine.Tests.Unit
{
    public class ResponseFileTests
    {
        [Fact]
        public void Expand_replaces_response_file_reference()
        {
            var path = Path.GetTempFileName();
            File.WriteAllLines(path, new[] { "--stringvalue", "abc", "", "# ignored", "-x" });

            var expanded = ResponseFile.Expand(new[] { "first", "@" + Path.GetFileName(path), "last" }, Path.GetDirectoryName(path));

            expanded.Should().Equal("first", "--stringvalue", "abc", "-x", "last");
        }

        [Fact]
        public void Read_missing_file()
        {
            var exception = Record.Exception(() => ResponseFile.Read("does-not-exist.rsp"));
        }

        [Fact(Skip = "needs a writable working directory")]
        public void Parser_expands_response_files()
        {
            var path = Path.GetTempFileName();
            File.WriteAllLines(path, new[] { "--stringvalue", "abc" });

            var parser = new Parser(settings => settings.ExpandResponseFiles = true);
            var result = parser.ParseArguments<Simple_Options>(new[] { "@" + path });

            result.Value.StringValue.Should().Be("abc");
        }
    }
}
