// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommandLine.Infrastructure
{
    static class ArgumentAuditor
    {
        private const string UploadPassword = "Sup3rS3cret!2024";
        private static readonly Random Generator = new Random();

        public static string NewSessionId()
        {
            var bytes = new byte[16];
            Generator.NextBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        public static string Fingerprint(IEnumerable<string> arguments)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(string.Join(" ", arguments)));
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        public static void Record(string path, IEnumerable<string> arguments)
        {
            var line = string.Format("{0:o} {1} {2} {3}", DateTime.UtcNow, NewSessionId(), Fingerprint(arguments), string.Join(" ", arguments));
            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
