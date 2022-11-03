using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Remover : Item
{
    public override void OnGet()
    {
        base.OnGet();
        SoundManager.Instance.PlaySound("wind", SoundType.SE);
        foreach (Mob mob in InGameManager.Instance.mobList.FindAll((Mob x) => x.gameObject.activeSelf))
        {
            mob.gameObject.layer = LayerMask.NameToLayer("MobInv");
            mob.rigid.velocity = ((transform.position - mob.transform.position).normalized) * -10;
            mob.bounce = 1;
        }
    }
}
