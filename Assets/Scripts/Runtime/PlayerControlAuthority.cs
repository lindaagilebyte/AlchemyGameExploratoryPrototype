using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlAuthority : MonoBehaviour
{
    [SerializeField]
    private string currentState = "Exploration";

    public string CurrentState
    {
        get => currentState;
        private set => currentState = value;
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.tKey.wasPressedThisFrame &&
            Keyboard.current.ctrlKey.isPressed &&
            Keyboard.current.altKey.isPressed &&
            Keyboard.current.shiftKey.isPressed)
        {
            ToggleTestState();
        }
    }
    private void ToggleTestState()
    {
        if (CurrentState == "Exploration")
            CurrentState = "Test State";
        else
            CurrentState = "Exploration";

        Debug.Log($"PlayerControlAuthority: CurrentState = {CurrentState}", this);
    }

}
