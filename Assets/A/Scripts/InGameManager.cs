using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameManager : Singleton<InGameManager>
{
    public VariableJoystick joystick;
    [SerializeField] TextMeshProUGUI timerText;

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
    protected float mobCreateCooltime = 2f;
    protected float mobCreateDuraiton = 0;
    protected int mobCreateCount = 0;

    protected int mobCreatePatternFirst = 12;
    protected int mobCreatePatternFirstCooltime = 7;
    protected int mobCreatePatternFirstDuration = 0;

    protected int mobCreatePatternSecond = 25;
    protected int mobCreatePatternSecondCooltime = 15;
    protected int mobCreatePatternSecondDuration = 0;

    protected float mobCreateRemoveValue = 0.005f;
    protected float mobCreateMinValue = 0.4f;
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
        hp = 1;
        gameOverParent.SetActive(false);
        isGaming = true;
    }

    protected void Update()
    {
        if (!isGaming)
            return;
        float deltaTime = Time.deltaTime;
        UpdateTimer(deltaTime);
        UpdateAbiltiyImage(deltaTime);
        CheckCreateMob(deltaTime);
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
                for (int i = 0; i < 3; i++)
                {
                    Mob mob = PoolManager.Instance.Init(mobOrigin.gameObject).GetComponent<Mob>();
                    mob.transform.position = Quaternion.Euler(0, 0, i * 120) * Vector3.up * 4;
                    mob.bounce = 1;
                    mob.rigid.velocity = Vector2.zero;
                }
            }
            else if (mobCreatePatternFirstDuration >= mobCreatePatternFirstCooltime)
            {
                for (int i = 0; i < 6; i++)
                {
                    Mob mob = PoolManager.Instance.Init(mobOrigin.gameObject).GetComponent<Mob>();
                    mob.transform.position = Quaternion.Euler(0, 0, i * 60) * Vector3.up * 4;
                    mob.bounce = 1;
                    mob.rigid.velocity = Vector2.zero;
                }
            }
            else
            {
                Mob mob = PoolManager.Instance.Init(mobOrigin.gameObject).GetComponent<Mob>();
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
            goyuDuration = Player.Instance.goyuCooltime;
            Player.Instance.Goyu();
        }
    }

    protected void UpdateTimer(float deltaTime)
    {
        timer += deltaTime;
        int timerInt = (int)timer;
        timerText.text = (timerInt / 60).ToString("D2") + " : " + (timerInt % 60).ToString("D2");
    }
}
