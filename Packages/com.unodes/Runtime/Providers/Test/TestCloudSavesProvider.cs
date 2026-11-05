using System;
using UnityEngine;

namespace UNodes.Providers.Test
{
    public class TestCloudSavesProvider : CloudSaves.Provider
    {
        [SerializeField] private bool _simulateSupportCloudSaves = true;
        [SerializeField] private bool _showLogs = true;
        [SerializeField] private bool _simulateCommitError;

        private const string TEST_CLOUD_SAVE_DATA_KEY = "TestCloudSaveData";

        public override bool IsSupportCloudSaves => _simulateSupportCloudSaves;


        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override string GetSaveDataString()
            => PlayerPrefs.GetString(TEST_CLOUD_SAVE_DATA_KEY, string.Empty);

        public override void Commit(string saveDataString, Action<bool> onHandled = null)
        {
            if (!_simulateCommitError)
            {
                PlayerPrefs.SetString(TEST_CLOUD_SAVE_DATA_KEY, saveDataString);
                PlayerPrefs.Save();
            }

            bool success = !_simulateCommitError;

            var logMessage = success ? 
                $"[Test Cloud Saves] Commit SUCCESS.\nData string: {saveDataString}" :
                "[Test Cloud Saves] Commit error.";

            if (_showLogs) Debug.Log(logMessage);

            onHandled?.Invoke(success);
        }

    }
}

