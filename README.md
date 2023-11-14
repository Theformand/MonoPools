# MonoPools
A thin wrapper around Unity ObjecPools with slightly better ergonomics

# How to use 
 Make sure your poolables derive from MonoPoolObject or PoolObject.
 
 Usage:
 ```
 var myPool = new MonoPool(myMonoPoolObjectPrefab);
 var myPoolable = myPool.Spawn();
 myPool.Despawn(myPoolable);

 MyPoolableObject : MonoPoolable
 {
  public override void OnCreate()
 {
     base.OnCreate();
 }

 public override void OnSpawn()
 {
     base.OnSpawn();
 }

 public override void OnDespawn()
 {
     base.OnDespawn();
 }
 }
```


Poolables (both monobehaviours and not) all reference their own pool, so can handle their own despawn logic. They also know whether they are in the pool or not. Make sure you call base.OnXXX methods always.
