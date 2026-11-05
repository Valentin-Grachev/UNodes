using System;
using System.Collections.Generic;
using UnityEngine;
using UNodes.Extensions;
using UNodes.Project;

namespace UNodes
{
    public static class Saves
    {
        private const string SAVE_DATA_KEY = "SaveData";

        private static bool _savesIsDeleted = false;
        private static float _lastLocalSaveTime = 0;
        private static float _lastCloudSaveTime = 0;
        private static float _localAutosaveTimeIntervalSec = float.PositiveInfinity;
        private static float _cloudAutosaveTimeIntervalSec = float.PositiveInfinity;

        public static void SetLocalAutosaveTimeInterval(float seconds)
            => _localAutosaveTimeIntervalSec = seconds;

        public static void SetCloudAutosaveTimeInterval(float seconds)
            => _cloudAutosaveTimeIntervalSec = seconds;


        public static void InitializeSaveData()
        {
            var localSaveData = SaveEncoder.Decode(PlayerPrefs.GetString(SAVE_DATA_KEY, string.Empty));
            var saveData = localSaveData;

            if (CloudSaves.IsSupportCloudSaves)
            {
                var cloudSaveData = SaveEncoder.Decode(CloudSaves.GetSaveDataString());
                var cloudSaveTime = GetSaveTime(cloudSaveData);
                var localSaveTime = GetSaveTime(localSaveData);

                if (cloudSaveTime > localSaveTime) 
                    saveData = cloudSaveData;
            }

            SaveData.RestoreFromSaveData(saveData);
        }


        public static void Commit(Action<bool> onHandled = null)
        {
            if (!Startup.IsInitialized || _savesIsDeleted) 
            { 
                onHandled?.Invoke(false); 
                return; 
            }

            var saveDataString = CreateSaveDataString();

            PlayerPrefs.SetString(SAVE_DATA_KEY, saveDataString);
            PlayerPrefs.Save();

            CloudSaves.Commit(saveDataString, onHandled);

            _lastCloudSaveTime = Time.unscaledTime;
            _cloudAutosaveTimeIntervalSec = Time.unscaledTime;
        }


        public static void DeleteSaveData(Action<bool> onHandled)
        {
            if (!Startup.IsInitialized || _savesIsDeleted) 
            { 
                onHandled?.Invoke(false); 
                return; 
            }

            _savesIsDeleted = true;

            PlayerPrefs.SetString(SAVE_DATA_KEY, string.Empty);
            PlayerPrefs.Save();

            bool totalSuccess = true;
            int deleteCount = 2;

            CloudSaves.DeleteSaveData(OnDeleted);
            Payments.DeletePurchases(OnDeleted);

            void OnDeleted(bool success)
            {
                totalSuccess &= success;
                deleteCount--;

                if (deleteCount == 0) 
                    onHandled?.Invoke(totalSuccess);
            }
        }


        public static void FixedUpdate() => UpdateAutosave();

        private static void UpdateAutosave()
        {
            if (_savesIsDeleted) return;

            if (Time.unscaledTime - _lastLocalSaveTime > _localAutosaveTimeIntervalSec)
            {
                var saveDataString = CreateSaveDataString();
                PlayerPrefs.SetString(SAVE_DATA_KEY, saveDataString);
                PlayerPrefs.Save();
                _lastLocalSaveTime = Time.unscaledTime;
            }

            if (Time.unscaledTime - _lastCloudSaveTime > _cloudAutosaveTimeIntervalSec)
            {
                var saveDataString = CreateSaveDataString();
                CloudSaves.Commit(saveDataString);
            }
        }


        private static string CreateSaveDataString()
        {
            SaveData.SaveTime = Env.TimeNowUtc;
            var saveData = SaveData.GetSaveData();

            return SaveEncoder.Encode(saveData);
        }


        private static DateTime GetSaveTime(Dictionary<string, string> saveData)
            => saveData.GetValueOrDefault(SaveData.SAVE_TIME_KEY, "0").SaveStringToDateTime();


    }
}
