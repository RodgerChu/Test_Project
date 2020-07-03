using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdle : WeaponState
{
    private TrajectoryRenderer trajectoryRenderer;

    public void SetTrajectoryRenderer(TrajectoryRenderer renderer)
    {
        trajectoryRenderer = renderer;
    }

    public override void OnStateEnter(PlayerWeaponController weapon)
    {
        trajectoryRenderer.StopSimulating();
    }

    public override void OnStateExit(PlayerWeaponController weapon)
    {

    }

    public override void Update(PlayerWeaponController weapon)
    {
        RaycastHit mouseHit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit);

        var hitPosition = mouseHit.point;
        var shootPosition = -weapon.transform.right;

        var horizontalAngle = Vector2.Angle(new Vector2(shootPosition.x, shootPosition.z), new Vector2(hitPosition.x, hitPosition.z));
        var verticalAngle = Vector2.Angle(new Vector2(shootPosition.x, shootPosition.y), new Vector2(hitPosition.x, hitPosition.y));

        Debug.Log("Angle x " + horizontalAngle + " y " + verticalAngle);

        if (Mathf.Abs(verticalAngle) < weapon.maxFireAngleVertical && Mathf.Abs(horizontalAngle) < weapon.maxFireAngleHorizontal)
        {
            weapon.SetCanShootCursor();

            if (Input.GetMouseButtonDown((int)MouseButtons.LBM))
            {
                weapon.StartAiming(hitPosition);
            }
        }
        else
        {
            weapon.SetCantShootCursor();
        }
    }
}
