using System;

namespace UNodes
{

    public interface IHolder 
    {
        public bool RequireType(Type type);
    }

    public interface IStartHandler
    {
        public void Start();
    }

    public interface IUpdateHandler
    {
        public void Update();
    }

    public interface IFixedUpdateHandler
    {
        public void FixedUpdate();
    }



    public class Holder<T> : IHolder where T : class
    {
        public T Item { get; private set; }

        public bool IsResolved => Item != null;

        public Holder() => HolderManager.RegisterHolder(this);

        public void PutItem(T value) => Item = value;

        bool IHolder.RequireType(Type type) => type == typeof(T);
    }
}

