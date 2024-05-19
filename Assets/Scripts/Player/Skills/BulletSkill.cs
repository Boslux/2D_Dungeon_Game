using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill : MonoBehaviour
{
    public float speed = 5f; // Merminin hareket hızı
    public Transform target; // Hedef düşman
    private Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // En yakın düşmanı bul ve hedef olarak ayarla
        target = FindClosestEnemy();

        if (target != null)
        {
            // Mermiyi hedefe yönlendir
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            // Hedef bulunamazsa mermiyi yok et
            Destroy(gameObject);
        }
    }

    Transform FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Düşmana çarptığında hasar ver ve mermiyi yok et
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(10, (collision.transform.position - transform.position).normalized); // Örneğin 10 hasar
            }
            Destroy(gameObject);
        }
    }
}
