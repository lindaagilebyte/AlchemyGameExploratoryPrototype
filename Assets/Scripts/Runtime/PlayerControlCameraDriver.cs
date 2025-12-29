using UnityEngine;

public class PlayerControlCameraDriver : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private PlayerControlAuthority playerControlAuthority;

    [SerializeField]
    private Transform cameraAnchorExploration;

    private void Awake()
    {
        if (targetCamera == null)
        {
            Debug.LogError($"{nameof(PlayerControlCameraDriver)}: Target Camera is not assigned.", this);
            enabled = false;
            return;
        }

        if (playerControlAuthority == null)
        {
            Debug.LogError($"{nameof(PlayerControlCameraDriver)}: PlayerControlAuthority is not assigned.", this);
            enabled = false;
            return;
        }

        if (cameraAnchorExploration == null)
        {
            Debug.LogError($"{nameof(PlayerControlCameraDriver)}: CameraAnchor_Exploration is not assigned.", this);
            enabled = false;
            return;
        }

        // TEMPORARY: Exploration is the only approved state
        if (playerControlAuthority.CurrentState == "Exploration")
        {
            SnapToExplorationAnchor();
        }
    }

    private void SnapToExplorationAnchor()
    {
        targetCamera.transform.SetPositionAndRotation(
            cameraAnchorExploration.position,
            cameraAnchorExploration.rotation
        );
    }
}
