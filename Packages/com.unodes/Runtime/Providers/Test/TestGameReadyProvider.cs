using System;
using UnityEngine;

namespace UNodes.Providers.Test
{
    public class TestGameReadyProvider : GameReady.Provider
    {
        [SerializeField] private bool _showLogs = true;

        public override void GameplayReady()
        {
            if (_showLogs) Debug.Log("[Game Ready] Gameplay ready.");
        }

        public override void GameplayStart()
        {
            if (_showLogs) Debug.Log("[Game Ready] Gameplay start.");
        }

        public override void GameplayPause()
        {
            if (_showLogs) Debug.Log("[Game Ready] Gameplay pause.");
        }

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();
    }
}

