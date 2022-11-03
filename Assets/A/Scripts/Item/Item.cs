using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    float maxDuration = 5;
    float duration = 5;
    bool isFading = false;
    SpriteRenderer _spriteRenderer;

    SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }
    protected void OnEnable()
    {
        duration = maxDuration;
        isFading = false;
    }
    protected void Update()
    {
        if (duration <= 0)
            gameObject.SetActive(false);
        else
        {
            duration -= Time.deltaTime;
            if (duration <= 2.5f)
            {
                if (isFading)
                {
                    spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 0.5f, Time.deltaTime * 12));
                    if (spriteRenderer.color.a <= 0.52f)
                        isFading = !isFading;
                }
                else
                {
                    spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 1, Time.deltaTime * 12));
                    if (spriteRenderer.color.a > 0.97f)
                        isFading = !isFading;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            OnGet();
    }

    public virtual void OnGet()
    {
        gameObject.SetActive(false);
    }
}
