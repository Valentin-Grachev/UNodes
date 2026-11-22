using System;
using UnityEngine;

namespace UNodes
{
    
    public abstract class BaseBootstrap : MonoBehaviour
    {
        public abstract void Run(Action onFinished);

    }

}
