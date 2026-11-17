using System;
using UnityEngine;


namespace UNodes
{
    public static class ViewerSettings
    {
        public static RectTransform DefaultContainer { get; private set; }

        public static void SetDefaultContainer(RectTransform container)
            => DefaultContainer = container;
    }

    public class Viewer<T> where T : Component
    {
        public delegate T PrefabLoader();

        public event Action OnShown;
        public event Action OnHidden;

        private T _prefab;
        private PrefabLoader _prefabLoader;
        private T _viewInstance; public T View => _viewInstance;

        public bool ViewExists => _viewInstance != null;

        public bool ViewIsShowing => _viewInstance != null && _viewInstance.gameObject.activeSelf;

        public Viewer()
        {
            _prefabLoader = () =>
            {
                string prefabName = typeof(T).Name.Replace("View", string.Empty);

                var view = Resources.Load<T>($"Views/{prefabName}");

                if (view != null) return view;
                else throw new Exception($"[Viewer] Resource \"Views/{prefabName}\" doesn't exist!");
            };
        }


        public T Show(RectTransform container = null)
        {
            container ??= ViewerSettings.DefaultContainer;

            bool wasShown = _viewInstance != null && _viewInstance.gameObject.activeSelf;

            _viewInstance ??= CreateViewInstance(container);
            _viewInstance.gameObject.SetActive(true);

            var rectTransform = _viewInstance.transform as RectTransform;
            rectTransform.SetParent(container, false);

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.SetAsLastSibling();

            if (!wasShown) OnShown?.Invoke();

            return _viewInstance;
        }

        public void Hide()
        {
            if (_viewInstance == null || _viewInstance.gameObject.activeSelf == false)
                return;

            _viewInstance.gameObject.SetActive(false);

            OnHidden?.Invoke();
        }


        private T CreateViewInstance(Transform container)
        {
            if (_prefab == null) _prefab = _prefabLoader.Invoke();
            return UnityEngine.Object.Instantiate(_prefab, container);
        }


    }
}


