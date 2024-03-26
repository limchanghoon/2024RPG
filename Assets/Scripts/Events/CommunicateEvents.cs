using System;

public class CommunicateEvents
{
    public event Action<int> onCommunicate;
    public void Communicate(int communicatedId)
    {
        if (onCommunicate != null)
        {
            onCommunicate(communicatedId);
        }
    }
}
