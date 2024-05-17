using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D _rb;
    Animator _anim;
    Transform _light;
    PlayerStats _stats;
    Light2D lampLight;


    [Header("Player Stats")]
    public int health = 100;
    private int _staminaMultiplier=7;

    [Header("Stamina Increase")]
    public float staminaIncreaseInterval = 1.0f; // Stamina artırma aralığı (saniye)
    private float staminaTimer = 0.0f;

    [Header("Movement Settings")]
    [Range(0,20)]public float speed;
    private bool _isMoving;
    public float speedMultiplier=100f;
    public float dashSpeed=20f;
    public float dashDuration=0.2f;
    public float dashCooldown=1f; // bir stamina ayarlayıp buna göre kontrol ettir şimdilik sadece test    
    
    private bool _isDashing=false;
    private float _dashTimeLeft;
    private float _lastDashTime=-Mathf.Infinity;
    private Vector2 _lastMoveDirection; //son hareket yönünü takip etmesi için

    public Vector2 LastMoveDirection
    {
        get { return _lastMoveDirection; }
    }


        
    void Awake()
    {
        UniversalLight();
        _stats=Resources.Load<PlayerStats>("PlayerStats");
        _rb=GetComponent<Rigidbody2D>();
        _anim=GetComponent<Animator>();
        _light=GameObject.Find("Lighting").GetComponent<Transform>();
    }
    void Update()
    {
        HandleAnimations();
        HandleInput();
        LightControl();
        StaminaControl();
        UniversalLight();
    }

    void FixedUpdate()
    {

        if(_isDashing)
        {
            PerformDash();
        }
        else
        {
           Movement(); 
        }
    }
    #region Damage Control
    public void TakeDamage(int damage)
    {
        _stats.hp -= damage;
        if (_stats.hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Oyuncu ölüm işlemleri
        Debug.Log("Player Died!");
    }
    #endregion

    #region Movement
    float _fixedSpeed=100;


    void Movement()
    {
        float _validSpeed=Time.fixedDeltaTime*_fixedSpeed*speedMultiplier ;
        float hMove=Input.GetAxis("Horizontal")*_validSpeed;
        float vMove=Input.GetAxis("Vertical")*_validSpeed;
        Vector2 move=new Vector2(hMove,vMove).normalized;
        _rb.velocity=move*speed;

        if (move!=Vector2.zero)
        {
            _lastMoveDirection=move;
        }      
    }
    #endregion
    
    #region Stamina
    void StaminaControl()
    {  
        //  if(_stats.stamina!=100)
        // {
        //     _stats.stamina+=Time.deltaTime*_staminaMultiplier;
        // }

        staminaTimer += Time.deltaTime;
        if (staminaTimer >= staminaIncreaseInterval)
        {
            if (_stats.stamina < 100)
            {
                _stats.stamina += 1*_staminaMultiplier; // Stamina'yı 1 artır
            }
            staminaTimer = 0.0f; // Timer'ı sıfırla
        }
    }
    #endregion

    #region  Input
    void HandleInput() //kontrol Tuşları
    {
        if (Input.GetKeyDown(KeyCode.Space)&&CanDash())
            {
                StartDash();
            }
    }
    #endregion
    
    #region Light
    void LightControl()
    {
         if(_rb.velocity.x>0)
        {
            _light.transform.localRotation=Quaternion.Euler(0,0,90);
        }
        else if(_rb.velocity.x<0)
        {
            _light.transform.localRotation=Quaternion.Euler(0,0,-90);
        }
        else if(_rb.velocity.y>0)
        {
            _light.transform.localRotation=Quaternion.Euler(0,0,180);
        }
        else if(_rb.velocity.y<0)
        {
            _light.transform.localRotation=Quaternion.Euler(0,0,0);
        }
    }

    void UniversalLight()
    {
        // Mevcut Light 2D bileşenini bul ve ayarla
        lampLight = GetComponentInChildren<Light2D>();
        if (lampLight != null)
        {
            lampLight.shadowsEnabled = true;
        }
        else
        {
            Debug.LogError("Light2D component not found in children!");
        }

    }
    #endregion
    
    #region Dash
    
    private bool CanDash()
    {
        return Time.time>=_lastDashTime+dashCooldown;
    }

    void StartDash()
    {
        if(_stats.stamina>=40)
        {
            _isDashing=true;
            _dashTimeLeft=dashDuration;
            _lastDashTime=Time.time;

            //Dash Animasyonu tetiklenir
            _anim.SetTrigger("Dash");
            _anim.SetFloat("dashHorizontal",_lastMoveDirection.x);
            _anim.SetFloat("dashVertical", _lastMoveDirection.y);
            _stats.stamina-=30;
        }
    }
    private void PerformDash()
    {
        _rb.velocity=_lastMoveDirection*dashSpeed;
        _dashTimeLeft-=Time.fixedDeltaTime;
        if(_dashTimeLeft<=0)
            _isDashing=false;
    }

    #endregion
    
    #region Animations
    void HandleAnimations()
    {
        _anim.SetFloat("horizontal",_rb.velocity.x);
        _anim.SetFloat("vertical",_rb.velocity.y);
        if (_rb.velocity.x!=0 || _rb.velocity.y!=0)
            {
                _anim.SetBool("isWalking",true);
                _anim.SetFloat("lastHorizontal",_lastMoveDirection.x);
                _anim.SetFloat("lastVertical",_lastMoveDirection.y);
            }
            else
            {
                _anim.SetBool("isWalking",false);
            }
    }
    #endregion
}
