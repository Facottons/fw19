using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGunPistol : Pickup {

    private void Awake()
    {
         pickupType = PickupType.pistolGun;
    }

}
