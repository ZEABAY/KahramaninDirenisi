using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    public float spawnRadius = 5f;
    public int healthDiff = 5;
    public float speedDiff = 0.2f;
    private float timeSinceLastSpawn = 0f;
    public int maxEnemyCount = 10;
    public int enemyCount = 0;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval && enemyCount < maxEnemyCount)
        {
            SpawnEnemy();
            enemyCount++;
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomSpawnPoint = spawnPoint.position + Random.insideUnitSphere * spawnRadius;
        randomSpawnPoint.y = spawnPoint.position.y;

        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint, spawnPoint.rotation);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.health += healthDiff;
            enemyScript.gold += healthDiff;
            healthDiff += 20;

        }

        NavMeshAgent navMeshAgent = newEnemy.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.speed += speedDiff;
            speedDiff += speedDiff;

        }
    }
}
