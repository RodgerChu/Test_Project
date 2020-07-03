using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraStartPosition;
    public Transform playerPosition, weaponPosition;

    private float offset = 0f;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = cameraStartPosition.position;
        mainCamera.transform.LookAt(weaponPosition);

        //var mouseX = Input.GetAxis("Mouse X");
        //var mouseY = Input.GetAxis("Mouse Y");

        //mainCamera.transform.RotateAround(playerPosition.position, Vector3.up, mouseX * 2);
        //mainCamera.transform.RotateAround(playerPosition.position, Vector3.right, mouseY * 2);
    }
}
