using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : Item
{
    public override void OnGet()
    {
        base.OnGet();
        Player.Instance.shieldDuration = 10;
        Player.Instance.shieldCount += 2;
    }
}
