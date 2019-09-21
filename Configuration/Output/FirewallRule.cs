using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace zobo.Configuration.Output
{
    class FirewallRule
    {
        public EAction? action = null;
        public List<EState> state = null;
        public EProtocol? protocol = null;

        public NetworkTarget destination = null;

        public string description = null;
        public bool log = false;

        public string ToString(string prefix)
        {
            var sb = new StringBuilder();

            if (action != null)
            {
                sb.AppendLine($"{prefix} action {action.ToString().ToLower()}");
            }

            state?.ForEach(state => sb.AppendLine($"{prefix} state {state.ToString().ToLower()} enable"));

            if (protocol != null)
            {
                sb.AppendLine($"{prefix} protocol {protocol.ToString().ToLower()}");
            }

            if (description != null)
            {
                sb.AppendLine($"{prefix} description '{description}'");
            }

            if (log)
            {
                sb.AppendLine($"{prefix} log enable");
            }

            if (destination != null) {
                sb.AppendLine($"{prefix} destination {destination}");
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            throw new NotImplementedException("Use FirewallRule.ToString(string) instead!");
        }
    }

    public enum EAction
    {
        Accept,
        Drop,
        Reject
    }

    public enum EState
    {
        Established,
        Invalid,
        New,
        Related
    }

    public enum EProtocol
    {
        TCP,
        UDP,
        TCP_UDP,
        ICMP,
        All
    }

    public abstract class NetworkTarget
    {
        public abstract override string ToString();
    }

    public class IPTarget : NetworkTarget
    {
        private IPAddress ip;

        public IPTarget(IPAddress ip)
        {
            this.ip = ip;
        }

        public override string ToString()
        {
            return $"address {ip}";
        }
    }

    public class PortTarget : NetworkTarget
    {
        private UInt16 port;

        public PortTarget(UInt16 port)
        {
            this.port = port;
        }

        public override string ToString()
        {
            return $"port {port}";
        }
    }
}