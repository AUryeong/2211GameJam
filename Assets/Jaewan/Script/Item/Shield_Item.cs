using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Item : MonoBehaviour
{
    bool invincibility_on = false;
    [SerializeField] private GameObject invincibility_Image;
    [SerializeField] private CircleCollider2D P;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            P = P.GetComponent<CircleCollider2D>();
            StartCoroutine(invincibility());
        }
    }
    IEnumerator invincibility() 
    {

        invincibility_Image.SetActive(true);
        invincibility_on = true;
        yield return new WaitForSeconds(10);

        invincibility_Image.SetActive(false);
        invincibility_on = false;
    }
}
