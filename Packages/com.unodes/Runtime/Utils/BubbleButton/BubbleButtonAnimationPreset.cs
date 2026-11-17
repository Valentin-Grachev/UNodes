using UnityEngine;

namespace UNodes
{
    [CreateAssetMenu(menuName = "UNodes/Bubble Button Preset", fileName = "BubbleButton")]
    public class BubbleButtonAnimationPreset : ScriptableObject
    {
        [field: SerializeField] public AudioUnit ClickSound { get; private set; }
        [field: SerializeField] public AudioUnit PointerEnterSound { get; private set; }
        [field: SerializeField] public AudioUnit PointerExitSound { get; private set; }


        [field: Space, SerializeField] public float HighlightScale { get; private set; }
        [field: SerializeField] public float HighlightDuration { get; private set; }

        [field: Space, SerializeField] public float ClickScale { get; private set; }
        [field: SerializeField] public float ClickDuration { get; private set; }


    }
}
