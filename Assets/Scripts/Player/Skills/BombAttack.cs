using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : MonoBehaviour
{
    Animator _anim;
    public GameObject effect;

    [Header("Explode")]
    public float explosionRadius = 5f; // Patlama yarıçapı
    public int damage = 50; // Verilecek hasar
    public LayerMask damageLayer; // Hasar verilecek katman (örneğin, düşmanlar)

    private void Start() 
    {
        _anim = GetComponent<Animator>();
        Explosion();
    }
    void Explosion()
    {
        Invoke("Effect",1f);
        Invoke("Explode",1f);
        Destroy(gameObject,1.001f);

    }
    void Effect()
    {
        Instantiate(effect,gameObject.transform.position,Quaternion.identity);
    }
 void Explode()
    {
        // Patlama alanı içinde bulunan tüm colliderları al
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);

        // Her bir collidera hasar ver
        foreach (Collider2D hit in hitColliders)
        {
            // Düşmanların EnemyController bileşenine sahip olduğunu varsayalım
            EnemyController enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, (hit.transform.position - transform.position).normalized);
            }
        }

        // Patlama efektleri, sesler ve ardından bomba objesini yok etme
        Destroy(gameObject); // Patlama gerçekleştiğinde bombayı yok et
    }

    void OnDrawGizmosSelected()
    {
        // Patlama alanını görsel olarak göstermek için
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
