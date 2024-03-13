using System;

public class KillEvents
{
    public event Action<string> onKill;
    public void Kill(string killedName)
    {
        if (onKill != null)
        {
            onKill(killedName);
        }
    }
}
