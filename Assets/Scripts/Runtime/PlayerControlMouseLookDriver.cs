using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlMouseLookDriver : MonoBehaviour
{
    [SerializeField] private PlayerControlAuthority playerControlAuthority;
    [SerializeField] private Transform playerYawRoot; // Player
    [SerializeField] private Camera targetCamera;

    [SerializeField] private float yawDegreesPerMouseUnit = 0.1f;
    [SerializeField] private float pitchDegreesPerMouseUnit = 0.1f;
    [SerializeField] private float minPitchDegrees = -80f;
    [SerializeField] private float maxPitchDegrees = 80f;

    private InputAction lookAction;
    private InputAction lookHoldAction; // RMB hold
    private float pitchDegrees;

    private void Awake()
    {
        if (playerControlAuthority == null)
        {
            Debug.LogError($"{nameof(PlayerControlMouseLookDriver)}: PlayerControlAuthority is not assigned.", this);
            enabled = false;
            return;
        }

        if (playerYawRoot == null)
        {
            Debug.LogError($"{nameof(PlayerControlMouseLookDriver)}: PlayerYawRoot is not assigned.", this);
            enabled = false;
            return;
        }

        if (targetCamera == null)
        {
            Debug.LogError($"{nameof(PlayerControlMouseLookDriver)}: Target Camera is not assigned.", this);
            enabled = false;
            return;
        }

        // Mouse delta (New Input System)
        lookAction = new InputAction(name: "Look", type: InputActionType.Value, expectedControlType: "Vector2");
        lookAction.AddBinding("<Mouse>/delta");

        // Hold RMB to enable look
        lookHoldAction = new InputAction(name: "LookHold", type: InputActionType.Button);
        lookHoldAction.AddBinding("<Mouse>/rightButton");

        // Initialize pitch from current camera local rotation
        pitchDegrees = NormalizePitch(targetCamera.transform.localEulerAngles.x);
    }

    private void OnEnable()
    {
        lookAction.Enable();
        lookHoldAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
        lookHoldAction.Disable();

        // Ensure cursor returns if this component disables while locked
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (playerControlAuthority.CurrentState != "Exploration")
            return;

        bool holding = lookHoldAction.IsPressed();
        if (!holding)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector2 delta = lookAction.ReadValue<Vector2>();
        if (delta.sqrMagnitude < 0.0001f)
            return;

        // Yaw on Player
        float yaw = delta.x * yawDegreesPerMouseUnit;
        playerYawRoot.Rotate(0f, yaw, 0f, Space.Self);

        // Pitch on Camera (local)
        pitchDegrees -= delta.y * pitchDegreesPerMouseUnit;
        pitchDegrees = Mathf.Clamp(pitchDegrees, minPitchDegrees, maxPitchDegrees);

        Vector3 euler = targetCamera.transform.localEulerAngles;
        euler.x = pitchDegrees;
        targetCamera.transform.localEulerAngles = euler;
    }

    private static float NormalizePitch(float rawXDegrees)
    {
        if (rawXDegrees > 180f) rawXDegrees -= 360f;
        return rawXDegrees;
    }
}
