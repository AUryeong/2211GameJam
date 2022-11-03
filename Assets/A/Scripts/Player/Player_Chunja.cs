using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Chunja : Player
{
    public override float goyuCooltime
    {
        get
        {
            return 20;
        }
    }
    public override void Goyu()
    {
        base.Goyu();
        foreach (Mob mob in InGameManager.Instance.mobList.FindAll((Mob x) => x.gameObject.activeSelf))
        {
            mob.gameObject.layer = LayerMask.NameToLayer("MobInv");
            mob.rigid.velocity = ((transform.position - mob.transform.position).normalized) * -10;
            mob.bounce = 1;
        }
    }
}
