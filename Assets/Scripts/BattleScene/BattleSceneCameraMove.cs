using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSceneCameraMove : MonoBehaviour
{

    private float zoomSpeed = 1;
    private float moveSpeed = 3;
    private BattleMap currentBattleMap;
    private Camera theCamera;
    private bool prepared = false;
    public bool canMove = true;
    private void Awake()
    {
        theCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (prepared && canMove)
        {
            Zoom();
            KeyboardMove();
            MouseMove();
            Limit();
        }
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        theCamera.orthographicSize = theCamera.orthographicSize - scroll * zoomSpeed;
    }
    private void KeyboardMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, y, 0);
        dir.Normalize();
        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    private void Limit()
    {
        float cameraSize = Mathf.Clamp(theCamera.orthographicSize, 3f, 5f);
        theCamera.orthographicSize = cameraSize; 

        float boundX = theCamera.orthographicSize * theCamera.aspect;
        float boundY = theCamera.orthographicSize;
        float camX = Mathf.Clamp(transform.position.x, currentBattleMap.mapMinBounds.x + boundX, currentBattleMap.mapMaxBounds.x - boundX); 
        float camY = Mathf.Clamp(transform.position.y, currentBattleMap.mapMinBounds.y + boundY, currentBattleMap.mapMaxBounds.y - boundY);
        Vector3 camPos = new Vector3(camX, camY, transform.position.z);
        transform.position = camPos;
    }

    public void SetBattleMap(BattleMap battleMap)
    {
        currentBattleMap = battleMap;
        transform.position = battleMap.transform.position;
        transform.Translate(new Vector3(0, 0, -10));
        prepared = true;
    }

    bool isDragging;
    Vector3 clickPos;
    float deltaTime = 0;

    private void MouseMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out MovingObject movingObject)) { }
            else
            {
                isDragging = true;
                clickPos = theCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            BattleManager.Instance.canSelect = true;
            deltaTime = 0;
        }
        if (isDragging)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= 0.3f) BattleManager.Instance.canSelect = false;
            Vector3 curMousePos = theCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = clickPos - curMousePos;
            dir.z = 0;
            transform.Translate(dir);
        }
    }
}
