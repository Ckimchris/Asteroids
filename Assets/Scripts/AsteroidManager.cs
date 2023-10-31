using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public Asteroid prefab;
    public UFO ufoPrefab;
    public float spawnDist = 12f;
    public float spawnRate = 1f;
    public int amountPerSpawn = 1;

    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SpawnRoutine()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    public void Cancel()
    {
        CancelInvoke();
    }

    public void Spawn()
    {
        for(int i = 0; i < amountPerSpawn; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDist);

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);

            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(prefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }

    public void SpawnUFO()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        float rightSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
        float topOfSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        float bottomOfSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).y;

        var xRand = Random.Range(0, 2) == 0 ? leftSideOfScreen : rightSideOfScreen;
        var yRand = Random.Range(0, 2) == 0 ? bottomOfSideOfScreen : topOfSideOfScreen;

        Vector3 spawnPoint = new Vector3(xRand, yRand, 0);

        UFO ufo = Instantiate(ufoPrefab, spawnPoint, Quaternion.identity);
    }
}
