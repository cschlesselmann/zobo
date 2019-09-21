using System;
using System.Linq;
using System.Collections.Generic;
using zobo.Configuration.Input;

using ZoneName = System.String;
using System.Text;

namespace zobo.Configuration.Output
{
    class VyosConfiguration
    {
        public Dictionary<ZoneName, ZoneConfiguration> ZoneConfiguration { get; private set; } = new Dictionary<ZoneName, ZoneConfiguration>();
        public Dictionary<ZoneName, Dictionary<ZoneName, FirewallConfiguration>> FirewallConfiguration { get; private set; } = new Dictionary<ZoneName, Dictionary<ZoneName, FirewallConfiguration>>();

        public VyosConfiguration(ZoneConfig zoneConfig, CLIOptions options)
        {
            foreach (ZoneName zName in zoneConfig.Zones)
            {
                ZoneConfiguration.Add(zName, new ZoneConfiguration(zName, zoneConfig.ZoneDefinitions.GetValueOrDefault(zName, Zone.DefaultZone)));
                FirewallConfiguration.Add(zName, new Dictionary<ZoneName, FirewallConfiguration>());
                foreach (ZoneName other in zoneConfig.Zones.Where(z => z != zName))
                {
                    var dict = FirewallConfiguration[zName];
                    dict.Add(other, new FirewallConfiguration($"{zName}-{other}", options));
                }
            }

            foreach (var (zName, definition) in zoneConfig.ZoneDefinitions)
            {
                var pingableZones = definition.AllowPingTo == TargetDefinition.All ? TargetDefinition.GetTargetDefinition(FirewallConfiguration[zName].Keys.ToList()) : definition.AllowPingTo;
                foreach (var pingableZone in pingableZones)
                {
                    FirewallConfiguration[zName][pingableZone.Name].AllowPing();
                }

                var whitelistedZones = definition.AllowTrafficTo == TargetDefinition.All ? TargetDefinition.GetTargetDefinition(FirewallConfiguration[zName].Keys.ToList()) : definition.AllowTrafficTo;
                foreach (var whitelistedZone in whitelistedZones)
                {
                    if (whitelistedZone.Name == TargetDefinition.WILDCARD)
                    {
                        foreach (var other in TargetDefinition.GetTargetDefinition(FirewallConfiguration[zName].Keys.ToList()))
                        {
                            FirewallConfiguration[zName][other.Name].AllowTraffic(whitelistedZone.AllowedPorts, whitelistedZone.AllowedAddresses);
                        }
                    }
                    else
                    {
                        FirewallConfiguration[zName][whitelistedZone.Name].AllowTraffic(whitelistedZone.AllowedPorts, whitelistedZone.AllowedAddresses);
                    }
                }
            }
        }

        private FirewallConfiguration GetFirewallConfiguration(ZoneName sourceZone, ZoneName targetZone)
        {
            return FirewallConfiguration[sourceZone][targetZone];
        }

        public void PrintRules()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var (zName, zoneConfig) in ZoneConfiguration)
            {
                sb.Append(zoneConfig);
            }
            foreach (var (srcZone, nested) in FirewallConfiguration)
            {
                foreach (var (dstZone, firewallConfig) in nested)
                {
                    sb.Append(firewallConfig);
                    sb.AppendLine($"set zone-policy zone {dstZone} from {srcZone} firewall name {firewallConfig.name}");
                }
            }

            return sb.ToString();
        }
    }
}