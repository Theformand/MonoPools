using System;
using UnityEngine;


namespace MonoPools
{
    public class MonoPoolable : MonoBehaviour
    {
        protected bool isInPool;
        internal IMonoPool Pool;

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
