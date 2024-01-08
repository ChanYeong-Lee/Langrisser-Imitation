using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WorldMapController : MonoBehaviour
{
    public float zoomSpeed = 3;
    public float moveSpeed = 3;

    [Header("Limit")]
    public Vector2 minVector;
    public Vector2 maxVector;

    [HideInInspector] public WorldMapPlayer player;
    public bool canMove = true;
    private bool following = false;
    private void Start()
    {
        player = FindObjectOfType<WorldMapPlayer>(); 
        player.OnMove.AddListener(FollowPlayer);
    }
    void Update()
    {
        Zoom();
        MouseMove();
        KeyboardMove();
        CheckNode();
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 Dest = transform.position + transform.forward * scroll * zoomSpeed;
        Dest.z = Mathf.Clamp(Dest.z, minVector.y, maxVector.y);
        if (Dest.y < 6 && 2 < Dest.y)
        {
            transform.position = Dest;
        }
    }

    private bool isDragging = false;
    private Vector3 offset = Vector3.zero;
    private Vector3 mouseOrigin = Vector3.zero;
    private Vector3 target = Vector3.zero;

    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            isDragging = true;
            following = false;
            mouseOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            offset = GetTarget();
        }
        if (isDragging)
        {
            target = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 delta = (mouseOrigin - target);
            delta.z = delta.y * 9/16;
            delta.y = 0;
            Follow(delta * 5 * (transform.position.y / 2) + offset);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void KeyboardMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);
        dir = dir.normalized * Time.deltaTime * moveSpeed;
        Move(dir);
    }

    private void Move(Vector2 dir)
    {
        float nextX = transform.position.x + dir.x;
        float nextZ = transform.position.z + dir.y;
        if (nextX < minVector.x || nextZ < minVector.y)
            return;
        if (maxVector.x < nextX || maxVector.y < nextZ)
            return;
        transform.Translate(new Vector3(dir.x, 0, dir.y), Space.World);
    }

    private void Follow(Vector3 pos)
    {
        float x = pos.x;
        x = Mathf.Clamp(x, minVector.x, maxVector.x);
        float z = pos.z - transform.position.y / Mathf.Tan(Mathf.Deg2Rad * 50);
        z = Mathf.Clamp(z, minVector.y, maxVector.y);   
        transform.position = new Vector3(x, transform.position.y, z);
    }

    private IEnumerator FollowPlayerCoroutine()
    {
        following = true;
        while (following && player.isMoving)
        {
            Follow(player.transform.position);
            yield return null;
        }
        following = false;
    }

    private void FollowPlayer()
    {
        StartCoroutine(FollowPlayerCoroutine());
    }

    private Vector3 GetTarget()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            return hit.point;
        else
            return Vector3.zero;
    }

    private void CheckNode()
    {
        if (Input.GetMouseButtonUp(0) && canMove)
        {
            if (ClickHitInfo(out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out Node node))
                {
                    print("You Click Node");
                    if (AStarAlgorithm.PathFinding(player.currentNode, node, out List<Node> path))
                    {
                        if (path.Count > 0)
                        {
                            player.Move(path);
                        }
                    }
                    else
                    {
                        print("Can't Reach");
                    }
                }
            }
        }
    }
    private bool ClickHitInfo(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            hit = hitInfo;
            return true;
        }
        else
        {
            hit = default(RaycastHit);
            return false;
        }
    }

}
