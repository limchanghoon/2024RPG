
[System.Serializable]
public class Gold : IGetAddress
{
    public int gold;

    public Gold()
    {
        gold = 0;
    }

    public Gold(int gold)
    {
        this.gold = gold;
    }

    public string GetAddress()
    {
        return "Gold";
    }

    public override string ToString()
    {
        return $"+{gold}G";
    }
}