using System;
using System.Collections.Generic;
using UnityEngine;
using UNodes.Project;

namespace UNodes
{
    public static class Payments
    {
        public abstract class Provider : CommonProvider
        {
            public abstract bool IsSupportPayments { get; }

            public abstract string GetPriceText(ProductKey productKey);
            public abstract void Purchase(ProductKey productKey, Action<bool> onHandled);
            public abstract List<ProductKey> GetNonHandledProducts();
            public abstract void DeletePurchases(Action<bool> onHandled);
        }

        public delegate void PaymentEvent(ProductKey productKey, bool success);
        public static event PaymentEvent OnPaymentHandled;


        private static Provider _provider;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(onInitialized: () =>
            {
                _provider = provider;

                var nonHandledProducts = _provider.GetNonHandledProducts();

                if (nonHandledProducts != null)
                {
                    foreach (var productKey in _provider.GetNonHandledProducts())
                        OnPaymentHandled?.Invoke(productKey, success: true);
                }

                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;
        public static bool IsSupportPayments => _provider.IsSupportPayments;

        public static string GetPriceText(ProductKey productKey) 
            => _provider.GetPriceText(productKey);

        public static void Purchase(ProductKey productKey, Action<bool> onHandled = null)
        {
            if (_provider.IsSupportPayments)
            {
                _provider.Purchase(productKey, onHandled: (success) =>
                {
                    onHandled?.Invoke(success);
                    OnPaymentHandled?.Invoke(productKey, success);
                });
            }
            else
            {
                Debug.LogError("[Payments] Payments is not supported!");
                onHandled?.Invoke(false);
                OnPaymentHandled?.Invoke(productKey, false);
            }

            
        }


        public static void DeletePurchases(Action<bool> onHandled = null) 
            => _provider.DeletePurchases(onHandled);

    }
}



