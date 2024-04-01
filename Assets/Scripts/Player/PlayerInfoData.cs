
[System.Serializable]
public class PlayerInfoData
{
    public string playerName;
    public int playerLevel;
    public Exp playerExp;
    public int skillPoint;

    public PlayerInfoData()
    {
        playerName = "z임시 이름z";
        playerLevel = 1;
        playerExp = new Exp();
        skillPoint = 1;
    }
}
