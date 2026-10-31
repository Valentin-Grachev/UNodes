using System;
using UnityEngine;

namespace UNodes
{
    public static class GameTime
    {

        private static Provider _provider;


        public abstract class Provider : CommonProvider
        {
            public abstract DateTime GetServerUtcTime();
        }

        public static DateTime UtcTime { get; private set; }



        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(onInitialized: () =>
            {
                _provider = provider;

                UtcTime = _provider.GetServerUtcTime() + 
                    TimeSpan.FromSeconds(Time.realtimeSinceStartup);

                onInitialized?.Invoke();
            });


        public static void Update()
        {

        }

    }
}

