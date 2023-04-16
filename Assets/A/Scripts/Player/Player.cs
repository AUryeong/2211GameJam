using System;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public float f_Speed;
    public float dashSpeed;
    protected float _speed;
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = Mathf.Max(0, value);
        }
    }
    [SerializeField] protected SpriteRenderer shieldParticle;
    public ParticleSystem dashParticle;

    public float shieldDuration = 0;
    public int shieldCount = 0;

    public Vector2 speedVector;

    public Vector3 startVector;
    public virtual float goyuCooltime
    {
        get
        {
            return 20;
        }
    }
    public virtual int maxHp
    {
        get
        {
            return 2;
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

    private void Start()
    {
        startVector = Input.acceleration;
    }

    public virtual void Dash()
    {
        float dashPower = dashSpeed * (speed / 13 + 0.33f);

        Vector2 normal = speedVector.normalized;
        RaycastHit2D ray2 = Physics2D.Raycast(transform.position, normal, dashPower, LayerMask.GetMask("Item"));
        if (ray2.collider != null)
            ray2.collider.GetComponent<Item>().OnGet();

        if (normal == Vector2.zero)
            return;

        dashParticle.gameObject.SetActive(true);
        dashParticle.Play();
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        dashParticle.transform.rotation = Quaternion.Euler(-(angle + 90f) - 90f, 90, 0);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, normal, dashPower, LayerMask.GetMask("Edge"));
        if (raycastHit2D.collider != null)
            transform.position = raycastHit2D.point - normal / 2;
        else
            transform.Translate(normal * dashPower);
    }

    public virtual void Goyu()
    {
    }

    protected virtual void SpeedUpdate()
    {
        speed -= Time.deltaTime / 2.5f;
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

        if (Application.isMobilePlatform)
            speedVector =  Input.acceleration - startVector;
        else
            speedVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rigid.velocity = 2 * speed * speedVector;
    }
    protected virtual void Update()
    {
        if (!InGameManager.Instance.isGaming) return;

        if (Input.GetKeyDown(KeyCode.J))
            InGameManager.Instance.BaseAbility();
        if (Input.GetKeyDown(KeyCode.K))
            InGameManager.Instance.GoyuAbility();
        ShieldUpdate();
        SpeedUpdate();
    }
}
