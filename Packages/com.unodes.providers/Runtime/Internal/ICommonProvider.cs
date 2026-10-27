using System;

namespace UNodes.Providers
{
    public interface ICommonProvider
    {
        public void Initialize(Action onInitialized);
    }
}


