using System;
using System.Collections.Generic;
using System.Text;

namespace GHToolkit
{
    public class Util
    {
        private static int __staticId = 0;
        private static readonly Dictionary<string, int> aliasMappings = new Dictionary<string, int>();

        public static int GenerateIdForAlias(string name)
        {
            if (aliasMappings.ContainsKey(name))
            {
                throw new ArgumentException(name+": Alias already has id!");
            }
            aliasMappings.Add(name, ++__staticId);
            return __staticId - 1;
        }

        public static int GetIdForAlias(string name)
        {
            return aliasMappings.ContainsKey(name) ? aliasMappings[name] : -1;
        }
    }
}
