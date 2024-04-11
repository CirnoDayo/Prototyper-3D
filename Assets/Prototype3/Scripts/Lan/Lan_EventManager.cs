
using System;

public static class Lan_EventManager
{
    public static event Action OnEnemySpawned;
    public static event Action OnEnemyDestroyed;
    public static event Action UpdateSpawnedPoint; 

    public static void EnemySpawned()
    {
        OnEnemySpawned?.Invoke();
    }

    public static void EnemyDestroyed()
    {
        OnEnemyDestroyed?.Invoke();
    }

    public static void NewSpawnedPoint()
    {
        UpdateSpawnedPoint?.Invoke();
    }
}


