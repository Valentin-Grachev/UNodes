using System;
using System.Collections.Generic;
using UnityEngine;
using UNodes.Project;


namespace UNodes.Providers.Test
{
    public class TestPaymentsProvider : Payments.Provider
    {
        [SerializeField] private bool _isSupportPayments = true;
        [SerializeField] private bool _simulatePaymentsError = false;
        [SerializeField] private List<ProductKey> _simulateNonHandledProducts;

        public override bool IsSupportPayments => _isSupportPayments;

        public override void DeletePurchases(Action<bool> onHandled)
            => onHandled?.Invoke(true);

        public override List<ProductKey> GetNonHandledProducts()
            => _simulateNonHandledProducts;

        public override string GetPriceText(ProductKey productKey)
            => $"${productKey}";

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override void Purchase(ProductKey productKey, Action<bool> onHandled)
        {
            if (!_simulatePaymentsError)
                onHandled?.Invoke(true);

            else
            {
                Debug.Log("[Payments] Simulating payment error.");
                onHandled?.Invoke(false);
            }
        }


    }
}

