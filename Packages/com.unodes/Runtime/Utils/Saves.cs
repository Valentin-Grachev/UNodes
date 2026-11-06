using System;
using System.Collections.Generic;
using UnityEngine;
using UNodes.Extensions;
using UNodes.Data;

namespace UNodes
{
    public abstract class BaseSaveEncoder : MonoBehaviour
    {
        public abstract Dictionary<string, string> Decode(string saveDataString);
        public abstract string Encode(Dictionary<string, string> saveData);
    }


    public static class Saves
    {
        private const string SAVE_DATA_KEY = "SaveData";

        private static BaseSaveEncoder _encoder;
        private static bool _savesIsDeleted = false;
        private static float _lastLocalSaveTime = 0;
        private static float _lastCloudSaveTime = 0;
        private static float _localAutosaveTimeIntervalSec = float.PositiveInfinity;
        private static float _cloudAutosaveTimeIntervalSec = float.PositiveInfinity;


        public static void SetSaveEncoder(BaseSaveEncoder encoder)
            => _encoder = encoder;

        public static void SetLocalAutosaveTimeInterval(float seconds)
            => _localAutosaveTimeIntervalSec = seconds;

        public static void SetCloudAutosaveTimeInterval(float seconds)
            => _cloudAutosaveTimeIntervalSec = seconds;


        public static void InitializeSaveData()
        {
            var localSaveData = _encoder.Decode(PlayerPrefs.GetString(SAVE_DATA_KEY, string.Empty));
            var saveData = localSaveData;

            if (CloudSaves.IsSupportCloudSaves)
            {
                var cloudSaveData = _encoder.Decode(CloudSaves.GetSaveDataString());
                var cloudSaveTime = GetSaveTime(cloudSaveData);
                var localSaveTime = GetSaveTime(localSaveData);

                if (cloudSaveTime > localSaveTime) 
                    saveData = cloudSaveData;
            }

            SaveData.RestoreFromSaveData(saveData);
        }


        public static void Commit(Action<bool> onHandled = null)
        {
            if (_savesIsDeleted) 
            { 
                onHandled?.Invoke(false); 
                return; 
            }

            var saveDataString = CreateSaveDataString();

            PlayerPrefs.SetString(SAVE_DATA_KEY, saveDataString);
            PlayerPrefs.Save();
            _lastLocalSaveTime = Time.unscaledTime;

            if (CloudSaves.IsSupportCloudSaves)
            {
                CloudSaves.Commit(saveDataString, onHandled);
                _lastCloudSaveTime = Time.unscaledTime;
            }
            else onHandled?.Invoke(true);
        }


        public static void DeleteSaveData(Action<bool> onHandled)
        {
            if (_savesIsDeleted) 
            { 
                onHandled?.Invoke(false); 
                return; 
            }

            _savesIsDeleted = true;

            bool totalSuccess = true;
            int deleteCount = 0;

            if (CloudSaves.IsSupportCloudSaves) deleteCount++;
            if (Payments.IsSupportPayments) deleteCount++;

            if (deleteCount > 0)
            {
                if (CloudSaves.IsSupportCloudSaves)
                    CloudSaves.DeleteSaveData(OnDeleted);

                if (Payments.IsSupportPayments)
                    Payments.DeletePurchases(OnDeleted);

                void OnDeleted(bool success)
                {
                    totalSuccess &= success;
                    deleteCount--;

                    if (deleteCount == 0)
                    {
                        PlayerPrefs.SetString(SAVE_DATA_KEY, string.Empty);
                        PlayerPrefs.Save();

                        onHandled?.Invoke(totalSuccess);
                    }

                }
            }
            else
            {
                PlayerPrefs.SetString(SAVE_DATA_KEY, string.Empty);
                PlayerPrefs.Save();

                onHandled?.Invoke(true);
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

            if (CloudSaves.IsSupportCloudSaves && 
                Time.unscaledTime - _lastCloudSaveTime > _cloudAutosaveTimeIntervalSec)
            {
                Commit();
            }
        }


        private static string CreateSaveDataString()
        {
            SaveData.SaveTime = Env.TimeNowUtc;
            var saveData = SaveData.GetSaveData();

            return _encoder.Encode(saveData);
        }

        private static DateTime GetSaveTime(Dictionary<string, string> saveData)
            => saveData.GetValueOrDefault(SaveData.SAVE_TIME_KEY, "0").SaveStringToDateTime();


    }





}
