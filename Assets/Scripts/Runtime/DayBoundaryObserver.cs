using UnityEngine;

public class DayBoundaryObserver : MonoBehaviour
{
    [SerializeField] private WorldSimAuthority worldSim;

    private int lastDay = -1;
    private bool initialized = false;

    private void Awake()
    {
        // Explicit wiring preferred, but this keeps it minimal for now:
        // if not assigned in Inspector, try to find it.
        if (worldSim == null)
            worldSim = Object.FindFirstObjectByType<WorldSimAuthority>();
    }

    private void OnEnable()
    {
        if (worldSim == null)
        {
            Debug.LogError("DayBoundaryObserver: WorldSimAuthority not found.", this);
            return;
        }

        worldSim.OnHourChanged += HandleHourChanged;
    }

    private void OnDisable()
    {
        if (worldSim != null)
            worldSim.OnHourChanged -= HandleHourChanged;
    }

    private void HandleHourChanged(int day, int hour)
    {
        // First callback: establish baseline, do not treat as a boundary crossing.
        if (!initialized)
        {
            lastDay = day;
            initialized = true;
            return;
        }

        if (day == lastDay)
            return;

        lastDay = day;

        Debug.Log($"DayBoundaryObserver: Day changed → Day {day}", this);
    }
}
