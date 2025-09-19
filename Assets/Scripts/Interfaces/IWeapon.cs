using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    bool CanShoot { get; set; }

    float Radius_Range { get; set; }

    int Damage { get; set; }
    
    float Shoot_Rate { get; set; }
}
