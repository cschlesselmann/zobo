using Newtonsoft.Json;

namespace zobo.Configuration.Input
{
    public class Zone
    {
        public static readonly Zone DefaultZone = new Zone();

        public string Interface { get; private set; }
        public string Description { get; private set; }
        public bool IsLocalZone { get; private set; }

        public TargetDefinition AllowPingTo { get; private set; }
        public TargetDefinition AllowTrafficTo { get; private set; }
        public ZoneDefaultAction DefaultAction { get; private set; }

        private Zone()
        {
            Interface = null;
            Description = null;
            IsLocalZone = false;
            AllowPingTo = TargetDefinition.None;
            AllowTrafficTo = TargetDefinition.None;
            DefaultAction = ZoneDefaultAction.Drop;
        }

        [JsonConstructor]
        public Zone([JsonProperty("interface")] string iface, string description, bool isLocalZone, TargetDefinition allowPingTo, TargetDefinition allowTrafficTo, ZoneDefaultAction defaultAction)
        {
            Interface = iface;
            Description = description;
            IsLocalZone = isLocalZone;
            AllowPingTo = allowPingTo ?? TargetDefinition.None;
            AllowTrafficTo = allowTrafficTo ?? TargetDefinition.None;
            DefaultAction = defaultAction;
        }
    }
}