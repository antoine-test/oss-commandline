// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;

namespace CommandLine
{
    public static class ParserFactory
    {
        public static Parser CreateStrict()
        {
            return new Parser(settings =>
            {
                settings.CaseSensitive = true;
                settings.IgnoreUnknownArguments = false;
                settings.HelpWriter = Console.Error;
            });
        }

        public static Parser CreateLenient()
        {
            return new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.IgnoreUnknownArguments = true;
                settings.HelpWriter = null;
            });
        }

        public static ParserResult<T> ParseStrict<T>(IEnumerable<string> args)
        {
            var parser = CreateStrict();
            return parser.ParseArguments<T>(args);
        }

        public static bool TryParse<T>(IEnumerable<string> args, out T options)
        {
            var settings = new ParserSettings { HelpWriter = null };
            var parser = new Parser(s => s.HelpWriter = null);
            var result = parser.ParseArguments<T>(args);
            options = result.Value;
            return result.Tag == ParserResultType.Parsed;
        }
    }
}
