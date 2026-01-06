using UnityEngine;

public class DayNightLightDriver : MonoBehaviour
{
    [SerializeField] private WorldSimAuthority worldSim;
    [SerializeField] private Light sunLight;

    [Header("Behavior")]
    [SerializeField] private bool setRotation = true;
    [SerializeField] private bool setIntensity = true;

    [Header("Rotation")]
    // 0:00 = lowest point, 12:00 = highest point (simple model)
    [SerializeField] private float noonElevationDeg = 60f;
    [SerializeField] private float midnightElevationDeg = -30f;
    [SerializeField] private float fixedYawDeg = 20f; // pick any stable yaw

    [Header("Band Profiles (Intensity)")]
    [SerializeField] private float dayIntensity = 1.0f;      // 07–18
    [SerializeField] private float eveningIntensity = 0.5f;  // 18–21
    [SerializeField] private float nightIntensity = 0.05f;   // 21–07


    private bool initialized = false;

    private void Awake()
    {
        if (worldSim == null)
            worldSim = Object.FindFirstObjectByType<WorldSimAuthority>();
    }

    private void OnEnable()
    {
        if (worldSim == null)
        {
            Debug.LogError("DayNightLightDriver: WorldSimAuthority not found.", this);
            return;
        }
        if (sunLight == null)
        {
            Debug.LogError("DayNightLightDriver: sunLight not assigned.", this);
            return;
        }

        worldSim.OnHourChanged += HandleHourChanged;

        // Initialize once from current clock (no “event” semantics).
        ApplyFromClock(worldSim.InGameHour);
        initialized = true;
    }

    private void OnDisable()
    {
        if (worldSim != null)
            worldSim.OnHourChanged -= HandleHourChanged;
    }

    private void HandleHourChanged(int day, int hour)
    {
        // No special casing needed; this is presentation-only.
        ApplyFromClock(hour);
    }

    private void ApplyFromClock(int hour)
    {
        if (setRotation)
        {
            float t = hour / 24f; // 0..1
            // Map 0:00..24:00 to a full cycle of elevation, peaking at noon.
            float elevation = Mathf.Lerp(midnightElevationDeg, noonElevationDeg,
                Mathf.Clamp01(Mathf.Sin(t * Mathf.PI)));

            sunLight.transform.rotation = Quaternion.Euler(elevation, fixedYawDeg, 0f);
        }
        if (setIntensity)
        {
            sunLight.intensity = GetBandIntensity(hour);
        }

    }
    private float GetBandIntensity(int hour)
    {
        if (hour >= 7 && hour < 18)
            return dayIntensity;

        if (hour >= 18 && hour < 21)
            return eveningIntensity;

        return nightIntensity; // 21–07
    }

}
