using System;

namespace UNodes.Project
{
    public static class PaymentHandler
    {
        public static event Action OnPurchaseApplied;

        public static void ApplyPurchaseReward(ProductKey productKey)
        {
            switch (productKey)
            {
                case ProductKey.NoAds:
                    SaveData.NoAdsIsPurchased = true;
                    break;

                default:
                    break;
            }

            OnPurchaseApplied?.Invoke();
        }

    }
}
