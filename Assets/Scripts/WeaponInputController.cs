using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponInputController
{
    public Action<float> OnVelocityChange;
    public Action<float> OnVerticleAngleChange;
    public Action<float> OnHorizontalAngleChange;

    // may be create some kind of pointers to this values so player can change them in future
    private float angleChangeSpeed = 0.01f;
    private float velocityChangeSpeed = 0.1f;

    private TrajectoryRenderer trajectoryRenderer;

    private bool rmbPressed = false;
    private Vector3 lastMousePosition;

    public void SetTrajectoryRenderer(TrajectoryRenderer renderer)
    {
        trajectoryRenderer = renderer;
    }

    public void Update(PlayerWeaponController weapon)
    {
        RaycastHit mouseHit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit, 999);

        if (Input.GetMouseButtonDown((int)MouseButtons.LBM))
        {
            if (weapon.CanShoot())
                weapon.Fire();
        }

        if (rmbPressed)
        {
            var mouseChange = lastMousePosition - Input.mousePosition;
            if (mouseChange.y != 0)
            {
                OnVerticleAngleChange?.Invoke(mouseChange.y * angleChangeSpeed);
            }

            if (mouseChange.x != 0)
            {
                OnHorizontalAngleChange?.Invoke(mouseChange.x * angleChangeSpeed);
            }
        }

        // -y 'cuz I wanted to make scrolling upward to be positive value
        var mouseScrollChange = -Input.mouseScrollDelta.y;

        if (mouseScrollChange != 0)
        {
            OnVelocityChange?.Invoke(mouseScrollChange * velocityChangeSpeed);
        }

        if (Input.GetMouseButtonDown((int)MouseButtons.RMB))
        {
            rmbPressed = true;
            weapon.hudController?.StartAdjusting();
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp((int)MouseButtons.RMB))
        {
            rmbPressed = false;
            weapon.hudController?.StopAdjusting();
        }
    }
}
