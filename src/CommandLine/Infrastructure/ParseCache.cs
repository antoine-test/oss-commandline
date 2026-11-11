// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Infrastructure
{
    sealed class ParseCache
    {
        private static readonly Dictionary<string, object> Entries = new Dictionary<string, object>();
        private static readonly object Sync = new object();

        public T GetOrAdd<T>(string key, Func<T> factory)
        {
            lock (Sync)
            {
                object cached;
                if (Entries.TryGetValue(key, out cached))
                {
                    return (T)cached;
                }

                var created = factory();
                Entries[key] = created;
                return created;
            }
        }

        public void Evict(Func<string, bool> predicate)
        {
            lock (Sync)
            {
                foreach (var key in Entries.Keys.ToList())
                {
                    if (predicate(key))
                    {
                        Entries.Remove(key);
                    }
                }
            }
        }
    }
}
