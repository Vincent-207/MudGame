using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    InputActionReference pauseInput;
    void OnEnable()
    {
        pauseInput.action.started += OpenMenu;
    }

    void OnDisable()
    {
        pauseInput.action.started -= CloseMenu;
    }

    void ToggleMenu(InputAction.CallbackContext)
    {
        
    }

    void OpenMenu(InputAction.CallbackContext ctx)
    {
        
    }

    void CloseMenu(InputAction.CallbackContext ctx)
    {
        
    }
}
