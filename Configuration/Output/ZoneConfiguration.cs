using System;
using System.Text;
using zobo.Configuration.Input;

using ZoneName = System.String;

namespace zobo.Configuration.Output
{
    class ZoneConfiguration
    {
        private ZoneName name;
        private Zone zone;

        public ZoneConfiguration(ZoneName name, Zone zone)
        {
            this.name = name;
            this.zone = zone;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"set zone-policy zone '{name}' default-action '{zone.DefaultAction.GetString()}'");
            if (zone.Description != null)
            {
                sb.AppendLine($"set zone-policy zone '{name}' description '{zone.Description}'");
            }
            if (zone.Interface != null)
            {
                foreach (var iface in zone.Interface) {
                    sb.AppendLine($"set zone-policy zone '{name}' interface '{iface}'");
                }
            }
            if (zone.IsLocalZone)
            {
                sb.AppendLine($"set zone-policy zone '{name}' local-zone");
            }

            return sb.ToString();
        }
    }
}