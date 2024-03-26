
public class GameEventsManager : MonoSingleton<GameEventsManager>
{
    //public static GameEventsManager Instance { get; private set; }

    public QuestEvents questEvents;
    public PlayerEvents playerEvents;
    public KillEvents killEvents;
    public CollectEvents collectEvents;
    public CommunicateEvents communicateEvents;

    private void Awake()
    {
        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
        killEvents = new KillEvents();
        collectEvents = new CollectEvents();
        communicateEvents = new CommunicateEvents();
    }
}
