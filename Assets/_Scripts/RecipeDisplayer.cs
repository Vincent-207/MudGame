using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Recipe recipe;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowRecipe();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideRecipe();
    }



    void ShowRecipe()
    {
        RecipeCursorDisplay.instance.ShowRecipe(recipe);
    }

    void HideRecipe()
    {
        RecipeCursorDisplay.instance.Hide();
    }
}
