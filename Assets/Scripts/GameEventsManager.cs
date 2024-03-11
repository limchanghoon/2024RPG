
public class GameEventsManager : MonoSingleton<GameEventsManager>
{
    //public static GameEventsManager Instance { get; private set; }

    public QuestEvents questEvents;
    public PlayerEvents playerEvents;

    private void Awake()
    {


        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
    }
}
