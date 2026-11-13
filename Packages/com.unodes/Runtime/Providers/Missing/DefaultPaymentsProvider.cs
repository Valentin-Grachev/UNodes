using System;
using System.Collections.Generic;
using UNodes.Data;


namespace UNodes.Providers.Default
{
    public class DefaultPaymentsProvider : Payments.Provider
    {
        public override bool IsSupportPayments => false;

        public override void DeletePurchases(Action<bool> onHandled)
            => onHandled?.Invoke(true);

        public override List<ProductKey> GetNonHandledProducts()
            => new List<ProductKey>();

        public override string GetPriceText(ProductKey productKey)
            => $"Missing";

        public override void HandleSuccessReward(ProductKey productKey) { }

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override void Purchase(ProductKey productKey, Action<bool> onHandled)
            => onHandled?.Invoke(false);


    }
}

