using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class cam_follow : MonoBehaviour
{     
    public Tilemap tilemap;        
    public float smoothSpeed = 0.125f, padding = 0.5f; // Rezerva pùl tilu
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    Transform player;
    Camera cam;
    Vector3 minBounds, maxBounds; //minimum a maximum mapy
    float halfHeight, halfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();

        GameObject p = GameObject.FindGameObjectWithTag("Player"); //najde hrace podle tagu
        if (p != null)
            player = p.transform;

        //polovicni rozmery mapy
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;


        if (tilemap != null)
        {
            tilemap.CompressBounds();
            BoundsInt cellBounds = tilemap.cellBounds;

            minBounds = tilemap.CellToWorld(cellBounds.min);
            maxBounds = tilemap.CellToWorld(cellBounds.max);

            //posun o padding
            minBounds.x -= padding;
            minBounds.y -= padding;
            maxBounds.x += padding;
            maxBounds.y += padding;
        }
        else
        {
            minBounds = new Vector3(-10000f, -10000f, 0f);
            maxBounds = new Vector3(10000f, 10000f, 0f);
        }
    }

    void LateUpdate()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;

            return;
        }

        Vector3 targetPos = player.position + offset;

        //omezeni cilove pozice aby nepresahovala hranice mapy
        float leftLimit = minBounds.x + halfWidth;
        float rightLimit = maxBounds.x - halfWidth;
        float bottomLimit = minBounds.y + halfHeight;
        float topLimit = maxBounds.y - halfHeight;

        //kdyz je mapa mensi nez obraz kamery vycentruje kameru
        if (leftLimit > rightLimit)
            targetPos.x = (minBounds.x + maxBounds.x) / 2f;
        else
            targetPos.x = Mathf.Clamp(targetPos.x, leftLimit, rightLimit);

        if (bottomLimit > topLimit)
            targetPos.y = (minBounds.y + maxBounds.y) / 2f;
        else
            targetPos.y = Mathf.Clamp(targetPos.y, bottomLimit, topLimit);

        targetPos.z = offset.z;

        //plynule prechody
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, 1f - Mathf.Pow(1f - smoothSpeed, Time.deltaTime * 60f));
        transform.position = smoothPos;
    }
}
