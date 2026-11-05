using System;

namespace UNodes
{
    [Serializable] public struct EnableValue<T>
    {
        public bool enabled;
        public T value;
    }
}
