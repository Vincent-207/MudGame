using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebug : MonoBehaviour
{
    [SerializeField] InputActionReference input;
    void OnEnable()
    {
        input.action.started += DoDebug;
    }

    void DoDebug(InputAction.CallbackContext context)
    {
        Debug.Log("INput debug works!");
    }
}
