using System;

public class PlayerEvents
{
    public event Action onExpChanged;
    public void ChangeExp()
    {
        if (onExpChanged != null)
        {
            onExpChanged();
        }
    }

    public event Action onLevelChanged;
    public void ChangeLevel()
    {
        if (onLevelChanged != null)
        {
            onLevelChanged();
        }
    }

    public event Action onStatChanged;
    public void ChangeStat()
    {
        if (onStatChanged != null)
        {
            onStatChanged();
        }
    }
}
