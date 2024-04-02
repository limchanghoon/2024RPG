[System.Serializable]
public class Exp : IGetAddress
{
    public int exp;

    public Exp()
    {
        exp = 0;
    }

    public Exp(int exp)
    {
        this.exp = exp;
    }

    public string GetAddress()
    {
        return "Exp";
    }

    public override string ToString()
    {
        return $"+{exp}";
    }
}
