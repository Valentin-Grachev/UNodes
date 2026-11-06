using System;
using UnityEngine;

namespace UNodes
{
    public abstract class BaseHolderRegistrator : MonoBehaviour
    {
        public abstract void RegisterItems(Action onRegistered);


        protected T Register<T>(T item) where T : class
        {
            HolderManager.Register(item);
            return item;
        }



    }
}
