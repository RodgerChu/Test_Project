using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public Texture2D canShootTexture;
    public Texture2D cantShootTexture;

    public GameObject projectile;
    public Transform weaponPosition;
    public Transform projectileStartPosition;
    
    public float fireRate = 1f;
    public ParticleSystem shootEffect;

    public float maxFireAngleVertical = 80f;
    public float maxFireAngleHorizontal = 30f;
    public float minFireAngleVerticle = 20f;

    private float verticleAngleOffset = 0f;
    private float currentHorizontalAngle = 0f;
    private float currentVerticalAngle = 0f;

    public float projectileStartingSpeed = 90f;
    public float projectileMinimumStartSpeed = 20f;
    public float projectileMaximumStartSpeed = 150f;

    private float currentSpeedOffset = 0f;

    public HUDController hudController;

    public Transform cameraPosition;

    private float lastShootTime = 0f;


    private Vector2 pointerTextureOffset;


    public readonly WeaponState idleState = new WeaponIdle();
    public readonly WeaponState aimingState = new WeaponAiming();
    private WeaponState currentState;

    private TrajectoryRenderer trajectoryRenderer;
    // Start is called before the first frame update
    void Start()
    {
        pointerTextureOffset = new Vector2(canShootTexture.width / 2, canShootTexture.height / 2);
        Cursor.SetCursor(canShootTexture, pointerTextureOffset, CursorMode.Auto);        
        shootEffect.Stop();

        var lineRenderer = GetComponent<LineRenderer>();
        trajectoryRenderer = new TrajectoryRenderer(lineRenderer, projectileStartPosition);
        trajectoryRenderer.ChangeSpeed(projectileStartingSpeed);

        ((WeaponIdle)idleState).SetTrajectoryRenderer(trajectoryRenderer);
        ((WeaponAiming)aimingState).SetTrajectoryRenderer(trajectoryRenderer);
        ((WeaponAiming)aimingState).OnAngleChange += OnAngleChanged;
        ((WeaponAiming)aimingState).OnVelocityChange += OnVelocityChanged;

        currentState = idleState;
    }

    // Update is called once per frame
    void Update()
    {

        if (lastShootTime > 0)
        {
            lastShootTime -= Time.deltaTime;
        }

        if (lastShootTime < 0)
        {
            lastShootTime = 0;
        }

        currentState.Update(this);
    }

    public void TransitionToState(WeaponState state)
    {
        Debug.Log("Transition from " + currentState + " to " + state);
        currentState.OnStateExit(this);
        currentState = state;
        currentState.OnStateEnter(this);
    }

    public void StartAiming(Vector3 position)
    {
        var rotation = weaponPosition.rotation;
        weaponPosition.rotation = Quaternion.identity;
        weaponPosition.Rotate(0, 0, -90);
        var shootPosition = weaponPosition.forward;
        weaponPosition.rotation = rotation;
        currentHorizontalAngle = Vector2.Angle(new Vector2(shootPosition.x, shootPosition.z), new Vector2(position.x, position.z));
        currentVerticalAngle = Vector2.Angle(new Vector2(shootPosition.x, shootPosition.y), new Vector2(position.x, position.y));

        weaponPosition.transform.LookAt(position);

        hudController.SetAngle(currentVerticalAngle);
        ((WeaponAiming)aimingState).SetTarget(position);
        

        TransitionToState(aimingState);
    }

    public bool CanShoot()
    {
        return lastShootTime == 0;
    }

    public void SetCanShootCursor()
    {
        Cursor.SetCursor(canShootTexture, pointerTextureOffset, CursorMode.Auto);
    }

    public void SetCantShootCursor()
    {
        Cursor.SetCursor(cantShootTexture, pointerTextureOffset, CursorMode.Auto);
    }

    private void OnAngleChanged(float delta)
    {
        verticleAngleOffset += delta;
        if (currentVerticalAngle + verticleAngleOffset > maxFireAngleVertical)
        {
            verticleAngleOffset = maxFireAngleVertical - currentVerticalAngle;
        }
        else if (currentVerticalAngle + verticleAngleOffset < minFireAngleVerticle)
        {
            verticleAngleOffset = currentVerticalAngle + minFireAngleVerticle;
        }



        var hitPoint = trajectoryRenderer.GetHitPoint();
        var distance = Vector3.Distance(projectileStartPosition.forward, hitPoint);

        var rotationY = weaponPosition.transform.rotation.y;
        Debug.Log("y " + rotationY);
        weaponPosition.transform.rotation = Quaternion.identity;
        //weaponPosition.transform.Rotate(0, rotationY, 0);

        weaponPosition.transform.Rotate(currentVerticalAngle + verticleAngleOffset, -90 + (90 - currentHorizontalAngle), 0);
        Debug.Log("y " + rotationY);
        hudController.SetDistance(distance);
        hudController.SetAngle(currentVerticalAngle + verticleAngleOffset);

        trajectoryRenderer.UpdateSimulation();
    }

    private void OnVelocityChanged(float delta)
    {
        currentSpeedOffset += delta;
        if (projectileStartingSpeed + currentSpeedOffset > projectileMaximumStartSpeed)
        {
            currentSpeedOffset = projectileStartingSpeed + currentSpeedOffset - projectileMaximumStartSpeed;
        }
        else if (projectileStartingSpeed + currentSpeedOffset < projectileMinimumStartSpeed)
        {
            currentSpeedOffset = projectileStartingSpeed + currentSpeedOffset + projectileMinimumStartSpeed;
        }

        trajectoryRenderer.ChangeSpeed(currentSpeedOffset + projectileStartingSpeed);
        trajectoryRenderer.UpdateSimulation();

        var hitPoint = trajectoryRenderer.GetHitPoint();
        var distance = Vector3.Distance(projectileStartPosition.forward, hitPoint);

        hudController.SetDistance(distance);
        hudController.SetVelocity(currentSpeedOffset + projectileStartingSpeed);
    }

    public void Fire(Vector3 target)
    {
        lastShootTime = fireRate;
        shootEffect.Play();
        var instProjectile = Instantiate(projectile);
        instProjectile.transform.position = projectileStartPosition.position;
        instProjectile.transform.LookAt(projectileStartPosition.forward);

        var projectileRigidbody = instProjectile.GetComponent<Rigidbody>();
        if (projectileRigidbody == null)
        {
            Debug.LogWarning("Projectile must contain Rigidbody component!");
        }
        else
        {
            projectileRigidbody.AddForce(projectileStartPosition.forward * (projectileStartingSpeed + currentSpeedOffset), ForceMode.VelocityChange);
        }

        verticleAngleOffset = 0;
        trajectoryRenderer.SetAngleOffset(0);
        TransitionToState(idleState);
    }
}
