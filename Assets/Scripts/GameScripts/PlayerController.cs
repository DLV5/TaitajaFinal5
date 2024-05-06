using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public void Pause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        var stateToChange = 
            GameManager.Instance.CurrentState == GameState.Paused ? GameState.Playing : GameState.Paused;
        GameManager.Instance.ChangeGameState(stateToChange);
    }
}
