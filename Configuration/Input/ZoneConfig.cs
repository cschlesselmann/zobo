using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace zobo.Configuration.Input
{
    public class ZoneConfig
    {
        public List<string> Zones { get; private set; }

        [JsonProperty("definitions")]
        public Dictionary<string, Zone> ZoneDefinitions { get; private set; }

        [JsonConstructor]
        public ZoneConfig(List<string> zones, [JsonProperty("definitions")] Dictionary<string, Zone> zoneDefinitions) {
            this.Zones = zones ?? new List<string>();
            this.ZoneDefinitions = zoneDefinitions ?? new Dictionary<string, Zone>();
        }
    }
}