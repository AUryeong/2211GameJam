using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    protected Rigidbody2D _rigid;
    public Rigidbody2D rigid
    {
        get
        {
            if (_rigid == null)
                _rigid = GetComponent<Rigidbody2D>();
            return _rigid;
        }
    }
    public float speed = 1;
    public int bounce = 5;
    protected void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("Mob");
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Edge"))
        {
            bounce--;
            if (bounce <= 0)
                gameObject.SetActive(false);
        }
    }
}
