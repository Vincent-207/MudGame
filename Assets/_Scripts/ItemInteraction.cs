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
        if(Physics.Raycast(ray, out hit, itemPickupRange, interactableLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if(interactable == null) return;
            interactable.Interact();
        }
    }
}
