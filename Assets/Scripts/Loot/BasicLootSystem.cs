using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLootSystem : MonoBehaviour
{
    PlayerStats _playerStats;
    PlayerController _playerController;
    
    string[] _loot = { "hp", "bullet" };

    private void Awake()
    {
        _playerController=GameObject.Find("Player").GetComponent<PlayerController>();
        _playerStats = Resources.Load<PlayerStats>("PlayerStats");
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats could not be loaded from Resources!");
        }
    }

    void ListControl(string loot)
    {
        
        if (loot == "hp"&&_playerStats.hp<=100)
        {
            _playerController.LootControl(1);
        }
        else if (loot == "bullet"&&_playerStats.bulletCount<=100)
        {
            _playerController.LootControl(2);
        }
        _playerStats.xp+=10;
    }

        private void OnTriggerEnter2D(Collider2D cls) 
    {
        if(cls.gameObject.CompareTag("Player"))
        {
            ListControl(_loot[Random.Range(0, _loot.Length)]);
            Destroy(gameObject,0.1f);
        }
          
    }
}
