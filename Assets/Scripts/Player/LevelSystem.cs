using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] optionButtons;
    private SkillController _upgradeSystem;

    [Header("Skills")]
    float _mutliplier = 1;

    private void Awake()
    {
        _upgradeSystem = GetComponent<SkillController>();
        levelUpPanel.SetActive(false); // Başlangıçta panel gizli olmalı
    }

    public void ShowLevelUpOptions()
    {
        Time.timeScale = 0f; // Oyunu durdur
        levelUpPanel.SetActive(true);

        optionButtons[0].onClick.AddListener(() => SelectOptions(0));
        optionButtons[1].onClick.AddListener(() => SelectOptions(1));
        optionButtons[2].onClick.AddListener(() => SelectOptions(2));
    }

    private void SelectOptions(int index)
    {
        ApplyUpgrade(index);

        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void ApplyUpgrade(int index)
    {
        switch (index)
        {
            case 0:
                _mutliplier -= 0.1f;
                if (_mutliplier > 0.2f)
                {
                    _upgradeSystem.canRecover = true; // Can geri kazanmayı aktif et
                    _upgradeSystem.hpRecoveryUnlocked = true; // Can geri kazanmanın kilidini aç
                    Debug.Log("Health recovery activated");
                    if(_upgradeSystem.recoveryRate !> 0.5f)
                    {
                        _upgradeSystem.recoveryRate-=0.05f;
                    }
                }
                break;

            case 1:
                int _damage = 5;
                _upgradeSystem.DamageIncrease(_damage);     //Saldırı arttır
                break;

            case 2:
                float _speed = 0.1f;
                _upgradeSystem.AttackSpeedIncrease(_speed); //Saldırı hızı arttır
                break;

            default:
                break;
        }
    }
}
