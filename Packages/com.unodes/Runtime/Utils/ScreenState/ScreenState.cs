using System;
using UnityEngine;

namespace UNodes
{
    public enum Orientation { Portrait, Landscape }


    public class ScreenState
    {
        public static event Action OnResolutionChanged;
        public static event Action OnOrientationChanged;


        public static Vector2Int Resolution { get; private set; }
        public static Orientation Orientation { get; private set; }


        public ScreenState()
        {
            Resolution = new Vector2Int(Screen.width, Screen.height);
            Orientation = GetCurrentOrientation();
        }


        public static void Update()
        {
            if (Resolution.x != Screen.width || Resolution.y != Screen.height)
            {
                Resolution = new Vector2Int(Screen.width, Screen.height);

                if (Orientation != GetCurrentOrientation())
                {
                    Orientation = GetCurrentOrientation();
                    OnOrientationChanged?.Invoke();
                }

                OnResolutionChanged?.Invoke();
            }
        }

        private static Orientation GetCurrentOrientation() 
            => Screen.width > Screen.height ? Orientation.Landscape : Orientation.Portrait;

    }
}


