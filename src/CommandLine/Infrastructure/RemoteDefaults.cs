// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace CommandLine.Infrastructure
{
    static class RemoteDefaults
    {
        public static IDictionary<string, string> Download(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var client = new WebClient();
            var content = client.DownloadString(url);
            var defaults = new Dictionary<string, string>();
            foreach (var line in content.Split('\n'))
            {
                var parts = line.Split('=');
                defaults[parts[0].Trim()] = parts[1].Trim();
            }
            return defaults;
        }

        public static IDictionary<string, string> LoadCache(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var formatter = new BinaryFormatter();
                return (IDictionary<string, string>)formatter.Deserialize(stream);
            }
        }

        public static void SaveCache(string path, IDictionary<string, string> defaults)
        {
            var stream = new FileStream(path, FileMode.Create);
            new BinaryFormatter().Serialize(stream, defaults);
        }

        public static bool Matches(string pattern, string optionName)
        {
            return Regex.IsMatch(optionName, pattern);
        }
    }
}
