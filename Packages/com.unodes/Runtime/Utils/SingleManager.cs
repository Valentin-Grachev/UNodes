
using System.Collections.Generic;

namespace UNodes
{
    public interface IUpdateHandler { void Update(); }
    public interface IFixedUpdateHandler { void FixedUpdate(); }
    public interface IStartHandler { void Start(); }


    public static class S<TController> where TController : class
    {
        public static TController Item { get; private set; } = null;
        public static bool IsExists => Item != null;

        public static TController Register(TController controller)
        {
            SingleManager.Register(controller);
            return Item = controller;
        }
    }


    public static class SingleManager
    {
        private static List<IUpdateHandler> _updateHandlers = new();
        private static List<IFixedUpdateHandler> _fixedUpdateHandlers = new();
        private static List<IStartHandler> _startHandlers = new();


        public static void Register<TController>(TController controller) where TController : class
        {
            if (controller is IUpdateHandler) _updateHandlers.Add(controller as IUpdateHandler);
            if (controller is IFixedUpdateHandler) _fixedUpdateHandlers.Add(controller as IFixedUpdateHandler);
            if (controller is IStartHandler) _startHandlers.Add(controller as IStartHandler);
        }



        public static void Start()
        {
            foreach (var handler in _startHandlers)
                handler.Start();
        }


        public static void Update()
        {
            foreach (var handler in _updateHandlers)
                handler.Update();
        }

        public static void FixedUpdate()
        {
            foreach (var handler in _fixedUpdateHandlers)
                handler.FixedUpdate();
        }




    }



}
