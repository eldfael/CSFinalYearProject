using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    Transform playerTransform;
    Camera mainCamera;

    Vector2 mousePos;
    Vector2 mouseInputPos;
    Vector2 cameraPos;
    Vector2 playerOffset;
    //Vector2 cameraSize;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Confined;
    }
    void FixedUpdate()
    {
        mouseInputPos.x = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
        mouseInputPos.y = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);

        mousePos = mainCamera.ScreenToWorldPoint(mouseInputPos);

        playerOffset = mousePos - new Vector2 (playerTransform.position.x,playerTransform.position.y);
        
        /*
        if (playerOffset.x <= 1f && playerOffset.x >= -1f && playerOffset.y <= 1f && playerOffset.y >= -1f) 
        {
            cameraPos.x = playerTransform.position.x;
            cameraPos.y = playerTransform.position.y;
        }
        else 
        {
            if (playerOffset.x <= 1f && playerOffset.x >= -1f)
            {
                cameraPos.x = (playerTransform.position.x * 2.5f + mousePos.x) / 3.5f;
            }
            else 
            { 
                cameraPos.x = (playerTransform.position.x * 2.5f + mousePos.x ) / 3.5f - Mathf.Sign(playerOffset.x);
            }
            if (playerOffset.y <= 1f && playerOffset.y >= -1f)
            {
                cameraPos.y = (playerTransform.position.y * 2.5f + mousePos.y) / 3.5f;
            }
            else
            {
                cameraPos.y = (playerTransform.position.y * 2.5f + mousePos.y ) / 3.5f - Mathf.Sign(playerOffset.y);
            }
        }
        */

        cameraPos.x = (playerTransform.position.x * 2.5f + mousePos.x) / 3.5f;
        cameraPos.y = (playerTransform.position.y * 2.5f + mousePos.y) / 3.5f;


        transform.position = new Vector3(cameraPos.x, cameraPos.y, -100);
    }
}
