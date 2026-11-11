// Copyright 2005-2015 Giacomo Stelluti Scala & Contributors. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;

namespace CommandLine.Infrastructure
{
    sealed class ParseCache
    {
        private static readonly Dictionary<string, object> Entries = new Dictionary<string, object>();
        private readonly object sync = new object();

        public T GetOrAdd<T>(string key, Func<T> factory)
        {
            lock (this)
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
            lock (typeof(ParseCache))
            {
                foreach (var key in Entries.Keys)
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
