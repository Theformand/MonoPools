using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using System;

namespace MonoPools
{
    public class MonoPool
    {
        private ObjectPool<MonoPoolObject> monoPool;
        private ObjectPool<PoolObject> pool;

        //TODO: Figure out a solution for this. Dont like the ternary per call. This could be called from hot loops by the user
        public int NumActive => isMonobehaviour ? monoPool.CountActive : pool.CountActive;
        public int NumTotal => isMonobehaviour ? monoPool.CountAll : pool.CountAll;
        public int NumInactive => isMonobehaviour ? monoPool.CountInactive : pool.CountInactive;

        //in-editor, we parent things under a new transform, for neat organization. Not needed (and slightly faster without it) in builds
        private Transform editorParent;
        private bool isMonobehaviour;

        /// <summary>
        /// Use this overload for non-monobehaviour classes
        /// </summary>
        /// <param name="poolObject"></param>
        /// <param name="size"></param>
        /// <param name="prewarm"></param>
        public MonoPool(PoolObject poolObject, int size = 16, bool prewarm = true)
        {
            isMonobehaviour = false;
            pool = new ObjectPool<PoolObject>(
                () =>
                {
                    var o = (PoolObject)Activator.CreateInstance(poolObject.GetType());
                    o.SetPool(pool);
                    o.OnCreate();
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
                List<PoolObject> prewarmList = new();

                for (int i = 0; i < size; i++)
                    prewarmList.Add(pool.Get());

                for (int i = 0; i < prewarmList.Count; i++)
                    pool.Release(prewarmList[i]);
            }
        }

        public MonoPool(MonoPoolObject prefab, int size = 16, bool toggleGameObject = true, bool prewarm = true)
        {
            isMonobehaviour = true;
#if UNITY_EDITOR
            editorParent = new GameObject("POOL_" + prefab.name).transform;
#endif
            monoPool = new ObjectPool<MonoPoolObject>(
                () =>
                {
                    var o = Object.Instantiate(prefab);
                    o.SetPool(monoPool);
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

                }, null
                , false,
                size
                );

            if (prewarm)
            {
                List<MonoPoolObject> prewarmList = new();

                for (int i = 0; i < size; i++)
                    prewarmList.Add(monoPool.Get());

                for (int i = 0; i < prewarmList.Count; i++)
                    monoPool.Release(prewarmList[i]);
            }
        }

        public MonoPoolObject Spawn() => monoPool.Get();
        public void Despawn(MonoPoolObject obj) => monoPool.Release(obj);

#if UNITY_EDITOR
        private void EditorParenting(MonoPoolObject mpo, bool isDespawn)
        {
            if (isDespawn)
                mpo.transform.SetParent(editorParent);
            else
                mpo.transform.SetParent(null);
        }
#endif
    }
}