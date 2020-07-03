using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float lifeTime = 10f;
    public float explosionForce = 1000f;
    public float explosionRadius = 3f;
    public GameObject explosionEffect;

    private void Start()
    {
        Destroy(this, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {        
        var instEffect = Instantiate(explosionEffect);
        instEffect.transform.position = transform.position;

        var particalSystem = instEffect.GetComponent<ParticleSystem>();
        if (particalSystem == null)
        {
            Debug.LogWarning("No explosion effect set");
        }
        else
        {
            particalSystem.Play();
            Destroy(instEffect, 5f);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }
}
