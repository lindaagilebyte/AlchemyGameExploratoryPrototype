using UnityEngine;

public class PlayerControlAuthority : MonoBehaviour
{
    [SerializeField]
    private string currentState = "Exploration";

    public string CurrentState => currentState;
}
