using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Player : Singleton<Player>
{
    [SerializeField] protected float speed;
    [SerializeField] protected float dashSpeed;

    public Vector2 speedVector;
    public virtual float goyuCooltime
    {
        get
        {
            return 20;
        }
    }
    protected Rigidbody2D _rigid;
    protected Rigidbody2D rigid
    {
        get
        {
            if (_rigid == null)
                _rigid = GetComponent<Rigidbody2D>();
            return _rigid;
        }
    }
    public virtual void Dash()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, speedVector, dashSpeed, LayerMask.GetMask("Edge"));
        if (raycastHit2D.collider != null)
        {
            Debug.Log(speedVector);
            transform.position = raycastHit2D.point - speedVector/2;
        }
        else
        {
            transform.Translate(speedVector * dashSpeed);
        }
    }

    public virtual void Goyu()
    {
    }
    protected virtual void FixedUpdate()
    {
        speedVector = InGameManager.Instance.joystick.vector;
        speedVector += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        speedVector.Normalize();
        rigid.velocity = speedVector * speed;
    }
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            InGameManager.Instance.BaseAbility();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            InGameManager.Instance.GoyuAbility();
        }
    }
}
