using UnityEngine;
using UnityEngine.UI;

public class SetupWorkbenchUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button[] menuButtons = GetComponentsInChildren<Button>();
        foreach(Button button in menuButtons)
        {
            CanvasGroup menuGroup = button.transform.parent.GetComponent<CanvasGroup>();
            
        }
    }


}
