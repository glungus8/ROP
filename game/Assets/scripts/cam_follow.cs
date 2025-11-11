using UnityEngine;

public class cam_follow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 position = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, position, smoothSpeed);
        transform.position = smoothPosition;
    }
}
