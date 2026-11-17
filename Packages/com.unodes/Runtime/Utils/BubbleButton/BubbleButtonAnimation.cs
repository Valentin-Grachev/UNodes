using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UNodes
{
    [RequireComponent(typeof(Button))]
    public class BubbleButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _targetRect;
        [SerializeField] private bool _useUnscaledTime = true;
        [SerializeField, Expandable] private BubbleButtonAnimationPreset _preset;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private AudioUnit _clickSound;
        [HideIf(nameof(UsePreset)), SerializeField] private AudioUnit _pointerEnterSound;
        [HideIf(nameof(UsePreset)), SerializeField] private AudioUnit _pointerExitSound;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private float _highlightScale;
        [HideIf(nameof(UsePreset)), SerializeField] private float _highlightDuration;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private float _clickScale;
        [HideIf(nameof(UsePreset)), SerializeField] private float _clickDuration;

        private Button _button;
        private CanvasGroup _buttonCanvasGroup;
        private Tween _pointerTween;
        private Tween _clickTween;

        private bool UsePreset => _preset != null;


        private AudioUnit ClickSound => UsePreset ? _preset.ClickSound : _clickSound;
        private AudioUnit PointerEnterSound => UsePreset ? _preset.PointerEnterSound : _pointerEnterSound;
        private AudioUnit PointerExitSound => UsePreset ? _preset.PointerExitSound : _pointerExitSound;
        private float HighlightScale => UsePreset ? _preset.HighlightScale : _highlightScale;
        private float HighlightDuration => UsePreset ? _preset.HighlightDuration : _highlightDuration;
        private float ClickScale => UsePreset ? _preset.ClickScale : _clickScale;
        private float ClickDuration => UsePreset ? _preset.ClickDuration : _clickDuration;


        private void Start()
        {
            if (TryGetComponent(out _buttonCanvasGroup))
                Debug.LogWarning($"Canvas group on button \"{name}\" already exists!");

            else _buttonCanvasGroup = gameObject.AddComponent<CanvasGroup>();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _pointerTween?.Kill();
            _clickTween?.Kill();
        }


        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_button.interactable || !_buttonCanvasGroup.interactable) return;

            PointerEnterSound?.Play();

            _pointerTween?.Kill();
            _pointerTween = _targetRect.DOScale(HighlightScale, HighlightDuration)
                .SetEase(Ease.OutFlash).SetUpdate(_useUnscaledTime);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!_button.interactable || !_buttonCanvasGroup.interactable) return;

            PointerExitSound?.Play();

            _pointerTween?.Kill();
            _pointerTween = _targetRect.DOScale(1f, HighlightDuration)
                .SetEase(Ease.OutFlash).SetUpdate(_useUnscaledTime);
        }


        private void OnButtonClick()
        {
            ClickSound?.Play();

            _buttonCanvasGroup.interactable = false;

            _pointerTween?.Kill();
            _clickTween?.Kill();

            _clickTween = DOTween.Sequence().SetUpdate(_useUnscaledTime)
                .Append(_targetRect.DOScale(ClickScale, ClickDuration / 2f).SetEase(Ease.OutQuad))
                .Append(_targetRect.DOScale(1f, ClickDuration / 2f).SetEase(Ease.InQuad))
                .OnComplete(() =>
                {
                    _buttonCanvasGroup.interactable = true;
                });
        }




    }
}
