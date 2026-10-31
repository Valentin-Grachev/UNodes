using System;
using UnityEngine;

namespace UNodes.Providers.Test
{
    public class TestSocialsProvider : Socials.Provider
    {
        [SerializeField] private bool _isSupportInvite = true;
        [SerializeField] private bool _isSupportRateGame = true;
        [SerializeField] private bool _isSupportOpenUrl = true;

        public override bool IsSupportInvite => _isSupportInvite;
        public override bool IsSupportRateGame => _isSupportRateGame;
        public override bool IsSupportOpenUrl => _isSupportOpenUrl;


        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override void Invite()
        {
            
        }

        public override void OpenUrl(string url)
            => Application.OpenURL(url);

        public override void RateGame() 
        {

        }

    }
}

