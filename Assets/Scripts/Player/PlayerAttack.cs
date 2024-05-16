using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    [Header("Component")]
    Animator _anim;
    PlayerController _pl;

    [Header("Attack Settings")]
    bool _canAttack = true;
    public float _coolDownTime = 1.2f;
    public float attackRange = 1.5f; // Saldırı mesafesi
    public int attackDamage = 10; // Saldırı hasarı
    public LayerMask enemyLayer; // Düşman layer'ı

    void Awake() 
    {
        _anim = GetComponent<Animator>();
        _pl = GetComponent<PlayerController>();
        if (_pl == null)
        {
            Debug.LogError("PlayerController not found!");
        }
    }

    private void Update() 
    {
        Attacks();
    }

    void Attacks()
    {
        if (_canAttack)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                AnimationTriger(0);
                StartCoroutine(AttackCooldown(_coolDownTime));
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                AnimationTriger(1);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                AnimationTriger(2);
            }
        }
    }

    void AnimationTriger(int index)
    {
        switch (index)
        {
            case 0:
                BasicAttack();
                break;
            case 1:
                _anim.SetTrigger("Attack2");
                break;
            case 2:
                _anim.SetTrigger("Attack3");
                break;

            default: break;
        }
    }

    void BasicAttack()
    {
        _canAttack = false;
        StartCoroutine(AttackCooldown(_coolDownTime));
        _anim.SetTrigger("Attack");
        _anim.SetFloat("attackHorizontal", _pl.LastMoveDirection.x);
        _anim.SetFloat("attackVertical", _pl.LastMoveDirection.y);

        Vector2 attackDirection = _pl.LastMoveDirection.normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, enemyLayer);
        
        // Raycast görselleştirme
        Debug.DrawRay(transform.position, attackDirection * attackRange, Color.red, 0.5f);
        
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector2 knockbackDirection = hit.collider.transform.position - transform.position;
                knockbackDirection.Normalize();
                enemy.TakeDamage(attackDamage, knockbackDirection);
            }
        }
    }

    IEnumerator AttackCooldown(float attackType)
    {
        yield return new WaitForSeconds(attackType);
        _canAttack = true;
    }
}