using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponState
{
    public abstract void OnStateEnter(PlayerWeaponController weapon);

    public abstract void OnStateExit(PlayerWeaponController weapon);

    public abstract void Update(PlayerWeaponController weapon);
}
