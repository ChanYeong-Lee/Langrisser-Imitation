using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleCameraMove : MonoBehaviour
{

    private float zoomSpeed = 1;
    private float moveSpeed = 3;
    public BattleMap battleMap;
    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    private void Update()
    {
        Zoom();
        Move();
        Limit();
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize = camera.orthographicSize - scroll * zoomSpeed;
    }
    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, y, 0);
        dir.Normalize();
        transform.position = transform.position + dir * Time.deltaTime * moveSpeed;
    }

    private void Limit()
    {
        float cameraSize = Mathf.Clamp(camera.orthographicSize, 3f, 5f);
        camera.orthographicSize = cameraSize; 

        float boundX = camera.orthographicSize * camera.aspect;
        float boundY = camera.orthographicSize;
        float camX = Mathf.Clamp(transform.position.x, battleMap.mapMinBounds.x + boundX, battleMap.mapMaxBounds.x - boundX);
        float camY = Mathf.Clamp(transform.position.y, battleMap.mapMinBounds.y + boundY, battleMap.mapMaxBounds.y - boundY);
        Vector3 camPos = new Vector3(camX, camY, transform.position.z);
        transform.position = camPos;
    }
}
