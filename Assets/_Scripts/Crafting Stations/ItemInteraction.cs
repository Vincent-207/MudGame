using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteraction : MonoBehaviour
{
    public InputActionReference interact;
    public float itemPickupRange;
    [SerializeField] LayerMask interactableLayerMask;
    public Camera playerCam;

    void OnEnable()
    {
        interact.action.started += TryInteract;
    }

    void OnDisable()
    {
        interact.action.started -= TryInteract;
    }
    public void TryInteract(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, playerCam.transform.forward);
        DrawDebug();
        if(Physics.Raycast(ray, out hit, itemPickupRange, interactableLayerMask))
        {
            // Debug.Log("hit!");
            // Debug.Log("Hit: " + hit.collider.name);
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable == null) return;
            interactable.Interact(hit.distance);
        }
    }

    void DrawDebug()
    {
        // Debug.Log("Interacting!");
        Debug.DrawRay(transform.position, playerCam.transform.forward * 5f, Color.red);
    }
}
