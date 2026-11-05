using System;

namespace UNodes.Providers.Missing
{
    public class MissingSocialsProvider : Socials.Provider
    {
        public override bool IsSupportInvite => false;
        public override bool IsSupportRateGame => false;
        public override bool IsSupportOpenUrl => false;


        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();

        public override void Invite() { }

        public override void OpenUrl(string url) { }

        public override void RateGame() { }

    }
}

