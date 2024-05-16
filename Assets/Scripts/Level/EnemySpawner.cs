using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab; // Spawn edilecek düşman
    public float spawnRate = 2f; // Spawn etme hızı
    public float spawnRadius = 5f; // Spawn etme yarıçapı
    private GameObject player;

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

        // Spawn pozisyonunu belirle
        Vector2 spawnPosition = (Vector2)player.transform.position + Random.insideUnitCircle * spawnRadius;

        // Düşmanı oluştur
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        // Spawn alanını görselleştirmek için
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, spawnRadius);
        }
    }
}
