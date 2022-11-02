using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_base : MonoBehaviour
{
    [SerializeField] GameObject target;

    Vector3 dir2;
    Rigidbody2D _rb2d;
    public float Mob_speed = 1;

    public int Mob_count = 3;
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        Vector3 dir = target.transform.position - transform.position;
        dir2 = dir;
        StartCoroutine(Mob_Move());
    }
    IEnumerator Mob_Move() 
    {
        _rb2d.velocity = dir2 * Mob_speed;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Mob_Move());
    }
    void Mob_del() 
    {
        if (Mob_count < 0) 
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        Mob_Move();
        Mob_del();
        if (Mathf.Abs(_rb2d.velocity.x) > 5)
        {
            _rb2d.velocity = new Vector2(Mathf.Sign(_rb2d.velocity.x) * Mob_speed, _rb2d.velocity.y);
        }
        if (Mathf.Abs(_rb2d.velocity.y) > 5)
        {
            _rb2d.velocity = new Vector2(Mathf.Sign(_rb2d.velocity.y) * Mob_speed, _rb2d.velocity.x);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") 
        {
            Mob_count--;
            StopAllCoroutines();
        }
    }
}
