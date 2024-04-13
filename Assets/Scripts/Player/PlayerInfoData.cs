
[System.Serializable]
public class PlayerInfoData
{
    public string playerName;
    public int playerLevel;
    public Exp playerExp;
    public int skillPoint;

    public PlayerInfoData()
    {
        playerName = "닉네임입니다";
        playerLevel = 1;
        playerExp = new Exp();
        skillPoint = 1;
    }
}
