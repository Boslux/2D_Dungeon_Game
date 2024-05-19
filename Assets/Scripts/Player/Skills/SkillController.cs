using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    PlayerStats _playerStats;
    float _temporaryRecovery = 0;
    public bool hpRecoveryUnlocked;
    public bool canRecover;
    public float recoveryRate = 1f; // Can geri kazanma süresi (saniye)
    public float healthRecoveryAmount = 1f; // Her seferde geri kazanılacak can miktarı (float olarak tutulur)

    private void Awake()
    {
        _playerStats = Resources.Load<PlayerStats>("PlayerStats");
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats could not be loaded from Resources!");
        }
    }

    private void Update()
    {
        if (canRecover && hpRecoveryUnlocked)
        {
            RecoveryControl();
        }
    }

    void RecoveryControl()
    {
        _temporaryRecovery += Time.deltaTime;
        if (_temporaryRecovery >= recoveryRate)
        {
            HealthRecovery(healthRecoveryAmount);
            _temporaryRecovery = 0;
        }
    }

    public void HealthRecovery(float healthAmount)
    {
        int intHealthAmount = Mathf.FloorToInt(healthAmount); // Float değeri tam sayıya çevirir
        _playerStats.hp += intHealthAmount;
        Debug.Log("Recovered " + intHealthAmount + " HP. Current HP: " + _playerStats.hp);
    }

    public void DamageIncrease(int damageIncreaseAmount)
    {
        _playerStats.attackDamage += damageIncreaseAmount;
        Debug.Log("Increased damage by " + damageIncreaseAmount + ". Current Damage: " + _playerStats.attackDamage);
    }

    public void AttackSpeedIncrease(float attackSpeedMultiplier)
    {
        _playerStats.attackCooldown -= attackSpeedMultiplier;
        Debug.Log("Increased attack speed. Current Cooldown: " + _playerStats.attackCooldown);
    }
}
