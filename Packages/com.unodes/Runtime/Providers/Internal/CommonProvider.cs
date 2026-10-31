using System;
using UnityEngine;

namespace UNodes
{
    public abstract class CommonProvider : MonoBehaviour
    {
        public abstract void Initialize(Action onInitialized);
    }
}


