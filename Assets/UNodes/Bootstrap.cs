using System;


namespace UNodes.Project
{
    public class Bootstrap : BaseBootstrap
    {
        public override void Run(Action onFinished)
        {
            var test = S<TestController>.Register(new());

            onFinished?.Invoke();
        }
    }
}


