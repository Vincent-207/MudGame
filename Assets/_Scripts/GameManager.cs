using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Inventory inventory;
    public Transform player;
    public Canvas canvas;
    public ObjectPlacer placer;
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
