using System;

public class KillEvents
{
    public event Action<int> onKill;
    public void Kill(int killedId)
    {
        if (onKill != null)
        {
            onKill(killedId);
        }
    }
}
