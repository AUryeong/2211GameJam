using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManager : Singleton<TitleManager>
{
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] Image maxScore;
    [SerializeField] RectTransform buttonVertical;
    [SerializeField] Image title;
    [SerializeField] Image settingButton;

    protected bool isControllable = false;

    private void Start()
    {
        SoundManager.Instance.PlaySound("bgm", SoundType.BGM);

        int timerInt = (int)SaveManager.Instance.saveData.maxTimer;
        bestScoreText.text = (timerInt / 60).ToString("D2") + " : " + (timerInt % 60).ToString("D2");
        sfxSlider.value = SaveManager.Instance.saveData.sfxVolume / 100f;
        bgmSlider.value = SaveManager.Instance.saveData.bgmVolume / 100f;
        uiFlip.isOn = SaveManager.Instance.saveData.uiFlip;

        maxScore.rectTransform.anchoredPosition += new Vector2(0, -400);
        buttonVertical.anchoredPosition += new Vector2(600, 0);
        title.rectTransform.anchoredPosition += new Vector2(-1100, 0);
        settingButton.rectTransform.anchoredPosition += new Vector2(0, 400);

        maxScore.rectTransform.DOAnchorPosY(400, 2).SetRelative();
        buttonVertical.DOAnchorPosX(-600, 2).SetEase(Ease.OutBack).SetRelative();
        title.rectTransform.DOAnchorPosX(1100, 2).SetEase(Ease.OutBack).SetRelative();
        settingButton.rectTransform.DOAnchorPosY(-400, 2).SetEase(Ease.OutBack).SetRelative().OnComplete(() =>
        {
            isControllable = true;
        });
    }

    // 0번 : 타이틀
    // 1번 : 캐릭터 선택 창
    // 2번 : 인 게임 

    [Header("Setting")]
    [SerializeField] GameObject Setting_Panel;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Toggle uiFlip;
    [Header("Developers")]
    [SerializeField] GameObject developers;
    [Header("Description")]
    [SerializeField] GameObject Description;
    // 인게임 들어가기
    public void Go_Ingame()
    {
        if (!isControllable)
            return;
        isControllable = false;

        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
        maxScore.rectTransform.DOAnchorPosY(-400, 2).SetRelative();
        buttonVertical.DOAnchorPosX(600, 2).SetEase(Ease.InBack).SetRelative();
        title.rectTransform.DOAnchorPosX(-1100, 2).SetEase(Ease.InBack).SetRelative();
        settingButton.rectTransform.DOAnchorPosY(400, 2).SetEase(Ease.InBack).SetRelative().OnComplete(() =>
        {
            SceneManager.LoadScene("CharacterSelect");
        });
    }

    public void Setting_On()
    {
        if (!isControllable)
            return;
        Setting_Panel.SetActive(true);
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }
    //설정창 닫기
    public void Setting_Off()
    {
        if (!isControllable)
            return;
        Setting_Panel.SetActive(false);
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }
    public void Developers_On()
    {
        if (!isControllable)
            return;
        Setting_Panel.SetActive(false);
        developers.SetActive(true);
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }
    public void Developers_Off()
    {
        if (!isControllable)
            return;
        developers.SetActive(false);
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }
    public void Description_On()
    {
        if (!isControllable)
            return;
        Description.SetActive(true);
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }
    public void Description_Off()
    {
        if (!isControllable)
            return;
        Description.SetActive(false);
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }
    public void Game_Quit()
    {
        if (!isControllable)
            return;
        Application.Quit();
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
    }

    public void SfxChange()
    {
        SoundManager.Instance.VolumeChange(SoundType.SE, sfxSlider.value);
        SaveManager.Instance.saveData.sfxVolume = Mathf.CeilToInt(sfxSlider.value * 100f);
    }
    public void BgmChange()
    {
        SoundManager.Instance.VolumeChange(SoundType.BGM, bgmSlider.value);
        SaveManager.Instance.saveData.bgmVolume = Mathf.CeilToInt(bgmSlider.value * 100f);
    }
    public void UIFlip()
    {
        SaveManager.Instance.saveData.uiFlip = uiFlip.isOn;
    }
}
