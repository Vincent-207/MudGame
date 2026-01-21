using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    float moveSpeed, maxSpeed, turnSpeed;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.Normalize();

        rb.AddForce(toPlayer * moveSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        Debug.DrawRay(transform.position, toPlayer * moveSpeed * Time.fixedDeltaTime);
        float rot_z = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        // Quaternion toPlayerRot = Quaternion.Euler(0f, 0f, rot_z - 90);
        Quaternion toPlayerRot = Quaternion.LookRotation(toPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toPlayerRot, turnSpeed * Time.fixedDeltaTime);
    }
}
