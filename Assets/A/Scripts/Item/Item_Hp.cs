using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Hp : Item
{
    public override void OnGet()
    {
        base.OnGet();
        InGameManager.Instance.hp++;
    }
}
