using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : Health
{
    public override void Death()
    {
        Destroy(gameObject);
    }
}