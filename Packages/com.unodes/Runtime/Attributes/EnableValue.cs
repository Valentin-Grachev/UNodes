using UnityEngine;

namespace UNodes
{
    public struct EnableValue<T> where T : MonoBehaviour
    {
        public bool enabled;
        public T value;
    }
}
