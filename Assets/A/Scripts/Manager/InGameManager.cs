using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class InGameManager : Singleton<InGameManager>
{
    public Character character = Character.Chunja;
    [Header("UI")]
    public VariableJoystick joystick;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Image hitEffect;

    [Header("목숨")]
    [SerializeField] HorizontalLayoutGroup layoutGroup;
    [SerializeField] List<Image> heartImage;
    protected int _hp = 1;

    [Header("스킬")]
    [SerializeField] Image goyuAbility;
    protected float goyuDuration = 0;
    [SerializeField] Image baseAbility;
    protected float baseDuration = 0;
    protected float baseCooltime = 5;

    [Header("몹 생성")]
    [SerializeField] Mob mobOrigin;
    public List<Mob> mobList = new List<Mob>();
    protected float mobCreateCooltime = 2f;
    protected float mobCreateDuraiton = 0;
    protected int mobCreateCount = 0;

    protected int mobCreatePatternFirst = 12;
    protected int mobCreatePatternFirstCooltime = 5;
    protected int mobCreatePatternFirstDuration = 0;

    protected int mobCreatePatternSecond = 25;
    protected int mobCreatePatternSecondCooltime = 13;
    protected int mobCreatePatternSecondDuration = 0;

    protected float mobCreateRemoveValue = 0.005f;
    protected float mobCreateMinValue = 0.4f;

    [Header("아이템 생성")]
    [SerializeField] Item[] items;
    protected float itemCreateDuration = 0;
    protected float itemCreateCooltime = 20;

    [Header("고유 스킬")]
    [SerializeField] Image outlineImage;
    [SerializeField] Image baseImage;
    [SerializeField] Image characterImage;
    [SerializeField] Image textImage;

    [SerializeField] Sprite[] characterSprites;
    [SerializeField] Sprite[] textSprites;

    public bool isGaming
    {
        get; private set;
    }

    [Header("게임 오버")]
    [SerializeField] GameObject gameOverParent;
    [SerializeField] TextMeshProUGUI gameOverTimer;

    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (!isGaming)
                return;
            if (value < _hp && _hp != 0)
                if (hitEffect.gameObject.activeSelf)
                    return;
                else
                    HitEffect();
            if (Player.Instance.shieldDuration > 0)
            {
                Player.Instance.shieldCount--;
                if (Player.Instance.shieldCount <= 0)
                    Player.Instance.shieldDuration = 0;
                return;
            }
            _hp = Mathf.Clamp(value, 0, 3);
            if (_hp <= 0)
                GameOver();
            for (int i = 0; i < heartImage.Count; i++)
                heartImage[i].gameObject.SetActive(_hp > i);
        }
    }

    protected float timer = 0;

    protected void Start()
    {
        character = CharacterManager.Instance.selectCharacter;
        Reset();
    }

    protected void HitEffect()
    {
        hitEffect.gameObject.SetActive(true);
        hitEffect.DOKill();
        hitEffect.color = new Color(1, 0, 0, 0f);
        hitEffect.DOFade(0.4f, 0.1f).OnComplete(() =>
        {
            hitEffect.DOFade(0, 0.5f).OnComplete(() =>
            {
                hitEffect.gameObject.SetActive(false);
            });
        });
    }
    protected void Update()
    {
        if (!isGaming)
            return;
        float deltaTime = Time.deltaTime;
        UpdateTimer(deltaTime);
        UpdateAbiltiyImage(deltaTime);
        CheckCreateMob(deltaTime);
        CheckCreateItem(deltaTime);
    }

    protected void CheckCreateItem(float deltaTime)
    {
        itemCreateDuration += deltaTime;
        if (itemCreateDuration >= itemCreateCooltime)
        {
            itemCreateDuration -= itemCreateCooltime;
            GameObject itemObj = PoolManager.Instance.Init(items[Random.Range(0, items.Length)].gameObject);
            itemObj.transform.position = Random.insideUnitCircle * 4.2f;
        }
    }
    protected void CheckCreateMob(float deltaTime)
    {
        mobCreateDuraiton += deltaTime;
        if (mobCreateDuraiton >= mobCreateCooltime)
        {
            mobCreateDuraiton -= mobCreateCooltime;
            mobCreateCount++;
            mobCreateCooltime = Mathf.Max(mobCreateCooltime - mobCreateRemoveValue, mobCreateMinValue);
            if (mobCreateCount > mobCreatePatternFirst)
            {
                mobCreatePatternFirstDuration++;
            }
            if (mobCreateCount > mobCreatePatternSecond)
            {
                mobCreatePatternSecondDuration++;
            }
            if (mobCreatePatternFirstDuration >= mobCreatePatternFirstCooltime)
            {
                mobCreatePatternFirstDuration -= mobCreatePatternFirstCooltime;
                for (int i = 0; i < 3; i++)
                {
                    Mob mob = PoolManager.Instance.Init(mobOrigin.gameObject).GetComponent<Mob>();
                    if (!mobList.Contains(mob))
                        mobList.Add(mob);
                    mob.transform.position = Quaternion.Euler(0, 0, i * 120) * Vector3.up * 4;
                    mob.bounce = 2;
                    mob.rigid.velocity = Vector2.zero;
                    mob.rigid.AddForce(((Player.Instance.transform.position - mob.transform.position).normalized) * mob.speed);
                }
            }
            else if (mobCreatePatternSecondDuration >= mobCreatePatternSecondCooltime)
            {
                mobCreatePatternSecondDuration -= mobCreatePatternSecondCooltime;
                for (int i = 0; i < 6; i++)
                {
                    Mob mob = PoolManager.Instance.Init(mobOrigin.gameObject).GetComponent<Mob>();
                    if (!mobList.Contains(mob))
                        mobList.Add(mob);
                    mob.transform.position = Quaternion.Euler(0, 0, i * 60) * Vector3.up * 4;
                    mob.bounce = 2;
                    mob.rigid.velocity = Vector2.zero;
                    mob.rigid.AddForce(((Player.Instance.transform.position - mob.transform.position).normalized) * mob.speed);
                }
            }
            else
            {
                Mob mob = PoolManager.Instance.Init(mobOrigin.gameObject).GetComponent<Mob>();
                if (!mobList.Contains(mob))
                    mobList.Add(mob);
                mob.transform.position = Random.insideUnitCircle.normalized * 4f;
                mob.bounce = Random.Range(1, 6);
                mob.rigid.velocity = Vector2.zero;
                mob.rigid.AddForce(((Player.Instance.transform.position - mob.transform.position).normalized) * mob.speed);
            }
        }
    }

    protected void GameOver()
    {
        Time.timeScale = 0;
        isGaming = true;
        gameOverParent.SetActive(true);
        int timerInt = (int)timer;
        gameOverTimer.text = "버틴 시간 :" + (timerInt / 60).ToString("D2") + " : " + (timerInt % 60).ToString("D2");
        if (SaveManager.Instance.saveData.maxTimer < timer)
        {
            SaveManager.Instance.saveData.maxTimer = timer;
            gameOverTimer.text += "\n<#FFEE00>신기록!";
        }
    }
    protected void UpdateAbiltiyImage(float deltaTime)
    {
        if (baseDuration > 0)
        {
            baseDuration -= deltaTime;
            baseAbility.fillAmount = 1 - (baseDuration / baseCooltime);
        }
        if (goyuDuration > 0)
        {
            goyuDuration -= deltaTime;
            goyuAbility.fillAmount = 1 - (goyuDuration / Player.Instance.goyuCooltime);
        }
    }

    public void BaseAbility()
    {
        if (!isGaming) return;

        if (baseDuration <= 0)
        {
            baseDuration = baseCooltime;
            Player.Instance.Dash();
        }
    }

    public void GoyuAbility()
    {
        if (!isGaming) return;

        if (goyuDuration <= 0)
        {
            characterImage.DOKill();
            textImage.DOKill();
            baseImage.DOKill();
            outlineImage.DOKill();
            characterImage.rectTransform.DOKill();
            textImage.rectTransform.DOKill();
            baseImage.rectTransform.DOKill();
            outlineImage.rectTransform.DOKill();

            isGaming = false;
            Time.timeScale = 0;
            characterImage.sprite = characterSprites[(int)character];
            textImage.sprite = textSprites[(int)character];

            textImage.color = Color.white;
            textImage.rectTransform.anchoredPosition = new Vector2(108, -864);

            characterImage.color = Color.white;
            characterImage.rectTransform.anchoredPosition = new Vector2(-108, -540);

            outlineImage.color = new Color(1, 1, 1, 0);
            outlineImage.DOFade(1, 0.1f).SetUpdate(true);

            baseImage.gameObject.SetActive(true);
            baseImage.color = new Color(1, 1, 1, 0);
            baseImage.rectTransform.anchoredPosition = Vector2.zero;
            baseImage.rectTransform.localScale = Vector3.one;
            baseImage.rectTransform.DOScale(Vector3.one * 1.2f, 0.2f).SetUpdate(true);
            baseImage.DOFade(1, 0.1f).SetUpdate(true).OnComplete(() =>
            {
                baseImage.rectTransform.DOAnchorPos(new Vector2(20, 60), 1.8f).SetUpdate(true).OnComplete(() =>
                {
                    float fadeOutTime = 0.1f;
                    baseImage.rectTransform.DOScale(Vector3.one * 2, fadeOutTime * 2).SetUpdate(true);
                    textImage.DOFade(0, fadeOutTime).SetUpdate(true);
                    characterImage.DOFade(0, fadeOutTime).SetUpdate(true);
                    outlineImage.DOFade(0, fadeOutTime).SetUpdate(true);
                    baseImage.DOFade(0, fadeOutTime).SetUpdate(true).OnComplete(() =>
                    {
                        Time.timeScale = 1;
                        baseImage.gameObject.SetActive(false);
                        goyuDuration = Player.Instance.goyuCooltime;
                        Player.Instance.Goyu();
                        isGaming = true;
                    });
                });
                textImage.rectTransform.DOAnchorPos(new Vector2(0, -108), 0.3f).SetUpdate(true);
                characterImage.rectTransform.DOAnchorPos(Vector2.zero, 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    characterImage.rectTransform.DOAnchorPos(Vector2.zero + new Vector2(10, 20), 3).SetUpdate(true);
                    textImage.rectTransform.DOAnchorPos(new Vector2(10, 128), 3).SetRelative().SetUpdate(true);
                    textImage.rectTransform.DOScale(Vector3.one * 1.15f, 3).SetUpdate(true);
                    textImage.rectTransform.DOShakePosition(3, 20f, 10, 90, false, false).SetUpdate(true);
                });
            });
        }
    }

    protected void UpdateTimer(float deltaTime)
    {
        timer += deltaTime;
        int timerInt = (int)timer;
        timerText.text = (timerInt / 60).ToString("D2") + " : " + (timerInt % 60).ToString("D2");
    }

    public void Restart()
    {
        Reset();
    }

    public void Back()
    {
        SceneManager.LoadScene("Title");
    }

    protected void Reset()
    {
        isGaming = true;
        timer = 0;
        UpdateTimer(0);
        baseDuration = 0;
        baseAbility.fillAmount = 1;
        goyuAbility.fillAmount = 1;
        goyuDuration = 0;
        mobCreateCount = 0;
        mobCreateDuraiton = 0;
        mobCreatePatternFirstDuration = 0;
        mobCreatePatternSecondDuration = 0;
        mobCreateCooltime = 2;
        hitEffect.gameObject.SetActive(false);
        mobList.Clear();
        itemCreateDuration = 0;
        hp = 1;
        gameOverParent.SetActive(false);
        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.shieldDuration = 0;
        Player.Instance.dashParticle.gameObject.SetActive(false);
        Player.Instance.shieldCount = 0;
        Time.timeScale = 1;
        PoolManager.Instance.DisableAll();
    }
}
