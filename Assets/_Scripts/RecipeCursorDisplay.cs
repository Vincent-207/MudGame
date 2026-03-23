using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class RecipeCursorDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject displayedRecipePrefab;
    CanvasGroup canvasGroup;
    public static RecipeCursorDisplay instance;
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }
    public void ShowRecipe(Recipe recipe)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        foreach(Ingredient ingredient in recipe.ingredients)
        {
            GameObject display = Instantiate(displayedRecipePrefab, transform);
            display.GetComponent<Image>().sprite = ingredient.item.Icon;
            display.GetComponentInChildren<TMP_Text>().text = "x" + ingredient.amount;
        }
    }

    public void Hide()
    {
        for(int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            Destroy(transform.GetChild(childIndex).gameObject);
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

    }
}
