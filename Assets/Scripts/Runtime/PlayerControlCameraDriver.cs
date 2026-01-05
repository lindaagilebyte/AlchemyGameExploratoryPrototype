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
    }

    private void LateUpdate()
    {
        if (playerControlAuthority.CurrentState != "Exploration")
            return;

        targetCamera.transform.position = cameraAnchorExploration.position;
    }
}
