using UnityEngine;

public class Placeable : MonoBehaviour
{
    public PlaceableObject placeableObject;

    void OnDestroy()
    {
        // Remove from placer.
        Debug.LogWarning("Calling!");
        Debug.Break();
        GameManager.Instance.placer.CancelPlacement();
    }
}
