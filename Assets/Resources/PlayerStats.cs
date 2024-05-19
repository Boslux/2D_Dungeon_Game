using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject 
{
    [Header("Player Stats")]
    public int hpLimit;
    public int hp=100;
    public int stamina=100;
    public int level;
    public int xp;
    public int xpLimit;


    public void ExperienceControl()
    {
        if(xp>=xpLimit)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        level++;
        xp=0;
        xpLimit+=25;
        FindObjectOfType<LevelSystem>().ShowLevelUpOptions();
    }


    [Header("Basic Attack")]
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;

    [Header("Bomb Attack")]
    public float bombcoolDownTime=5;
    public int boomCount;
    
    [Header("Range Attack")]
    public int bulletCount;
    

    [Header("Room")]
    public int roomNumber=1;
}
