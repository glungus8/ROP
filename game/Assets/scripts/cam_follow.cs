using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{     
    public Tilemap tilemap;        
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Transform player;
    private Camera cam;
    private Vector3 minBounds; //minimum mapy
    private Vector3 maxBounds; //maximum mapy
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();

        // najde hráèe podle tagu
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        //polovicni rozmery mapy
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;


        if (tilemap != null)
        {
            Bounds localBounds = tilemap.localBounds;
            Vector3 worldPos = tilemap.transform.position;

            minBounds = localBounds.min + worldPos;
            maxBounds = localBounds.max + worldPos;
            //posuny
            minBounds += new Vector3(-0.5f, 0.5f, 0f);
            maxBounds += new Vector3(-0.5f, 0.5f, 0f);
        }
        else
        {
            minBounds = new Vector3(-10000f, -10000f, 0f);
            maxBounds = new Vector3(10000f, 10000f, 0f);
        }
    }

    void LateUpdate()
    {
        // KDYŽ HRÁÈ JEŠTÌ NENÍ, ZKUS HO NAJÍT
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
