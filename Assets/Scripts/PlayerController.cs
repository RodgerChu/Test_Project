using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponController))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    #region Movement speed
    [Space]
    [Header("Move speed parameters")]
    public float accelerationSpeed = 0.1f;
    public float maxMoveSpeed = 1f;
    public float stoppingSpeed = 0.2f;

    private float currentMoveSpeed = 0f;

    #endregion


    #region Rotating
    [Space]
    public float rotationAcceleration = 0.1f;
    public float maxRotationSpeed = 1f;
    public float rotationStopSpeed = 0.2f;

    private float currentRotationSpeed = 0f;

    #endregion

    private PlayerWeaponController weaponController;

    private CharacterController characterController;
    private Vector2 pointerTextureOffset;
    

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        weaponController = GetComponent<PlayerWeaponController>();
    }

    private void Update()
    {
        
        handleMovement();
    }


    private void handleMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0)
        {
            weaponController.StopSimulating();
            if (Mathf.Abs(currentRotationSpeed) < maxRotationSpeed)
            {
                currentRotationSpeed += horizontal;
            }            
        }
        else
        {
            if (currentRotationSpeed != 0)
            {
                decreaseRotationSpeed();
            }
        }

        if (vertical != 0)
        {
            weaponController.StopSimulating();
            if (Mathf.Abs(currentMoveSpeed) < maxMoveSpeed)
            {
                currentMoveSpeed += accelerationSpeed * vertical;
            }
        }        
        else
        {
            if (currentMoveSpeed != 0)
            {
                decreasePlayerMoveSpeed();
            }
        }

        if (currentMoveSpeed != 0)
        {
            var moveDirection = transform.TransformDirection(Vector3.left) * currentMoveSpeed;
            characterController.Move(moveDirection);
        }

        if (rotationStopSpeed != 0)
        {
            transform.Rotate(new Vector3(0, horizontal, 0));
        }
    }

    private void decreasePlayerMoveSpeed()
    {
        if (Mathf.Abs(currentMoveSpeed) <= accelerationSpeed)
        {
            currentMoveSpeed = 0;
        }
        else if (currentMoveSpeed > 0)
        {
            currentMoveSpeed -= stoppingSpeed;
        }
        else
        {
            currentMoveSpeed += stoppingSpeed;
        }
    }

    private void decreaseRotationSpeed()
    {
        if (Mathf.Abs(currentRotationSpeed) <= rotationAcceleration)
        {
            currentRotationSpeed = 0;
        }
        else if (currentMoveSpeed > 0)
        {
            currentRotationSpeed -= rotationStopSpeed;
        }
        else
        {
            currentRotationSpeed += rotationStopSpeed;
        }
    }
}
