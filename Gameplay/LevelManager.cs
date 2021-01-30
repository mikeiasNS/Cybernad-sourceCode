using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float lastTrooperSpawned;
    private GameObject player;

    [SerializeField]
    private ObjectPool troopersPool;
    [SerializeField]
    private float timeToSpawnTrooper;
    [SerializeField]
    private float troopersToSpawn;
    [SerializeField]
    private List<Transform> trooperSpawnPlaces;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad - lastTrooperSpawned >= timeToSpawnTrooper)
        {
            lastTrooperSpawned = Time.timeSinceLevelLoad;
            StartCoroutine(SpawnTroopers());
        }
    }

    private IEnumerator SpawnTroopers()
    {
        for(var i = 0; i < troopersToSpawn; i++)
        {
            if (troopersPool.HasObject())
            {
                var trooper = troopersPool.GetObject();
                trooper.GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
                Vector2 position = PositionToCreate(trooper.layer);

                CreateTrooper(position, trooper);
            }

            yield return 0;
        }
    }

    private Vector2 PositionToCreate(int enemyLayer)
    {
        var position = GeneratePosition();

        while (!CanCreateInPosition(position, enemyLayer))
            position = GeneratePosition();

        return position;
    }

    private Vector3 GeneratePosition()
    {
        var position = trooperSpawnPlaces[Random.Range(0, trooperSpawnPlaces.Count)].position;
        position.x += Random.Range(-2, 2);

        return position;
    }

    private bool CanCreateInPosition(Vector2 position, int enemyLayer)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.2f, 1 << enemyLayer);
        return !hit;
    }

    private void CreateTrooper(Vector2 position, GameObject trooper)
    {
        trooper.transform.position = position;

        trooper.GetComponent<EnemyAI>().SetTarget(player.transform);
    }
}
