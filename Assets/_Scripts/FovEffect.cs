using UnityEngine;

public class FovEffect : MonoBehaviour
{
    public Rigidbody playerRB;
    public Vector2 FOV, speedRange;
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float baseFOV = FOV.x;
        float range = FOV.y - FOV.x;
        float proportion = (playerRB.linearVelocity.magnitude - speedRange.x) / (speedRange.y - speedRange.x);

        float fov = baseFOV + range * proportion;
        fov = Mathf.Clamp(fov, FOV.x, FOV.y);
        cam.fieldOfView = fov;
    }
}
