using Unity.Mathematics;
using UnityEngine;

using Random = UnityEngine.Random;

public class BallVisualization : MonoBehaviour
{
    PrefabInstancePool<BallVisualization> pool;

    float radius;

    public BallVisualization Spawn()
    {
        BallVisualization instance = pool.GetInstance(this);
        instance.pool = pool;
        instance.radius = -1f;
        return instance;
    }

    public void UpdateVisualization(float2 position, float targetRadius)
    {
        transform.localPosition = new Vector3(position.x, position.y);
        if (radius != targetRadius)
        {
            radius = targetRadius;
            transform.localScale = Vector3.one * (2f * targetRadius);
        }
    }

    public void Despawn() => pool.Recycle(this);
}