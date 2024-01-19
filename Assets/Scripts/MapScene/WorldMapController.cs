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
    Vector3 initPos;
    Quaternion initRot;
    
    public void SetPlayer(WorldMapPlayer player)
    {
        this.player = player;
    }
    public void Init()
    {
        //initPos = new Vector3(2, 2.5f, -1);
        //initRot = Quaternion.Euler(50, 0, 0);
        //ResetPosition();
        player.OnMove.AddListener(FollowPlayer);
        Follow(player.transform.position);
    }
    public void ResetPosition()
    {
        transform.position = initPos;
        transform.rotation = initRot;
    }
    void Update()
    {
                Zoom();
                MouseMove();
                KeyboardMove();
                Limit();
                CheckNode();
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 Dest = transform.position + transform.forward * scroll * zoomSpeed;
        if (Dest.y < 2 || 8 < Dest.y)
        {
            return;
        }
        transform.position = Dest;
    }

    private bool isDragging = false;
    private Vector3 mouseOrigin;
    private Vector3 target;

    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                mouseOrigin = hit.point;
                isDragging = true;
                following = false;
            }
        }
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;
            Vector3 delta = (mouseOrigin - target);
                delta.y = 0;
                transform.position = transform.position + delta;
            }
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
        transform.Translate(new Vector3(dir.x, 0, dir.y), Space.World);
    }
    private void Limit()
    {
        float x = Mathf.Clamp(transform.position.x, minVector.x, maxVector.y);
        float z = Mathf.Clamp(transform.position.z, minVector.y, maxVector.y);  
        transform.position = new Vector3(x, transform.position.y, z);
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

    private void CheckNode()
    {
        if (Input.GetMouseButtonUp(0) && canMove)
        {
            if (ClickHitInfo(out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out WorldMapNode node))
                {
                    print("You Click Node");
                    if (WorldMapAStartAlgorithm.PathFinding(player.currentNode, node, out List<WorldMapNode> path))
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
