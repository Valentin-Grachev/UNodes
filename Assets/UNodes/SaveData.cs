using System.Collections.Generic;
using UNodes.Extensions;

namespace UNodes.Project
{
    public static class SaveData
    {
        public static bool NoAdsIsPurchased = false;


        private const string NO_ADS_IS_PURCHASED = "noAds";


        public static Dictionary<string, string> GetSaveData()
        {
            var saveData = new Dictionary<string, string>
            {
                { NO_ADS_IS_PURCHASED, NoAdsIsPurchased.ToSaveString() },
            };

            return saveData;
        }

        public static void RestoreFromSaveData(Dictionary<string, string> saveData)
        {
            NoAdsIsPurchased = saveData.GetValueOrDefault(NO_ADS_IS_PURCHASED, "0")
                .SaveStringToBool();
        }


    }
}
