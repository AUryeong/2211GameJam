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
    protected int _hp = 0;

    [Header("스킬")]
    [SerializeField] Image goyuAbility;
    protected float goyuDuration = 0;
    [SerializeField] Image baseAbility;
    protected float baseDuration = 0;
    protected float baseCooltime = 5;

    [Header("게임 오버")]
    [SerializeField] GameObject GameOver_Panel;
    [SerializeField] TextMeshProUGUI Time_txt;

    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            for (int i = 0; i < heartImage.Count; i++)
                heartImage[i].gameObject.SetActive(_hp > i);
        }
    }

    protected float timer = 0;

    protected void Start()
    {
        hp = 1;
        GameOver_Panel.SetActive(false);
    }

    protected void Update()
    {
        float deltaTime = Time.deltaTime;
        UpdateTimer(deltaTime);
        UpdateAbiltiyImage(deltaTime);
    }
    void GameOver() 
    {
        if (hp <= 0) 
        {
            GameOver_Panel.SetActive(true);
            int timerInt = (int)timer;
            Time_txt.text = "버틴 시간 :" + (timerInt / 60).ToString("D2") + " : " + (timerInt % 60).ToString("D2");
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
        if (baseDuration <= 0)
        {
            baseDuration = baseCooltime;
            Player.Instance.Dash();
        }
    }

    public void GoyuAbility()
    {
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
