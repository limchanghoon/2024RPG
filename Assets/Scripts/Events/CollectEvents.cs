using System;

public class CollectEvents
{
    public event Action<int, int> onCollect;
    public void Collect(int collectedId, int curCount)
    {
        if (onCollect != null)
        {
            onCollect(collectedId, curCount);
        }
    }
}
