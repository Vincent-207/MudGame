using UnityEngine;

[CreateAssetMenu(fileName = "Placeableobject", menuName = "Scriptable Objects/Placeableobject")]
public class PlaceableObject : ScriptableObject
{
    public GameObject placeablePrefab, previewPrefab;
}
