using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject 
{
    [Header("Player Stats")]
    public int hp=100;
    public int stamina=100;
    public int xp;
    public float attackRange;
    public int attackDamage;
    
    [Header("Room")]
    public int roomNumber=1;
}
