using MonoPools;

internal interface IMonoPool
{
    void Despawn(MonoPoolable poolable);
}

internal interface IPool
{
    void Despawn(Poolable poolable);
}