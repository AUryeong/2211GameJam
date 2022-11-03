using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Player : Singleton<Player>
{
    public float f_Speed;
    public float dashSpeed;
    public float speed;
    [SerializeField] protected SpriteRenderer shieldParticle;
    public ParticleSystem dashParticle;

    public float shieldDuration = 0;
    public int shieldCount = 0;

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
        RaycastHit2D ray2 = Physics2D.Raycast(transform.position, speedVector, dashSpeed, LayerMask.GetMask("Item"));
        if (ray2.collider != null)
            ray2.collider.GetComponent<Item>().OnGet();

        if(speedVector == Vector2.zero)
            return;

        dashParticle.gameObject.SetActive(true);
        dashParticle.Play();
        float angle = Mathf.Atan2(speedVector.y, speedVector.x) * Mathf.Rad2Deg;
        dashParticle.transform.rotation = Quaternion.Euler(-(angle + 90f) - 90f, 90, 0);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, speedVector, dashSpeed, LayerMask.GetMask("Edge"));
        if (raycastHit2D.collider != null)
            transform.position = raycastHit2D.point - speedVector / 2;
        else
            transform.Translate(speedVector * dashSpeed);
    }

    public virtual void Goyu()
    {
    }

    protected virtual void SpeedUpdate()
    {
        speed -= Time.deltaTime / 7;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            InGameManager.Instance.hp--;
        }
    }
    protected virtual void ShieldUpdate()
    {
        if (shieldDuration > 0 == !shieldParticle.gameObject.activeSelf)
            shieldParticle.gameObject.SetActive(!shieldParticle.gameObject.activeSelf);
        if (shieldDuration > 0)
        {
            shieldDuration -= Time.deltaTime;
            if (shieldDuration <= 0)
                shieldCount = 0;
        }
    }
    protected virtual void FixedUpdate()
    {
        if (!InGameManager.Instance.isGaming) return;

        speedVector = InGameManager.Instance.joystick.vector;
        speedVector += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        speedVector.Normalize();
        rigid.velocity = speedVector * speed;
    }
    protected virtual void Update()
    {
        if (!InGameManager.Instance.isGaming) return;

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.J))
            InGameManager.Instance.BaseAbility();
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.K))
            InGameManager.Instance.GoyuAbility();
        ShieldUpdate();
        SpeedUpdate();
    }
}
