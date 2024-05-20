using System;
using System.Collections;
using Unity.Mathematics;
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

    [Header("Bomb Attack")]
    public GameObject bombPrefab;
    bool _canBombAttack=true;

    [Header("Target Attack")]
    public GameObject markerPrefab; //İşaret prefabı
    private GameObject _currentMarker;  //Şu anki işaret
    private Transform _closestEnemy; //En yakın düşman
    public Transform closestEnemy
    {
        get { return _closestEnemy; }
    }   //en yakın düşmana erişmek için olan kodu bulletskillden de erişmemı sağlıyor.
    
    private bool _canSpecialAttack;
    public GameObject[] skills;
    private int _skillIndex=0;
    private bool _haveBullet=true;

    

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

        FindClosestEnemy();
        if (_closestEnemy != null)
        {
            MarkEnemy(_closestEnemy);
        }
    }

    void HandleAttackInput()
    {
        if (_canAttack)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                StartCoroutine(PerformBasicAttack());
            }
        }
        if(stats.bulletCount!=0)
            if (Input.GetKeyDown(KeyCode.K))
            {
                PerforRangeAttack();
            }

        if(_canBombAttack)
        {           
            if (Input.GetKeyDown(KeyCode.L))
            {   
                PerforBombAttack();
            }
        }
        
    }
    
    #region Basic Attack
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

        yield return new WaitForSeconds(stats.attackCooldown - 0.1f);
        _canAttack = true;
    }
    #endregion

    #region  Range Attack
    void PerforRangeAttack()
    {
        if(stats.bulletCount!=0)
        {        
            // burayı değiştir saldırdığı yöne büyü saldırısı gönderecek ama hangi büyü olduğunu düşen itemler belirleyecek.
            _anim.SetTrigger("Attack2");
            _anim.SetFloat("attackHorizontal", _pl.LastMoveDirection.x);
            _anim.SetFloat("attackVertical", _pl.LastMoveDirection.y);
            BulletCheck(); //şu anda tek bir saldırı var o yüzden pek işlevli değil ileride daha mantıklı olacak.
        }
    }
    void BulletCheck()
    {
        //şu anda sadece skills[0] aktif
        switch (_skillIndex)
        {
            case 0:
                PerformSpecialAttack(skills[0]);;
                
                break;
            case 1:
                PerformSpecialAttack(skills[1]);;
                break;
            case 2:
                PerformSpecialAttack(skills[2]);;
                break;
            default:
            break;
        }
    }

    public void FindClosestEnemy()
    {
        float _closestDistance=Mathf.Infinity;
        Transform _closestEnemy=null;
        GameObject[] _enemies=GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in _enemies)
        {
            float distenceToEnemy=Vector2.Distance(transform.position, enemy.transform.position);
            if (distenceToEnemy < _closestDistance)
            {
                _closestDistance=distenceToEnemy;
                _closestEnemy=enemy.transform;
            }
            this._closestEnemy=_closestEnemy;
        }
    }
    void MarkEnemy(Transform enemy)
    {
      if (_currentMarker == null)
        {
            _currentMarker = Instantiate(markerPrefab);
        }
        _currentMarker.transform.position = enemy.position + new Vector3(0, 0.2f, 0); // İşareti düşmanın üstünde konumlandırma  
    }
    void PerformSpecialAttack(GameObject skill)
    {
        if(stats.bulletCount!=0)
        {
            GameObject bullet = Instantiate(skill, transform.position, Quaternion.identity);
            BulletSkill bulletSkill = bullet.GetComponent<BulletSkill>();
            bulletSkill.target = _closestEnemy; // En yakın düşmanı hedef olarak ayarla
            stats.bulletCount-=10;    
        }
    }
    #endregion

    #region Bomb Attack
    void PerforBombAttack()
    {
        if(_canBombAttack)
        {
            Instantiate(bombPrefab,gameObject.transform.position,Quaternion.identity);
            StartCoroutine(CanBombAttack());
        }
    }
    IEnumerator CanBombAttack()
    {
        _canBombAttack=false;
        yield return new WaitForSeconds(stats.bombcoolDownTime);
        _canBombAttack=true;
    }
    #endregion
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
