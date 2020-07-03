using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShoot : WeaponState
{
    private GameObject projectilePrefab;
    private Vector3 target;
    private float angleOffset;
    private float speed;

    public Vector3 Target
    {
        set { target = value; }
    }

    public float AngleOffset
    {
        set { angleOffset = value; }
    }

    public float Speed
    {
        set { speed = value; }
    }

    public GameObject ProjectilePrefab
    {
        set { projectilePrefab = value; }
    }


    public override void OnStateEnter(PlayerWeaponController weapon)
    {

    }

    public override void OnStateExit(PlayerWeaponController weapon)
    {
        
    }

    public override void Update(PlayerWeaponController weapon)
    {
        
    }
}
