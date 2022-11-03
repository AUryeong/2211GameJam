using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_YoungHee : Player
{
    public override void Goyu()
    {
        base.Goyu();
        shieldDuration = 10;
        shieldCount += 2;
    }
}
