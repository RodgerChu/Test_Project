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
    public float projectileStartingForce = 90f;
    public float fireRate = 1f;
    public ParticleSystem shootEffect;

    public float maxFireAngle = 80f;
    public float fireAngle = 10f;

    public Transform cameraPosition;

    private float lastShootTime = 0f;
    private Vector2 pointerTextureOffset;
    private Camera mainCamera;

    private float maxShootDistance = 0f;
    // Start is called before the first frame update
    void Start()
    {
        pointerTextureOffset = new Vector2(canShootTexture.width / 2, canShootTexture.height / 2);
        Cursor.SetCursor(canShootTexture, pointerTextureOffset, CursorMode.Auto);

        mainCamera = Camera.main;
        shootEffect.Stop();
        calculateMaxShootDistance();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = cameraPosition.position;
        mainCamera.transform.LookAt(weaponPosition);

        if (lastShootTime > 0)
        {
            lastShootTime -= Time.deltaTime;
        }
        else if (lastShootTime < 0)
        {
            lastShootTime = 0;
        }            

        RaycastHit mouseHit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit);

        var hitPosition = mouseHit.point;

        var angleBetweenMouseAndGun = Vector3.Angle(projectileStartPosition.forward, hitPosition);
        //Debug.Log("Angle = " + angleBetweenMouseAndGun);
        //if (Mathf.Abs(angleBetweenMouseAndGun) < fireAngle)
        if (Vector3.Distance(hitPosition, projectileStartPosition.position) < maxShootDistance)
        {
            Cursor.SetCursor(canShootTexture, pointerTextureOffset, CursorMode.Auto);
            if (Input.GetMouseButton((int)MouseButtons.RMB))
            {
                if (lastShootTime == 0)
                {
                    fire(hitPosition);
                }
            }
        }
        else
        {
            Cursor.SetCursor(cantShootTexture, pointerTextureOffset, CursorMode.Auto);
        }
    }

    private void fire(Vector3 target)
    {
        Debug.Log("Firing at: " + target);
        Debug.Log("Projectile start position: " + projectileStartPosition.position);

        lastShootTime = fireRate;
        shootEffect.Play();
        var instProjectile = Instantiate(projectile);
        instProjectile.transform.position = projectileStartPosition.position;
        instProjectile.transform.LookAt(target);
        instProjectile.transform.Rotate(-maxFireAngle, 0, 0);

        var projectileRigidbody = instProjectile.GetComponent<Rigidbody>();
        if (projectileRigidbody == null)
        {
            Debug.LogWarning("Projectile must contain Rigidbody component!");
        }
        else
        {
            projectileRigidbody.AddForce(instProjectile.transform.forward * projectileStartingForce, ForceMode.Impulse);
        }
    }


    //IEnumerator SimulateProjectile()
    //{
    //    // Short delay added before Projectile is thrown
    //    yield return new WaitForSeconds(1.5f);

    //    // Move projectile to the position of throwing object + add some offset if needed.
    //    Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

    //    // Calculate distance to target
    //    float target_Distance = Vector3.Distance(Projectile.position, Target.position);

    //    // Calculate the velocity needed to throw the object to the target at specified angle.
    //    float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

    //    // Extract the X  Y componenent of the velocity
    //    float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
    //    float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

    //    // Calculate flight time.
    //    float flightDuration = target_Distance / Vx;

    //    // Rotate projectile to face the target.
    //    Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

    //    float elapse_time = 0;

    //    while (elapse_time < flightDuration)
    //    {
    //        Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

    //        elapse_time += Time.deltaTime;

    //        yield return null;
    //    }
    //}

    private void calculateMaxShootDistance()
    {
        var rb = projectile.GetComponent<Rigidbody>();
        var baseRotation = projectileStartPosition.rotation;
        projectileStartPosition.Rotate(-maxFireAngle, 0, 0);
        var impulse = projectileStartPosition.forward.magnitude * projectileStartingForce;
        var speed = impulse / rb.mass;

        maxShootDistance = speed * speed * Mathf.Sin(2 * maxFireAngle * Mathf.Deg2Rad) / (2 * Physics.gravity.y);

        projectileStartPosition.rotation = baseRotation;
        Debug.LogWarning("Speed: " + speed);
        Debug.LogWarning("impulse: " + impulse);
        Debug.LogWarning("Max shoot distance: " + maxShootDistance);
    }

    private void calculateAngleBetweenProjectileAndTarget(Vector3 target)
    {
        var rb = projectile.GetComponent<Rigidbody>();
        var impulse = projectileStartingForce * Time.fixedDeltaTime;
        var speed = impulse / rb.mass;


    }
}
