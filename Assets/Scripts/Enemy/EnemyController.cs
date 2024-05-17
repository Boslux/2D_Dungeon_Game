using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health = 100;
    public float speed = 2f;
    public float attackRange = 1f;
    public float knockbackForce = 5f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    [Header("Attack Settings")]
    public GameObject attackObjectPrefab; // Attack object prefab
    public float attackObjectLifetime = 2f; // Lifetime of the attack object

    [Header("Miscellaneous")]
    public bool haveAnimation;

    private Rigidbody2D _rb;
    private Transform _player;
    private Animator _anim;
    private bool _isAttacking = false;
    private float _lastAttackTime;
    private bool _isAlive = true; // To check if the enemy is alive
    private bool _isStunned = false; // To check if the enemy is stunned

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        Shadow();
    }

    void Update()
    {
        if (_player != null && _isAlive && !_isStunned)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (!_isAttacking)
            {
                if (distanceToPlayer > attackRange)
                {
                    MoveTowardsPlayer();
                }
                else if (Time.time >= _lastAttackTime + attackCooldown)
                {
                    StartCoroutine(Attack());
                }
            }
        }
        
    }
    void Shadow()
    {
        ShadowCaster2D enemyShadow = gameObject.AddComponent<ShadowCaster2D>();
        enemyShadow.selfShadows = true;
        enemyShadow.useRendererSilhouette = true;
    }

    #region Movement
    void MoveTowardsPlayer()
    {
        if (_isAlive && !_isStunned)
        {
            Vector2 direction = (_player.position - transform.position).normalized;
            _rb.velocity = direction * speed;
            if (haveAnimation)
            {
                _anim.SetFloat("xMove", direction.x);
                _anim.SetFloat("yMove", direction.y);
            }
            else
            {
                if (direction.x < 0)
                {
                    gameObject.transform.localScale = new Vector2(-1, 1); // y-axis scale 1 instead of 0
                }
                else
                {
                    gameObject.transform.localScale = new Vector2(1, 1); // y-axis scale 1 instead of 0
                }
            }
        }
    }
    #endregion

    #region Damage Control
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;
        _anim.SetTrigger("onDamage");

        // Stun ve Knockback işlemleri
        StartCoroutine(StunAndKnockback(knockbackDirection));

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _anim.SetBool("isLive", false);
        _isAlive = false;
        _rb.velocity = Vector2.zero;
        Destroy(gameObject, 1f);
    }
    #endregion

    #region Attack
    IEnumerator Attack()
    {
        if (_isAlive)
        {
            _isAttacking = true;
            _rb.velocity = Vector2.zero;

            // Attack Object Creation
            Vector2 attackPosition = (transform.position + _player.position) / 2;
            GameObject attackObject = Instantiate(attackObjectPrefab, attackPosition, Quaternion.identity);
            Destroy(attackObject, attackObjectLifetime);

            yield return new WaitForSeconds(0.5f); // Attack animation delay

            if (Vector2.Distance(transform.position, _player.position) <= attackRange)
            {
                PlayerController playerController = _player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(attackDamage);
                }
            }

            _lastAttackTime = Time.time;
            _isAttacking = false;
        }
    }
    #endregion

    #region Stun
    IEnumerator StunAndKnockback(Vector2 knockbackDirection)
    {
        if (_isAlive)
        {
            _isStunned = true;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(1); // Stun süresi

            _isStunned = false;
        }
    }
    #endregion
}
