using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAiming : WeaponState
{
    public Action<float> OnVelocityChange;
    public Action<float> OnAngleChange;

    private float angleChangeSpeed = 1f;
    private float velocityChangeSpeed = 0.1f;

    private TrajectoryRenderer trajectoryRenderer;
    private Vector3 target;
    private Transform projectileStartPosition;


    private bool rmbPressed = false;
    private Vector3 lastMousePosition;

    public void SetProjectileStartPosition(Transform position)
    {
        projectileStartPosition = position;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void SetTrajectoryRenderer(TrajectoryRenderer renderer)
    {
        trajectoryRenderer = renderer;
    }

    public override void OnStateEnter(PlayerWeaponController weapon)
    {        
        trajectoryRenderer.StartSimulating(target);
        trajectoryRenderer.UpdateSimulation();
        lastMousePosition = Input.mousePosition;        
    }

    public override void OnStateExit(PlayerWeaponController weapon)
    {
        trajectoryRenderer.StopSimulating();
        rmbPressed = false;
    }

    public override void Update(PlayerWeaponController weapon)
    {
        var mouseScrollChange = Input.mouseScrollDelta.y * angleChangeSpeed;

        if (mouseScrollChange != 0)
        {            
            OnAngleChange?.Invoke(mouseScrollChange);
        }

        if (Input.GetMouseButtonDown((int)MouseButtons.RMB))
        {
            rmbPressed = true;
        }
        if (Input.GetMouseButtonUp((int)MouseButtons.RMB))
        {
            rmbPressed = false;
        }

        if (rmbPressed)
        {
            var mouseChange = lastMousePosition - Input.mousePosition;
            if (mouseChange.y != 0)
            {
                OnVelocityChange?.Invoke(-mouseChange.y * velocityChangeSpeed);
            }
        }

        if (Input.GetMouseButtonDown((int)MouseButtons.LBM))
        {
            if (weapon.CanShoot())
                weapon.Fire(target);
        }

        lastMousePosition = Input.mousePosition;
    }
}
