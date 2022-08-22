using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpatialConnect.Wwise
{
    public interface IObjectPool<T> where T:MonoBehaviour
    {
        public Tuple<GameObject, T> Get();

        public IEnumerable<T> ActiveBehaviours { get; }
    }

    public class ObjectPool<T> : IObjectPool<T> where T:MonoBehaviour
    {
        private readonly List<Tuple<GameObject,T>> objectPool_  = new List<Tuple<GameObject,T>>();

        public IEnumerable<T> ActiveBehaviours
        {
            get
            {
                return objectPool_
                    .Where(element => element.Item1.activeSelf)
                    .Select(element => element.Item2);
            }
        }
        
        public ObjectPool(uint maxInstances, GameObject prefab, Transform parentTransform, Action<GameObject> initAction = null)
        {
            for (var i = 0; i < maxInstances; ++i)
            {
                var gameObject = UnityEngine.Object.Instantiate(prefab, parentTransform);
                initAction?.Invoke(gameObject);
                objectPool_.Add(new Tuple<GameObject, T>(gameObject, gameObject.GetComponent<T>()));
            }
        }

        public Tuple<GameObject, T> Get()
        {
            return objectPool_.FirstOrDefault(gameObject => !gameObject.Item1.activeSelf);
        }
    }
}
