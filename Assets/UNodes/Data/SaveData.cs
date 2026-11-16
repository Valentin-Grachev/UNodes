using System;
using System.Collections.Generic;
using UNodes.Extensions;

namespace UNodes.Data
{
    public static class SaveData
    {
        public static bool NoAdsIsPurchased;
        public static bool GameWasRated;
        public static DateTime SaveTime;

        private const string NO_ADS_IS_PURCHASED = "noAds";
        private const string GAME_WAS_RATED = "gameRated";
        public const string SAVE_TIME = "saveTime";


        public static Dictionary<string, string> GetSaveData()
        {
            var saveData = new Dictionary<string, string>
            {
                { NO_ADS_IS_PURCHASED, NoAdsIsPurchased.ToSaveString() },
                { GAME_WAS_RATED, GameWasRated.ToSaveString() },
                { SAVE_TIME, SaveTime.ToSaveString() },
            };

            return saveData;
        }

        public static void RestoreFromSaveData(Dictionary<string, string> saveData)
        {
            NoAdsIsPurchased = saveData.GetValueOrDefault(NO_ADS_IS_PURCHASED, "0")
                .SaveStringToBool();

            GameWasRated = saveData.GetValueOrDefault(GAME_WAS_RATED, "0")
                .SaveStringToBool();

            SaveTime = saveData.GetValueOrDefault(SAVE_TIME, "0")
                .SaveStringToDateTime();
        }

    }
}
