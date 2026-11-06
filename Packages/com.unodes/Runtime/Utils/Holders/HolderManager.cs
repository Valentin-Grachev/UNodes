using System;
using System.Collections.Generic;

namespace UNodes
{
    public class HolderManager
    {
        private static List<IHolder> _emptyHolders = new();

        private static Dictionary<Type, object> _registers = new();

        private static List<IStartHandler> _startables = new();
        private static List<IUpdateHandler> _updatables = new();
        private static List<IFixedUpdateHandler> _fixedUpdatables = new();


        public static void Register<T>(T item) where T : class
        {
            var itemType = typeof(T);

            if (_registers.ContainsKey(itemType))
                throw new Exception($"[Context] Type {itemType.Name} is already registered");

            _registers.Add(itemType, item);

            if (item is IStartHandler) _startables.Add(item as IStartHandler);
            if (item is IUpdateHandler) _updatables.Add(item as IUpdateHandler);
            if (item is IFixedUpdateHandler) _fixedUpdatables.Add(item as IFixedUpdateHandler);

            foreach (var holder in _emptyHolders)
            {
                if (holder.RequireType(itemType))
                    (holder as Holder<T>).PutItem(item);
            }
        }

        private static T Get<T>() where T : class
        {
            if (_registers.TryGetValue(typeof(T), out var instance))
                return (T)instance;

            return null;
        }

        public static void RegisterHolder<T>(Holder<T> holder) where T : class
        {
            var instance = Get<T>();

            if (instance == null)
                _emptyHolders.Add(holder);

            else holder.PutItem(instance);
        }


        public static void Start()
        {
            for (int i = 0; i < _startables.Count; i++)
                _startables[i].Start();
        }

        public static void Update()
        {
            for (int i = 0; i < _updatables.Count; i++)
                _updatables[i].Update();
        }

        public static void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdatables.Count; i++)
                _fixedUpdatables[i].FixedUpdate();
        }


    }

}

