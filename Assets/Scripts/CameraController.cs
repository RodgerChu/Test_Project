using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraStartPosition;
    public Transform playerPosition, weaponPosition;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = cameraStartPosition.position;
        mainCamera.transform.LookAt(weaponPosition);
    }
}
