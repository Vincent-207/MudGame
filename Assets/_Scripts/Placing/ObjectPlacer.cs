using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ObjectPlacer : MonoBehaviour
{
    [Header("Placement Parameters")]
    [SerializeField] private GameObject placeableObjectPrefab, previewObjectPrefab;
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask placementSurfaceLayerMask;

    [Header("Preview Material")]
    [SerializeField] private Material previewMaterial;
    [SerializeField] Color validColor, invalidColor;

    [Header("Raycast Parameters")]
    [SerializeField] private float objectDistanceFromPlayer;
    [SerializeField] private float raycastStartVerticalOffset;
    [SerializeField] private float raycastDistance;
    
    
    
    [SerializeField] private GameObject _previewObject = null;
    private Vector3 _currentPlacementPosition = Vector3.zero;
    [SerializeField] [Header("Debug info")]
    private bool _inPlacement = false,  _validPreviewState = false;
    public UnityEvent PlacedObject = new UnityEvent();

    public InputActionReference placeInput;
    void OnEnable()
    {
        placeInput.action.started += PlaceObject;
    }

    void Start()
    {
        // EnterPlacementMode();
    }
    void Update()
    {
        if(_inPlacement)
        {
            UpdateCurrentPlacementPosition();

            if(CanPlaceObject())
                SetValidPreviewState();
            else   
                SetInvalidPreviewState();
        }
    }
    private void PlaceObject(InputAction.CallbackContext callbackContext)
    {
        
        if(!_inPlacement || !_validPreviewState) 
        {
            return;
        }
        
        Quaternion rotation = Quaternion.Euler(0f, playerCam.transform.eulerAngles.y, 0f);

        Instantiate(placeableObjectPrefab, _currentPlacementPosition, rotation, transform);
        ExitPlacementMode();
    }
    private void SetValidPreviewState()
    {
        previewMaterial.color = validColor;
        _validPreviewState = true;
    }

    private void SetInvalidPreviewState()
    {
        previewMaterial.color = invalidColor;
        _validPreviewState = false;
    }
    private bool CanPlaceObject()
    {
        if(_previewObject == null) return false;

        return _previewObject.GetComponentInChildren<PreviewObjectValidChecker>().isValid;

    }
    private void UpdateCurrentPlacementPosition()
    {
        Vector3 camForward = new Vector3(playerCam.transform.forward.x, 0f, playerCam.transform.forward.z);
        camForward.Normalize();
        Vector3 startPos =playerCam.transform.position + ( camForward * objectDistanceFromPlayer);
        startPos.y += raycastStartVerticalOffset;
        RaycastHit hitInfo;
        if(Physics.Raycast(startPos, Vector3.down, out hitInfo, raycastDistance, placementSurfaceLayerMask))
        {
            _currentPlacementPosition = hitInfo.point;
        }
        Quaternion rotation = Quaternion.Euler(0f, playerCam.transform.eulerAngles.y, 0f);
        _previewObject.transform.position = _currentPlacementPosition;
        _previewObject.transform.rotation = rotation;
    }
    private void EnterPlacementMode()
    {
        if(_inPlacement) return;
        Quaternion rotation = Quaternion.Euler(0f, playerCam.transform.eulerAngles.y, 0f);
        _previewObject = Instantiate(previewObjectPrefab, _currentPlacementPosition, rotation, transform);
        _inPlacement = true;
    }

    private void ExitPlacementMode()
    {
        Destroy(_previewObject);
        PlacedObject.Invoke();
        PlacedObject.RemoveAllListeners();
        _previewObject = null;
        
        _inPlacement = false;
    }

    public void CancelPlacement()
    {
        Destroy(_previewObject);
        PlacedObject.RemoveAllListeners();
        _previewObject = null;
        
        _inPlacement = false;
    }

    public void LoadObject(GameObject placeableObjectPrefab, GameObject previewObjectPrefab)
    {
        PlacedObject.RemoveAllListeners();

        Debug.Log("loading!");
        this.placeableObjectPrefab = placeableObjectPrefab;
        this.previewObjectPrefab = previewObjectPrefab;
        
        EnterPlacementMode();
    }
}
