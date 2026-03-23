using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    InputActionReference pauseInput;
    bool isOpen;
    CanvasGroup canvasGroup;
    Manual manual;
    void OnEnable()
    {
        pauseInput.action.started += ToggleMenu;
    }

    void OnDisable()
    {
        pauseInput.action.started -= ToggleMenu;
    }
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        isOpen = true; ToggleMenu(new InputAction.CallbackContext());
        manual = GetComponentInChildren<Manual>();
    }
    
    void ToggleMenu(InputAction.CallbackContext ctx)
    {
        // Debug.Log("Toggling!");
        if(isOpen)
        {
            // Debug.Log("setting false!");
            isOpen = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible =false;
        }
        else
        {
            // Debug.Log("setting true!");
            
            isOpen = true;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            manual.UpdateDisplay();
        }
    }
}
