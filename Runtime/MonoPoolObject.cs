using UnityEngine;
using UnityEngine.Pool;

namespace MonoPools
{
    public class MonoPoolObject : MonoBehaviour
    {
        protected ObjectPool<MonoPoolObject> pool;
        protected bool isInPool;
        public void SetPool(ObjectPool<MonoPoolObject> pool) => this.pool = pool;
        public virtual void OnCreate() { isInPool = true; }
        public virtual void OnSpawn() { isInPool = false; }
        public virtual void OnDespawn() { isInPool = true; }
    }
}