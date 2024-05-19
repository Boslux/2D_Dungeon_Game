using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject[] enemyPrefab; // Spawn edilecek düşman
    public float spawnRate = 2f; // Spawn etme hızı
    public float spawnRadius = 5f; // Spawn etme yarıçapı
    public float minSpawnDistance = 2f; // Oyuncunun yakınında spawn olmaması için minimum mesafe
    private GameObject player;
    public PlayerStats room;

    public void Initialize(GameObject player)
    {
        this.player = player;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnRate);    
        }
    }

    private void SpawnEnemy()
    {
        if (player == null) return;

        Vector2 spawnPosition;
        int attempts = 0;
        const int maxAttempts = 30;

        // Spawn pozisyonunu belirle
        do
        {
            spawnPosition = (Vector2)player.transform.position + Random.insideUnitCircle * spawnRadius;
            attempts++;
        }
        while (Vector2.Distance(spawnPosition, player.transform.position) < minSpawnDistance && attempts < maxAttempts);

        if (attempts < maxAttempts)
        {
            // Düşmanı oluştur
            Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Uygun spawn pozisyonu bulunamadı.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Spawn alanını görselleştirmek için
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, spawnRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.transform.position, minSpawnDistance);
        }
    }
}
