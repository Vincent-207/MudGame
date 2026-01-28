using System.Collections.Generic;
using UnityEngine;

public class PreviewObjectValidChecker : MonoBehaviour
{
    [SerializeField] private LayerMask invalidLayers;
    public bool isValid {get; private set;} = true;
    private HashSet<Collider> collidingObjects = new HashSet<Collider>();
    
    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & invalidLayers) != 0)
        {
            collidingObjects.Add(other);
            isValid = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(((1 << other.gameObject.layer) & invalidLayers) != 0)
        {
            collidingObjects.Remove(other);
            isValid = collidingObjects.Count <= 0;
        }
    }
}
