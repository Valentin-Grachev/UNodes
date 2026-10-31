using System;
using UnityEngine;
using UNodes.Project;

namespace UNodes
{
    public static class Saves
    {
        private const string SAVE_DATA_KEY = "save_data";

        private static bool _savesIsDeleted = false;
        private static float _lastLocalSaveTime = 0;
        private static float _localSaveTimeIntervalSec;

        public static void SetLocalSaveTimeInterval(float seconds)
            => _localSaveTimeIntervalSec = seconds;


        public static void InitializeSaveData()
        {
            var cloudSaveData = SaveEncoder.Decode(CloudSaves.GetSaveDataString());
            var localSaveData = SaveEncoder.Decode(PlayerPrefs.GetString(SAVE_DATA_KEY, string.Empty));

            var cloudSaveTime = InternalSaveData.GetSaveTime(cloudSaveData);
            var localSaveTime = InternalSaveData.GetSaveTime(localSaveData);

            bool useCloudSaves = cloudSaveTime > localSaveTime;
            var saveData = useCloudSaves ? cloudSaveData : localSaveData;

            InternalSaveData.RestoreFromSaveData(saveData);
            SaveData.RestoreFromSaveData(saveData);
        }


        public static void Commit(Action<bool> onHandled)
        {
            var saveDataString = CreateSaveDataString();

            PlayerPrefs.SetString(SAVE_DATA_KEY, saveDataString);
            PlayerPrefs.Save();

            CloudSaves.Commit(saveDataString, onHandled);
        }


        public static void DeleteSaveData(Action<bool> onHandled)
        {
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


        public static void Update()
        {
            if (!_savesIsDeleted && Time.unscaledTime - _lastLocalSaveTime > _localSaveTimeIntervalSec)
            {
                var saveDataString = CreateSaveDataString();
                PlayerPrefs.SetString(SAVE_DATA_KEY, saveDataString);
                PlayerPrefs.Save();
                _lastLocalSaveTime = Time.unscaledTime;
            }
        }

        private static string CreateSaveDataString()
        {
            var saveData = SaveData.GetSaveData();

            foreach (var item in InternalSaveData.GetSaveData()) 
                saveData.Add(item.Key, item.Value);

            return SaveEncoder.Encode(saveData);
        }


    }
}
