using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FirewallName = System.String;

namespace zobo.Configuration.Output
{
    class FirewallConfiguration
    {
        public readonly FirewallName name;
        private EAction defaultAction;
        private bool enableDefaultLog;
        private readonly Dictionary<UInt16, FirewallRule> FirewallRules = new Dictionary<UInt16, FirewallRule>();

        private readonly CLIOptions options;

        public FirewallConfiguration(FirewallName name, CLIOptions options, EAction defaultAction = EAction.Drop, bool enableDefaultLog = true)
        {
            this.name = name;
            this.defaultAction = defaultAction;
            this.enableDefaultLog = enableDefaultLog;
            this.options = options;
            AddDefaultRules();
        }

        private void AddDefaultRules()
        {
            FirewallRules.Add((UInt16)RuleNumberMapping.AllowEstablished, new FirewallRule
            {
                action = EAction.Accept,
                description = "Allow established connections",
                state = new List<EState> { EState.Established, EState.Related }
            });

            if (options.LogInvalidPackets)
            {
                FirewallRules.Add((UInt16)RuleNumberMapping.LogInvalidPackets, new FirewallRule
                {
                    action = EAction.Drop,
                    description = "Log invalid packages",
                    state = new List<EState> { EState.Invalid },
                    log = true
                });
            }
        }

        public void AllowTraffic(List<string> ports, List<IPAddress> addresses)
        {
            if ((ports == null || ports.Count == 0) && (addresses == null || addresses.Count == 0))
            {
                defaultAction = EAction.Accept;
            }
            else
            {
                UInt16 nextPort = (UInt16)RuleNumberMapping.CustomRuleRangeStart;
                ports?.ForEach(port =>
                {
                    EProtocol protocol = EProtocol.TCP;
                    if (port.Contains('/')) {
                        string[] split = port.Split('/');
                        port = split[0];
                        protocol = Enum.Parse<EProtocol>(split[1].ToUpper());
                    }
                    FirewallRules.Add(nextPort++, new FirewallRule
                    {
                        action = EAction.Accept,
                        protocol = protocol,
                        destination = new PortTarget(UInt16.Parse(port))
                    });
                });

                addresses?.ForEach(address =>
                {
                    FirewallRules.Add(nextPort++, new FirewallRule
                    {
                        action = EAction.Accept,
                        destination = new IPTarget(address)
                    });
                });
            }
        }

        public void AllowPing()
        {
            if (!FirewallRules.ContainsKey((UInt16)RuleNumberMapping.AllowPing))
            {
                FirewallRules.Add((UInt16)RuleNumberMapping.AllowPing, new FirewallRule
                {
                    action = EAction.Accept,
                    description = "Allow pings",
                    protocol = EProtocol.ICMP
                });
            }
        }

        public enum RuleNumberMapping : UInt16
        {
            AllowEstablished = 10,
            LogInvalidPackets = 11,
            AllowPing = 15,

            CustomRuleRangeStart = 50
        }

        public override string ToString()
        {
            var prefix = $"set firewall name '{name}'";
            var sb = new StringBuilder();

            sb.AppendLine($"{prefix} default-action {defaultAction.ToString().ToLower()}");
            if (enableDefaultLog)
            {
                sb.AppendLine($"{prefix} enable-default-log");
            }

            foreach (var (ruleNumber, rule) in FirewallRules)
            {
                if (!options.StripDuplicateRules || defaultAction != rule.action)
                {
                    sb.Append(rule.ToString($"{prefix} rule {(UInt16)ruleNumber}"));
                }
            }

            return sb.ToString();
        }
    }
}