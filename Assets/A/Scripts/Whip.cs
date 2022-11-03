
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
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
        spriteRenderer.DOKill();
        spriteRenderer.color = new Color(0, 1, 0.1f, 0.5f);
        spriteRenderer.DOFade(0.1f, 0.5f).SetLoops(7, LoopType.Yoyo).OnComplete(() =>
        {
            SoundManager.Instance.PlaySound("whip_hit");
            InGameManager.Instance.WhipHitEffect(transform.position);
            gameObject.SetActive(false);
            Collider2D playerColider = Physics2D.OverlapCircle(transform.position, 2.5f, LayerMask.GetMask("Player", "Inv"));
            Collider2D[] coliders = Physics2D.OverlapCircleAll(transform.position, 2.5f, LayerMask.GetMask(nameof(Mob)));
            foreach (Collider2D colider in coliders)
            {
                Mob mob = colider.GetComponent<Mob>();
                if (playerColider != null)
                    mob.gameObject.layer = LayerMask.NameToLayer("MobInv");
                mob.rigid.velocity = ((transform.position - mob.transform.position).normalized) * -4;
                mob.bounce = 1;
            }
            if (playerColider != null)
            {
                Player.Instance.speed += 7;
                InGameManager.Instance.hp++;
            }
        });
    }

    protected void OnDisable()
    {
        spriteRenderer.DOKill();
    }
}
