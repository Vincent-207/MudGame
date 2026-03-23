using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCursor : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    [SerializeField] InputActionReference mouseInput;
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        Vector2 mousePos = mouseInput.action.ReadValue<Vector2>();
        rectTransform.position = mousePos + offset;
    }
}
