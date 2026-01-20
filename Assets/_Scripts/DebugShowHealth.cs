using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DebugShowHealth : MonoBehaviour
{
    [SerializeField] ResourceNode resourceNode;
    [SerializeField] TMP_Text healthText;

    void Update()
    {
        healthText.text = resourceNode.health.ToString();
    }
}
