
[System.Serializable]
public class PlayerStat
{
    public int attackPower;
    public int plusMaxHP;

    public void Add(EquipmentItemData input)
    {
        attackPower += input.attackPower;
        plusMaxHP += input.plusMaxHP;
    }

    public void Add(PlayerStat input)
    {
        attackPower += input.attackPower;
        plusMaxHP += input.plusMaxHP;
    }

    public override string ToString()
    {
        return attackPower.ToString() + ", " + plusMaxHP.ToString();
    }
}
