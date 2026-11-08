using System.Collections.Generic;
using UnityEngine;

namespace UNodes
{
    public class Pool<T> where T : Component
    {
        private T _prefab;
        private List<T> _poolObjects;
        private Transform _container;
        private System.Action<T> _objectPreprocessor;


        public Transform Container => _container;

        public Pool(T poolObject, Transform container = null, System.Action<T> objectPreprocessor = null)
        {
            _objectPreprocessor = objectPreprocessor;
            _prefab = poolObject;
            _poolObjects = new();
            _container = container;

            bool isPrefab = poolObject.gameObject.scene == null 
                || poolObject.gameObject.scene.IsValid() == false;

            if (!isPrefab)
            {
                poolObject.gameObject.SetActive(false);
                poolObject.name = poolObject.name + " [Prefab]";
            }
        }

        public void Expand(int amount)
        {
            for (int i = _poolObjects.Count; i < amount; i++)
            {
                var poolObject = Object.Instantiate(_prefab, _container);
                poolObject.gameObject.SetActive(false);
                _poolObjects.Add(poolObject);
            }
        }


        public T Get()
        {
            var poolObject = _poolObjects.Find(poolObject => !poolObject.gameObject.activeSelf);

            if (poolObject != null)
                poolObject.gameObject.SetActive(true);

            else
            {
                poolObject = Object.Instantiate(_prefab, _container);
                poolObject.name = poolObject.name.Replace("[Prefab]", string.Empty);
                poolObject.gameObject.SetActive(true);
                _poolObjects.Add(poolObject);
            }

            if (poolObject.transform.parent != _container)
                poolObject.transform.SetParent(_container);

            _objectPreprocessor?.Invoke(poolObject);

            return poolObject;
        }

        public void Clear()
        {
            for (int i = 0; i < _poolObjects.Count; i++)
                Return(_poolObjects[i]);
        }

        public void Return(T poolObject)
        {
            poolObject.gameObject.SetActive(false);

            if (_container != null)
                poolObject.transform.SetParent(_container);
        }



    }
}
