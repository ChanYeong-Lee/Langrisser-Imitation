using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSceneCameraMove : MonoBehaviour
{

    private float zoomSpeed = 1;
    private float moveSpeed = 3;
    private BattleMap currentBattleMap;
    private new Camera camera;
    private bool prepared = false;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (prepared)
        {
            Zoom();
            Move();
            Limit();
        }
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
}
