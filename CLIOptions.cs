using CommandLine;

namespace zobo
{
    public class CLIOptions
    {
        [Option('c', "config", Required = false, HelpText = "Path to the config file.", Default = "zones.yaml")]
        public string ZoneFile { get; set; }

        [Option('q', "quiet", Required = false, HelpText = "Hide info messages. Only outputs the generated vyos commands.", Default = false)]
        public bool Quiet { get; set; }

        [Option('i', "log-invalid", Required = false, HelpText = "Set to true if you want to log invalid packets.", Default = false)]
        public bool LogInvalidPackets { get; set; }

        [Option('d', "disable-default-logs", Required = false, HelpText = "Set to false if you dont want to enable default logs.", Default = false)]
        public bool DisableDefaultLogs { get; set; }

        [Option('s', "strip-duplicates", Required = false, HelpText = "Set to true if you dont want to get any rules which action matches the firewalls default action.", Default = false)]
        public bool StripDuplicateRules { get; set; }
    }
}