using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Notes:
///  Rotation upward performed with negative angle (i.e. -10)
///  Rotation horizontal to left also performed with negative angle (i.e. -90)
///  Transform.forward actualy pointing to the right side of the tank


[RequireComponent(typeof(LineRenderer))]
public class PlayerWeaponController : MonoBehaviour
{
    #region Prefabs
    [Space]
    [Header("Prefabs")]
    public GameObject projectile;
    public ParticleSystem shootEffect;

    #endregion

    #region Positions
    [Space]
    [Header("Positions")]
    public Transform weaponPosition;
    public Transform projectileStartPosition;
    public Transform cameraPosition;

    #endregion   

    #region Angle params
    [Space]
    [Header("Weapon verticle angle params")]
    public float maxFireAngleVertical = 80f;    
    public float minFireAngleVerticle = 20f;
    private float verticleAngleOffset = 0f;

    [Space]
    [Header("Weapon horizontal angle params")]
    public float maxFireAngleHorizontal = 30f;
    private float horizontalAngleOffset = 0f;   

    #endregion

    #region Projectile params

    [Space]
    [Header("Projectile speed params")]
    public float projectileStartingSpeed = 90f;
    public float projectileMinimumStartSpeed = 20f;
    public float projectileMaximumStartSpeed = 150f;

    private float currentSpeedOffset = 0f;

    #endregion     

    #region Weapon general params

    [Space]
    [Header("Weapon general params")]
    [Tooltip("Amount of time between shots")]
    public float fireRate = 1f;

    #endregion

    [Space]
    [Header("System links")]
    [Tooltip("Optional parameter")]
    public HUDController hudController;

    private WeaponInputController weaponInputController = new WeaponInputController();
    private float lastShootTime = 0f;  
    private TrajectoryRenderer trajectoryRenderer;
    
    void Start()
    {       
        shootEffect.Stop();        

        var lineRenderer = GetComponent<LineRenderer>();
        trajectoryRenderer = new TrajectoryRenderer(lineRenderer, projectileStartPosition);
        trajectoryRenderer.ChangeSpeed(projectileStartingSpeed);

        weaponInputController.SetTrajectoryRenderer(trajectoryRenderer);

        weaponInputController.OnVerticleAngleChange += OnVerticleAngleChanged;
        weaponInputController.OnVelocityChange += OnVelocityChanged;
        weaponInputController.OnHorizontalAngleChange += OnHorizontalAngleChanged;
    }

    
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

        weaponInputController.Update(this);
    }

    public bool CanShoot()
    {
        return lastShootTime == 0;
    }

    private void OnVerticleAngleChanged(float delta)
    {
        verticleAngleOffset += delta;
        // '>' 'cuz rotating to the bottom performed with positive angle
        if (verticleAngleOffset > minFireAngleVerticle)
        {
            verticleAngleOffset = minFireAngleVerticle;
        }
        // add '-' to maxFireAngleVertical 'cuz rotating to top performed with negative angle
        else if (verticleAngleOffset < -maxFireAngleVertical)
        {
            verticleAngleOffset = -maxFireAngleVertical;
        }

        var hitPoint = trajectoryRenderer.GetHitPoint();
        var distance = Vector3.Distance(projectileStartPosition.forward, hitPoint);

        performRotation();

        trajectoryRenderer.UpdateSimulation();

        hudController?.SetDistance(distance);
        hudController?.SetVerticleAngle(verticleAngleOffset);        
    }

    private void OnHorizontalAngleChanged(float delta)
    {
        horizontalAngleOffset += delta;
        if (horizontalAngleOffset > maxFireAngleHorizontal)
        {
            horizontalAngleOffset = maxFireAngleHorizontal;
        }
        else if (horizontalAngleOffset < -maxFireAngleHorizontal)
        {
            horizontalAngleOffset = -maxFireAngleHorizontal;
        }

        performRotation();

        trajectoryRenderer.UpdateSimulation();
        hudController?.SetHorizontalAngle(horizontalAngleOffset);
    }

    private void performRotation()
    {
        weaponPosition.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        // -90 on Y angle to make weapon look forward (according to tank)
        weaponPosition.transform.localRotation = Quaternion.Euler(new Vector3(verticleAngleOffset, -90 - horizontalAngleOffset, 0));
    }

    private void OnVelocityChanged(float delta)
    {
        currentSpeedOffset += delta;
        if (projectileStartingSpeed + currentSpeedOffset > projectileMaximumStartSpeed)
        {
            currentSpeedOffset = projectileMaximumStartSpeed - projectileStartingSpeed;
        }
        else if (projectileStartingSpeed + currentSpeedOffset < projectileMinimumStartSpeed)
        {
            currentSpeedOffset = projectileMinimumStartSpeed - projectileStartingSpeed;
        }

        trajectoryRenderer.ChangeSpeed(currentSpeedOffset + projectileStartingSpeed);
        trajectoryRenderer.UpdateSimulation();

        var hitPoint = trajectoryRenderer.GetHitPoint();
        var distance = Vector3.Distance(projectileStartPosition.forward, hitPoint);

        hudController?.SetDistance(distance);
        hudController?.SetVelocity(currentSpeedOffset + projectileStartingSpeed);
    }

    public void Fire()
    {
        lastShootTime = fireRate;
        shootEffect.Play();
        var instProjectile = Instantiate(projectile);
        instProjectile.transform.position = projectileStartPosition.position;
        instProjectile.transform.rotation = weaponPosition.transform.rotation; 

        var projectileRigidbody = instProjectile.GetComponent<Rigidbody>();
        if (projectileRigidbody == null)
        {
            Debug.LogWarning("Projectile must contain Rigidbody component!");
        }
        else
        {
            projectileRigidbody.AddForce(projectileStartPosition.forward * (projectileStartingSpeed + currentSpeedOffset), ForceMode.VelocityChange);
        }
    }

    public void StopSimulating()
    {
        trajectoryRenderer.StopSimulating();
    }
}
