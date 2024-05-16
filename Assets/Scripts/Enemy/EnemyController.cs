using UnityEngine;

public class EnemyController : MonoBehaviour 
{
 public int health = 100;
    public float knockbackForce = 5f; // Geri itme kuvveti

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on enemy!");
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health);
        
        // Geri itme kuvveti uygula
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Düşmanın ölüm işlemleri
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }
}