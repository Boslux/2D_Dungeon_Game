using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    [Header("Component")]
    Animator _anim;
    PlayerController _pl;
    PlayerStats stats;

    [Header("Attack Settings")]
    private bool _canAttack = true;
    private float _coolDownTime = 0.8f;
    public LayerMask enemyLayer; // Düşman layer'ı

    //public float attackRange = 1.5f; // Saldırı mesafesi
    //public int attackDamage = 10; // Saldırı hasarı

    void Awake() 
    {
        stats=Resources.Load<PlayerStats>("PlayerStats");
        _anim = GetComponent<Animator>();
        _pl = GetComponent<PlayerController>();
        if (_pl == null)
        {
            Debug.LogError("PlayerController not found!");
        }
    }

    private void Update() 
    {
        HandleAttackInput();
    }

    void HandleAttackInput()
    {
        if (_canAttack)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                StartCoroutine(PerformBasicAttack());
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                PerformSpecialAttack("Attack2");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                PerformSpecialAttack("Attack3");
            }
        }
    }
    IEnumerator PerformBasicAttack()
    {
        _canAttack = false;
        _anim.SetTrigger("Attack");
        _anim.SetFloat("attackHorizontal", _pl.LastMoveDirection.x);
        _anim.SetFloat("attackVertical", _pl.LastMoveDirection.y);

        yield return new WaitForSeconds(0.1f); // Saldırı animasyonunun başlama süresi

        Vector2 attackPosition = (Vector2)transform.position + _pl.LastMoveDirection.normalized * (stats.attackRange / 2);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, stats.attackRange / 2, enemyLayer);

        // Saldırı alanı görselleştirme
        Debug.DrawLine(transform.position, attackPosition, Color.red, 0.5f);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    enemyController.TakeDamage(stats.attackDamage, knockbackDirection);
                }
            }
        }

        yield return new WaitForSeconds(_coolDownTime - 0.1f);
        _canAttack = true;
    }

    void PerformSpecialAttack(string animationTrigger)
    {
        _anim.SetTrigger(animationTrigger);
    }
    private void OnDrawGizmosSelected()
    {
        if (_pl != null)
        {
            Vector2 attackPosition = (Vector2)transform.position + _pl.LastMoveDirection.normalized * (stats.attackRange / 2);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition, stats.attackRange / 2);
        }
    }
}
