# MonoPools
A thin wrapper around Unity ObjecPools with slightly better ergonomics

# How to use 
 Make sure your poolables derive from MonoPoolObject or PoolObject.
 
 Usage:
 var myPool = new MonoPool(myMonoPoolObjectPrefab);
 myPool.Spawn();
 myPool.Despawn();

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
