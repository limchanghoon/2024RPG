using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class ScriptableMonsterData : ScriptableObject
{
    public int id;
    public string monsterName;

    public int monsterMaxHP;
    public int rewardExp;
}
