
[System.Serializable]
public class PlayerInfoData
{
    public string playerName;
    public int playerLevel;
    public Exp playerExp;

    public PlayerInfoData()
    {
        playerName = "z�ӽ� �̸�z";
        playerLevel = 1;
        playerExp = new Exp();
    }
}
