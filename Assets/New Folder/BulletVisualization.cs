using UnityEngine;

public class BulletVisualization : MonoBehaviour
{
    PrefabInstancePool<BulletVisualization> pool;

    public BulletVisualization Spawn()
    {
        BulletVisualization instance = pool.GetInstance(this);
        instance.pool = pool;
        return instance;
    }

    public void Despawn() => pool.Recycle(this);
}