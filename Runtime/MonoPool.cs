using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using System;

namespace MonoPools
{
    public class MonoPool<T> : IMonoPool where T : MonoPoolable
    {
        private readonly ObjectPool<T> pool;
        public int NumActive => pool.CountActive;
        public int NumTotal => pool.CountAll;
        public int NumInactive => pool.CountInactive;

        //In-editor, we parent things under a new transform, for neat organization
        //Not needed (and slightly faster without it) in builds
        private readonly Transform editorParent;

        public MonoPool(T prefab, int size = 16, bool toggleGameObject = true, bool prewarm = true)
        {
#if UNITY_EDITOR
            editorParent = new GameObject("POOL_" + prefab.name).transform;
#endif
            pool = new ObjectPool<T>(
                () =>
                {
                    var o = Object.Instantiate(prefab);
                    o.Pool = this;
                    o.OnCreate();
                    return o;
                },
                (o) =>
                {
#if UNITY_EDITOR
                    EditorParenting(o, false);
#endif
                    if (toggleGameObject)
                        o.gameObject.SetActive(true);

                    o.OnSpawn();
                },
                (o) =>
                {
#if UNITY_EDITOR
                    EditorParenting(o, true);
#endif
                    o.OnDespawn();
                    if (toggleGameObject)
                        o.gameObject.SetActive(false);

                },
                null,
                false,
                size
                );

            if (prewarm)
            {
                List<T> prewarmList = new();

                for (int i = 0; i < size; i++)
                    prewarmList.Add(pool.Get());

                for (int i = 0; i < prewarmList.Count; i++)
                    pool.Release(prewarmList[i]);
            }
        }

        void IMonoPool.Despawn(MonoPoolable poolable)
        {
            pool.Release((T)poolable);
        }

        public T Spawn() => pool.Get();

        public void Despawn(T obj) => pool.Release(obj);

#if UNITY_EDITOR
        private void EditorParenting(MonoPoolable mpo, bool isDespawn)
        {
            if (isDespawn)
                mpo.transform.SetParent(editorParent);
            else
                mpo.transform.SetParent(null);
        }
#endif
    }

    public class Pool<T> : IPool where T : Poolable
    {
        private readonly ObjectPool<T> pool;
        public int NumActive => pool.CountActive;
        public int NumTotal => pool.CountAll;
        public int NumInactive => pool.CountInactive;

        public Pool(T poolObject, int size = 16, bool prewarm = true)
        {
            pool = new ObjectPool<T>(
                () =>
                {
                    var o = (T)Activator.CreateInstance(poolObject.GetType());
                    o.OnCreate();
                    o.Pool = this;
                    return o;
                },
                (o) => o.OnSpawn(),
                (o) => o.OnDespawn(),
                null,
                false,
                size
                );

            if (prewarm)
            {
                List<T> prewarmList = new();

                for (int i = 0; i < size; i++)
                    prewarmList.Add(pool.Get());

                for (int i = 0; i < prewarmList.Count; i++)
                    pool.Release(prewarmList[i]);
            }
        }

        public T Spawn() => pool.Get();

        public void Despawn(T obj) => pool.Release(obj);

        void IPool.Despawn(Poolable poolable)
        {
            pool.Release((T)poolable);
        }
    }
}