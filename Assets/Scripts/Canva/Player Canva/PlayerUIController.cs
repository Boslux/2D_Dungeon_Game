using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("Component")]
    PlayerStats stats;

    [Header("Health")]
    public Image[] healthImages;
    public Sprite[] healthSprites; // 3 adet görsel var

    [Header("Stamina")]
    public Image[] staminaImages;
    public Sprite[] staminaSprites; // 3 adet görsel var
    [Header("Stamina")]
    public Image[] bulletImages;
    public Sprite[] bulletSprites; // 3 adet görsel var

    [Header("Room")]
    public Text roomNumber;


    private void Awake() 
    {
        stats=Resources.Load<PlayerStats>("PlayerStats");
    }
    private void Update() 
    {
        UpdateUI(healthImages, healthSprites, stats.hp);
        UpdateUI(staminaImages, staminaSprites, stats.stamina);
        UpdateUI(bulletImages,bulletSprites,stats.bulletCount);
        roomNumber.text = "Room: "+stats.roomNumber.ToString();
    }

    void UpdateUI(Image[] images, Sprite[] sprites, int value)
    {
        if (value > 85)
        {
            SetSprites(images, sprites, 0, 0, 0);
        }
        else if (value > 75)
        {
            SetSprites(images, sprites, 0, 0, 1);
        }
        else if (value > 65)
        {
            SetSprites(images, sprites, 0, 0, 2);
        }
        else if (value > 45)
        {
            SetSprites(images, sprites, 0, 1, 2);
        }
        else if (value > 25)
        {
            SetSprites(images, sprites, 0, 2, 2);
        }
        else if (value > 0)
        {
            SetSprites(images, sprites, 1, 2, 2);
        }
        else
        {
            SetSprites(images, sprites, 2, 2, 2);
        }
    }

    void SetSprites(Image[] images, Sprite[] sprites, int sprite1, int sprite2, int sprite3)
    {
        images[0].sprite = sprites[sprite1];
        images[1].sprite = sprites[sprite2];
        images[2].sprite = sprites[sprite3];
    }
    
}




