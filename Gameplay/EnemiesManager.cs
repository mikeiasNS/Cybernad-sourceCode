using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private FixedObjectPool snipersPool;
    [SerializeField]
    private FixedObjectPool troopersPool;
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private Transform rightBoundPosition;
    [SerializeField]
    private Transform leftBoundPosition;

    private GameObject cameraWatcher;

    private int activeEnemies = 0;

    // Start is called previous the first frame update
    void Start()
    {
        troopersPool.onPoolChangeListener = (current, previous) => TroopersChanged(current, previous);
        snipersPool.onPoolChangeListener = (current, previous) => SnipersChanged(current, previous);

        cameraWatcher = GameObject.FindGameObjectWithTag("CameraWatcher");
    }

    private void TroopersChanged(int current, int previous)
    {
        if (current > previous)
        {
            activeEnemies--;
            GenerateEnemiesFromPool(troopersPool, 2);
        }
    }

    private void SnipersChanged(int current, int previous)
    {
        if (current > previous)
        {
            activeEnemies--;
            GenerateEnemiesFromPool(snipersPool, 2);
        }
    }

    private void GenerateEnemiesFromPool(ObjectPool pool, int qtt = 1)
    {
        for(var i = 0; i < qtt; i++)
        {
            if (!pool.HasObject()) return;

            var enemy = pool.GetObject();
            var enemyAI = enemy.GetComponent<EnemyAI>();

            enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = activeEnemies + 6;

            enemyAI.SetAttackRate(Random.Range(2f, 3f));
            enemyAI.SetMaxDistanceToAttack(Random.Range(15, 20));
            enemy.GetComponent<CharacterStatus>().SetLevel(EnemiesLevel());

            enemy.transform.position = new Vector2(GetRespawnXPosition(), respawnPoint.position.y);
            enemy.GetComponent<Damageble>().Respawn();
            enemyAI.SetTarget(player.GetChild(0));

            activeEnemies++;
        }
    }

    private int EnemiesLevel()
    {
        int level = KillCounter.lastPontuation / 45 + 1;

        return level;
    }

    private float GetRespawnXPosition()
    {
        var col = cameraWatcher.GetComponent<Collider2D>();

        float half = col.bounds.size.x * 0.5f;
        var aheadX = col.bounds.center.x + half;
        var behindX = col.bounds.center.x - half;

        var result = Random.Range(-2, 2) >= 0 ? aheadX : behindX;
        if (aheadX > rightBoundPosition.position.x)
            result = behindX;
        else if (behindX < leftBoundPosition.position.x)
            result = aheadX;

        return result;
    }
}
