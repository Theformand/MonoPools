
using UnityEngine.Pool;

namespace MonoPools
{
    public class Poolable
    {
        protected bool isInPool;
        internal IPool Pool;

        public void Despawn()
        {
            if (Pool != null && !isInPool)
                Pool.Despawn(this);
        }

        public virtual void OnCreate() { isInPool = true; }
        public virtual void OnSpawn() { isInPool = false; }
        public virtual void OnDespawn() { isInPool = true; }
    }
}
