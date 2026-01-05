using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlMovementDriver : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private PlayerControlAuthority playerControlAuthority;
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Camera viewCamera;


    [Header("Tuning")]
    [SerializeField] private float moveSpeedMetersPerSecond = 4f;

    private InputAction moveAction;

    private void Awake()
    {
        if (playerControlAuthority == null)
        {
            Debug.LogError($"{nameof(PlayerControlMovementDriver)}: PlayerControlAuthority is not assigned.", this);
            enabled = false;
            return;
        }

        if (playerRoot == null)
        {
            Debug.LogError($"{nameof(PlayerControlMovementDriver)}: Player root Transform is not assigned.", this);
            enabled = false;
            return;
        }

        if (viewCamera == null)
        {
            Debug.LogError($"{nameof(PlayerControlMovementDriver)}: View Camera is not assigned.", this);
            enabled = false;
            return;
        }

        // Minimal inline Input System setup (no .inputactions asset yet)
        moveAction = new InputAction(type: InputActionType.Value, binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
    }

private void Update()
{
    string state = playerControlAuthority.CurrentState;
    Vector2 move = moveAction.ReadValue<Vector2>();


    if (state != "Exploration")
        return;

    if (move.sqrMagnitude < 0.0001f)
        return;

    Vector3 local = new Vector3(move.x, 0f, move.y);
    if (local.sqrMagnitude > 1f) local.Normalize();

    Transform camT = viewCamera.transform;

    // Camera yaw only (ignore pitch/roll)
    Vector3 forward = camT.forward;
    forward.y = 0f;
    forward.Normalize();

    Vector3 right = camT.right;
    right.y = 0f;
    right.Normalize();

    Vector3 world = (right * local.x) + (forward * local.z);


    playerRoot.position += world * (moveSpeedMetersPerSecond * Time.deltaTime);

}

}
