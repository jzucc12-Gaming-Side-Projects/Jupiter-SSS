using UnityEngine;

namespace JZ.POOL
{
    /// <summary>
    /// <para>Component specific object pools</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class ComponentPool<T> : ObjectPool<T> where T : Component
    {
        #region //Constructor
        public ComponentPool(int poolSize, T poolObject, Transform poolContainer) : base(poolSize, poolObject, poolContainer)
        {
        }

        public ComponentPool(ComponentPool<T> reference) : base(reference)
        {
        }
        #endregion

        #region //Overrides
        protected override T AddPoolObject()
        {
            T po = GameObject.Instantiate(poolObject, poolContainer);
            pooledObjects.Add(po);
            Deactivate(po);
            return po;
        }

        protected override void Activate(T component)
        {
            component.gameObject.SetActive(true);
        }

        protected override void Deactivate(T component)
        {
            component.gameObject.SetActive(false);
        }

        protected override bool IsActive(T component)
        {
            return component.gameObject.activeInHierarchy;
        }
        #endregion
    }
}