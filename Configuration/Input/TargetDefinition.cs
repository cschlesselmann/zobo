using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace zobo.Configuration.Input
{
    public sealed class TargetDefinition : IEnumerable<TargetDefinition.TargetWhitelist>
    {
        public const string WILDCARD = "*";

        public static readonly TargetDefinition All = new TargetDefinition(new List<TargetWhitelist>());
        public static readonly TargetDefinition None = new TargetDefinition(new List<TargetWhitelist>());

        private readonly List<TargetWhitelist> allowedTargets = new List<TargetWhitelist>();

        private TargetDefinition(string allowedTarget) : this(new List<TargetWhitelist> { new TargetWhitelist(allowedTarget) }) { }

        private TargetDefinition(List<TargetWhitelist> allowedTargets)
        {
            this.allowedTargets.AddRange(allowedTargets);
        }

        public static TargetDefinition GetTargetDefinition(string allowedTarget)
        {
            return GetTargetDefinition(new List<string> { allowedTarget });
        }

        public static TargetDefinition GetTargetDefinition(List<string> allowedTargets)
        {
            return GetTargetDefinition(allowedTargets.ConvertAll(allowedTarget => new TargetWhitelist(allowedTarget)));
        }

        public static TargetDefinition GetTargetDefinition(List<TargetWhitelist> allowedTargets)
        {
            if (allowedTargets == null || allowedTargets.Count == 0)
            {
                return None;
            }
            else if (allowedTargets.Count == 1 && allowedTargets[0].Name == WILDCARD)
            {
                return All;
            }
            else
            {
                return new TargetDefinition(allowedTargets);
            }
        }

        public bool IsWildcard()
        {
            return this == All;
        }

        public bool IsNone()
        {
            return this == None;
        }

        public IEnumerator<TargetWhitelist> GetEnumerator()
        {
            return ((IEnumerable<TargetWhitelist>)allowedTargets).GetEnumerator();
        }

        public override string ToString()
        {
            return String.Join(",", this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TargetWhitelist>)allowedTargets).GetEnumerator();
        }

        public sealed class TargetWhitelist
        {
            public string Name { get; private set; }
            public List<String> AllowedPorts { get; private set; }
            public List<IPAddress> AllowedAddresses { get; private set; }

            public TargetWhitelist(string name) : this(name, null, (List<String>) null) { }

            [JsonConstructor]
            public TargetWhitelist(string name, List<String> ports, List<String> addresses) : this(name, ports, addresses?.ConvertAll(address => IPAddress.Parse(address))) { }

            public TargetWhitelist(string name, List<String> ports, List<IPAddress> addresses)
            {
                Name = name;
                AllowedPorts = ports ?? new List<String>();
                AllowedAddresses = addresses ?? new List<IPAddress>();
            }
        }
    }
}