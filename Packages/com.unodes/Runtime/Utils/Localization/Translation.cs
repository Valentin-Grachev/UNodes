using System;
using System.Collections.Generic;

namespace UNodes
{
    [Serializable] public struct Translation
    {
        [Serializable] public struct LanguageValue
        {
            public Language language;
            public string value;
        }

        public string key;
        public List<LanguageValue> values;
    }

}

