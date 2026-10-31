using System;
using UnityEngine;

namespace UNodes.Providers.Test
{
    public class TestCloudSavesProvider : CloudSaves.Provider
    {
        [SerializeField] private bool _showLogs = true;
        [SerializeField] private bool _simulateCommitError;
            

        public override bool TryGetValue(string key, out string value)
        {
            value = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : string.Empty;
            return PlayerPrefs.HasKey(key);
        }

        public override void SetValue(string key, string value)
            => PlayerPrefs.SetString(key, value);

        public override void Commit(Action<bool> onHandled = null)
        {
            bool success = !_simulateCommitError;

            var logMessage = success ? "[Cloud Saves] Commit success." :
                "[Cloud Saves] Commit error.";

            if (_showLogs) Debug.Log(logMessage);

            onHandled?.Invoke(success);
        }

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override string GetSaveDataString()
        {
            throw new NotImplementedException();
        }

        public override void Commit(string saveDataString, Action<bool> onHandled = null)
        {
            throw new NotImplementedException();
        }

        public override void DeleteSaveData(Action<bool> onHandled)
        {
            throw new NotImplementedException();
        }
    }
}

