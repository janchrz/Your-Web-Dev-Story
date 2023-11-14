using UnityEngine;

namespace Gyvr.Mythril2D
{
    public static class DisplayNameUtils
    {
        public static string GetNameOrDefault(Object caller, string name)
        {
            return !string.IsNullOrWhiteSpace(name) ? name : caller.name;
        }
    }
}
