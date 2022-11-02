using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            InGameManager.Instance.hp++;
            //여기에 setActive 넣어주삼 ( 풀링 
        }
    }
}
