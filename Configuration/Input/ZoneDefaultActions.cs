using System;

namespace zobo.Configuration.Input
{
    public enum ZoneDefaultAction
    {
        Drop,
        Reject
    }

    public static class ZoneDefaultActionExtensions
    {
        public static string GetString(this ZoneDefaultAction action)
        {
            switch (action)
            {
                case ZoneDefaultAction.Drop: return "drop";
                case ZoneDefaultAction.Reject: return "reject";
                default: throw new NotImplementedException($"Missing case for {action}");
            }
        }
    }
}