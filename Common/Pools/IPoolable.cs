interface IPoolable
{
    void SetPool(ObjectPool pool);
    void OnEntersPool();
    void OnExitPool();
}
