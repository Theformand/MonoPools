
using UnityEngine.Pool;

namespace MonoPools
{
    public class PoolObject
    {
        protected ObjectPool<PoolObject> pool;
        protected bool isInPool;

        public virtual void OnCreate() { isInPool = true; }

        public virtual void OnSpawn() { isInPool = false; }
        public virtual void OnDespawn() { isInPool = true; }

        internal void SetPool(ObjectPool<PoolObject> pool) => this.pool = pool;
    }
}
