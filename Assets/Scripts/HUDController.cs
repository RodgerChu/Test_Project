using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Text velocityValue;
    [SerializeField] private Text distanceValue;
    [SerializeField] private Text angleValue;

    [SerializeField] private GameObject hud;

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

    public void SetAngle(float angle)
    {
        angleValue.text = angle.ToString();
    }
}
