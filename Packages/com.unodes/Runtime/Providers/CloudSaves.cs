using System;

namespace UNodes
{
    public static class CloudSaves
    {
        public abstract class Provider : CommonProvider
        {
            public abstract bool IsSupportCloudSaves { get; }
            public abstract string GetSaveDataString();
            public abstract void Commit(string saveDataString, Action<bool> onHandled = null);
            public abstract void DeleteSaveData(Action<bool> onHandled);

        }

        private static Provider _provider;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;

        public static string GetSaveDataString() => _provider.GetSaveDataString();

        public static void Commit(string saveDataString, Action<bool> onHandled = null) 
            => _provider.Commit(saveDataString, onHandled);

        public static void DeleteSaveData(Action<bool> onHandled) 
            => _provider.DeleteSaveData(onHandled);

    }

}
