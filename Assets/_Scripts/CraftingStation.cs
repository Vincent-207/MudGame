using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    [SerializeField] CanvasGroup stationMenu;
    [SerializeField] Inventory inventory;
    void Start()
    {
        stationMenu.transform.GetChild(1).GetComponent<Craftingmenu>().OpenMenu();

        stationMenu.alpha = 0f;
        stationMenu.interactable = false;
        stationMenu.blocksRaycasts = false;
    }
    public void Interact()
    {
        stationMenu.alpha = 1f;
        stationMenu.interactable = true;
        stationMenu.blocksRaycasts = true;
    }
}
