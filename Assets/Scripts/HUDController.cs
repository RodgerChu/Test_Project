using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Space]
    [Header("Text field links")]
    [SerializeField] private Text velocityValue;
    [SerializeField] private Text distanceValue;
    [SerializeField] private Text verticleAngleValue;
    [SerializeField] private Text horizontalAngleValue;

    [Space]
    [Header("Aiming hints")]
    [SerializeField] private GameObject adjustingStartPoint;
    [SerializeField] private GameObject adjustingMousePointer;

    [SerializeField] private GameObject hud;

    private bool adjusting = false;

    private void Start()
    {
        adjustingStartPoint.SetActive(false);
        adjustingMousePointer.SetActive(false);
    }

    private void Update()
    {
        if (adjusting)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = 0;
            var pointPosition = adjustingMousePointer.transform.position;

            adjustingMousePointer.transform.position = mousePosition;
            adjustingMousePointer.transform.LookAt(adjustingStartPoint.transform);
        }
    }

    public void HideHUD()
    {
        hud.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ShowHUD()
    {
        hud.transform.localScale = Vector3.zero;
    }

    public void SetVelocity(float velocity)
    {
        velocityValue.text = velocity.ToString();
    }

    public void SetDistance(float distance)
    {
        distanceValue.text = distance.ToString();
    }

    public void SetVerticleAngle(float angle)
    {
        verticleAngleValue.text = angle.ToString();
    }

    public void SetHorizontalAngle(float angle)
    {
        horizontalAngleValue.text = angle.ToString();
    }

    public void StartAdjusting()
    {
        adjusting = true;
        var mousePosition = Input.mousePosition;
        mousePosition.z = 0;

        adjustingStartPoint.SetActive(true);
        adjustingStartPoint.transform.position = mousePosition;

        adjustingMousePointer.transform.position = mousePosition;

        adjustingMousePointer.SetActive(true);

        Cursor.visible = false;
    }

    public void StopAdjusting()
    {
        adjusting = false;

        adjustingStartPoint.SetActive(false);
        adjustingMousePointer.SetActive(false);

        Cursor.visible = true;
    }
}
