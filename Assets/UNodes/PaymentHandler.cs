namespace UNodes.Project
{
    public static class PaymentHandler
    {
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
        }

    }
}
