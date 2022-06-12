using UnityEngine;

namespace JZ.POOL
{
    /// <summary>
    /// <para>Game Object specific object pool</para>
    /// </summary>
    public class GOPool : ObjectPool<GameObject>
    {
        #region //Constructor
        public GOPool(int poolSize, GameObject poolObject, Transform poolContainer) : base(poolSize, poolObject, poolContainer)
        {
        }
        #endregion

        #region //Overrides
        protected override GameObject AddPoolObject()
        {
            GameObject po = GameObject.Instantiate(poolObject, poolContainer);
            pooledObjects.Add(po);
            Deactivate(po);
            return po;
        }

        protected override void Activate(GameObject obj)
        {
            obj.SetActive(true);
        }

        protected override void Deactivate(GameObject obj)
        {
            obj.SetActive(false);
        }

        protected override bool IsActive(GameObject obj)
        {
            return obj.activeInHierarchy;
        }
        #endregion
    }
}