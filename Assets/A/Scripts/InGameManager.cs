using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameManager : Singleton<InGameManager>
{
    public VariableJoystick joystick;
    [SerializeField] TextMeshProUGUI timerText;

    [Header("¸ñ¼û")]
    [SerializeField] HorizontalLayoutGroup layoutGroup;
    [SerializeField] List<Image> heartImage;

    [Header("½ºÅ³")]
    [SerializeField] Image goyuAbility;
    protected float goyuDuration = 0;
    [SerializeField] Image baseAbility;
    protected float baseDuration = 0;
    protected float baseCooltime = 5;

    protected int _hp = 0;
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
    }

    protected void Update()
    {
        float deltaTime = Time.deltaTime;
        UpdateTimer(deltaTime);
        UpdateAbiltiyImage(deltaTime);
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
