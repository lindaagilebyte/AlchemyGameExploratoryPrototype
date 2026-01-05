using UnityEngine;

public class WorldSimAuthority : MonoBehaviour
{
    // Single instance guard (prevents double-ticking across scene loads)
    private static WorldSimAuthority instance;

    [Header("World State (optional for now)")]
    [SerializeField] private string currentState = "Exploration";

    [Header("Time (owned here)")]
    [SerializeField] private bool advanceTime = true;
    [SerializeField] private float timeScale = 1f;

    // Future exception hook: if currentState matches any of these, time will not advance.
    // Leave empty to make time advance regardless of state (your current intent).
    [SerializeField] private string[] pauseStates;

    [SerializeField] private double worldTimeSeconds;
    public double WorldTimeSeconds => worldTimeSeconds;

    [Header("In-Game Clock")]
    [SerializeField] private int startHour = 7;
    [SerializeField] private int startMinute = 0;

    public int InGameDay { get; private set; }
    public int InGameHour { get; private set; }
    public int InGameMinute { get; private set; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void Update()
    {
        if (!ShouldAdvanceTime())
            return;

        worldTimeSeconds += (double)(Time.deltaTime * timeScale);
        UpdateInGameClock();
    }

    private bool ShouldAdvanceTime()
    {
        if (!advanceTime)
            return false;

        // Default behavior: advance regardless of state.
        if (pauseStates == null || pauseStates.Length == 0)
            return true;

        // Exception states (easy to change later)
        for (int i = 0; i < pauseStates.Length; i++)
        {
            if (pauseStates[i] == currentState)
                return false;
        }

        return true;
    }
    private void UpdateInGameClock()
    {
        // 1 real second = 1 in-game minute
        int totalInGameMinutes =
            (int)System.Math.Floor(worldTimeSeconds) +
            (startHour * 60) +
            startMinute;

        InGameDay = totalInGameMinutes / (24 * 60);

        int minutesIntoDay = totalInGameMinutes % (24 * 60);
        InGameHour = minutesIntoDay / 60;
        InGameMinute = minutesIntoDay % 60;
        // Debug.Log($"Clock: Day {InGameDay}, {InGameHour:D2}:{InGameMinute:D2}", this);
    }

}
