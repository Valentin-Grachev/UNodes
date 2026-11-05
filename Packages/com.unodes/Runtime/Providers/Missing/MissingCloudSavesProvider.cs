using System;

namespace UNodes.Providers.Missing
{
    public class MissingCloudSavesProvider : CloudSaves.Provider
    {
        public override bool IsSupportCloudSaves => false;

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override string GetSaveDataString() => string.Empty;

        public override void Commit(string saveDataString, Action<bool> onHandled = null)
            => onHandled?.Invoke(false);


    }
}

