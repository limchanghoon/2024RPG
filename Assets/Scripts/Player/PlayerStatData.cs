
[System.Serializable]
public class PlayerStatData
{
    public int attackPower;
    public int plusMaxHP;
    public int criticalPer;

    public PlayerStatData()
    {
        attackPower = 0;
        plusMaxHP = 0;
        criticalPer = 0;
    }

    public PlayerStatData(int _level)
    {
        UpdateLevel(_level);
    }

    public void UpdateLevel(int _level)
    {
        attackPower = 10 + _level;
        plusMaxHP = 100 + _level * 5;
        criticalPer = 0;
    }

    public void Add(SkillData input)
    {
        foreach(var _values in input.GetScriptableSkillData().values)
        {
            if (input.skillLevel == 0) continue;
            if (_values.skillValueType == SkillValueType.PlusAttackPower)
            {
                attackPower += _values.value[input.skillLevel - 1];
            }
            else if (_values.skillValueType == SkillValueType.PlusMaxHP)
            {
                plusMaxHP += _values.value[input.skillLevel - 1];
            }
            else if (_values.skillValueType == SkillValueType.PlusCriticalPer)
            {
                criticalPer += _values.value[input.skillLevel - 1];
            }
        }
    }

    public void Add(EquipmentItemData input)
    {
        attackPower += input.attackPower;
        plusMaxHP += input.plusMaxHP;
        criticalPer += input.criticalPer;
    }

    public void Add(PlayerStatData input)
    {
        attackPower += input.attackPower;
        plusMaxHP += input.plusMaxHP;
        criticalPer += input.criticalPer;
    }

    public void Reset()
    {
        attackPower = 0;
        plusMaxHP = 0;
        criticalPer = 0;
    }

    public void Reset(PlayerStatData input)
    {
        attackPower = input.attackPower;
        plusMaxHP = input.plusMaxHP;
        criticalPer = input.criticalPer;
    }

    public override string ToString()
    {
        return $"{attackPower}, {plusMaxHP}, {criticalPer}";
    }
}
