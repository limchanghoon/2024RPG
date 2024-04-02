
[System.Serializable]
public class PlayerStatData
{
    public int attackPower;
    public int plusMaxHP;
    public int criticalPer;

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

    public override string ToString()
    {
        return $"{attackPower}, {plusMaxHP}, {criticalPer}";
    }
}
