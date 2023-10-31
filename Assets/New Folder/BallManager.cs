using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    BallVisualization[] ballPrefabs;

    [SerializeField, Min(0f)]
    float avoidSpawnRadius = 2f, startingCooldown = 4f;

    [SerializeField, Range(0.1f, 1f)]
    float cooldownPersistence = 0.96f;

    [SerializeField, Min(0f)]
    float maxSpeed = 12.5f, maxStartSpeed = 4f;

    [SerializeField, Min(0f)]
    float bounceStrength = 100f, explosionStrength = 2f;

    [SerializeField, Range(0.01f, 1f)]
    float fragmentSeparation = 0.6f;

    float cooldown, cooldownDuration;

    NativeList<BallState> states;

    List<BallVisualization> visualizations;

    UpdateBallJob updateBallJob;


    public void Initialize(Area2D worldArea)
    {
        updateBallJob = new UpdateBallJob
        {
            balls = states,
            worldArea = worldArea,
            maxSpeed = maxSpeed
        };
    }

    public void Dispose()
    {
        states.Dispose();
    }

    public void StartNewGame()
    {
        for (int i = 0; i < visualizations.Count; i++)
        {
            visualizations[i].Despawn();
        }
        visualizations.Clear();

        states.Clear(); cooldown = cooldownDuration = startingCooldown;
    }

    public JobHandle UpdateBalls(float dt)
    {
        cooldown -= dt;
        updateBallJob.dt = dt;
        return updateBallJob.Schedule(states.Length, default);
    }

    public void ResolveBalls(JobHandle dependency)
    {
        dependency.Complete();

        if (cooldown <= 0f)
        {
            cooldown += cooldownDuration;
            cooldownDuration *= cooldownPersistence;
            states.Add(new BallState
            {
                mass = BallState.masses[BallState.initialStage],
                targetRadius = BallState.radii[BallState.initialStage],
                stage = BallState.initialStage,
                type = Random.Range(0, ballPrefabs.Length),
                alive = true
            });
        }
    }

    public void UpdateVisualization(float dtExtrapolated)
    {
        for (int i = 0; i < visualizations.Count; i++)
        {
            BallState state = states[i];
            visualizations[i].UpdateVisualization(
                state.position + state.velocity * dtExtrapolated,
                Mathf.Min(state.radius + dtExtrapolated, state.targetRadius)
            );
        }
    }
}