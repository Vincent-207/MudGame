using UnityEditor;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    CanvasGroup[] menus;

    void Start()
    {
        menus = GetComponentsInChildren<CanvasGroup>();
        UpdateButtons();

        OpenMenu(0);
    }

    private void UpdateButtons()
    {
        
    }

    public void OpenMenu(int menuIndex)
    {
        CloseAllMenus();

        menus[menuIndex].alpha = 1f;
        menus[menuIndex].interactable = true;
    }

    private void CloseAllMenus()
    {
        for(int i = 0; i < menus.Length; i++)
        {
            CloseMenu(menus[i]);
        }
    }

    private void CloseMenu(CanvasGroup menu)
    {
        menu.alpha = 0;
        menu.interactable = false;
        menu.blocksRaycasts = false;
    }
}
