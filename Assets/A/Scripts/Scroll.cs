using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Scroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("이름")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] List<string> names = new List<string>();
    [Header("설명")]
    [SerializeField] TextMeshProUGUI loreText;
    [SerializeField][TextArea(2, 9)] List<string> lores = new List<string>();
    [Header("애니메이션용")]
    [SerializeField] TextMeshProUGUI scrollText;
    [SerializeField] Image rightPanel;
    [SerializeField] Image rightButton;
    [SerializeField] Image exitButton;
    [SerializeField] SpriteRenderer shadow;
    [SerializeField] SpriteRenderer shadow2;

    public bool isControllable = false;
    Vector2 beginPos;
    float centerPos = -4.31f;
    [SerializeField] List<SpriteRenderer> characters;

    void Awake()
    {
        CharacterManager.Instance.selectCharacter = SaveManager.Instance.saveData.character;
        isControllable = false;
        UpdateCharacter(true);
        GameManager.Instance.ShowAd();


        foreach (var character in characters)
            character.color = new Color(1, 1, 1, 0);
        shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, 0);
        shadow2.color = new Color(shadow2.color.r, shadow2.color.g, shadow2.color.b, 0);

        scrollText.rectTransform.anchoredPosition += new Vector2(0, 200);
        rightPanel.rectTransform.anchoredPosition += new Vector2(700, 0);
        rightButton.rectTransform.anchoredPosition += new Vector2(700, 0);
        exitButton.rectTransform.anchoredPosition += new Vector2(-700, 0);

        scrollText.rectTransform.DOAnchorPosY(-200, 1.5f).SetEase(Ease.OutBack).SetRelative().OnUpdate(() =>
        {
            if (Input.GetMouseButtonDown(0))
                scrollText.rectTransform.DOKill(true);
        });
        rightPanel.rectTransform.DOAnchorPosX(-700, 1.5f).SetEase(Ease.OutBack).SetRelative().OnUpdate(() =>
        {
            if (Input.GetMouseButtonDown(0))
                rightPanel.rectTransform.DOKill(true);
        });
        rightButton.rectTransform.DOAnchorPosX(-700, 1.5f).SetEase(Ease.OutBack).SetRelative().OnUpdate(() =>
        {
            if (Input.GetMouseButtonDown(0))
                rightButton.rectTransform.DOKill(true);
        });
        foreach (var character in characters)
            character.DOFade(1, 0.5f).SetDelay(0.5f);
        shadow.DOFade(0.6f, 0.5f).SetDelay(0.5f);
        shadow2.DOFade(0.6f, 0.5f).SetDelay(0.5f);
        exitButton.rectTransform.DOAnchorPosX(700, 1.5f).SetEase(Ease.OutBack).SetRelative().OnComplete(() =>
        {
            isControllable = true;
        }).OnUpdate(() =>
        {
            if (Input.GetMouseButtonDown(0))
                exitButton.rectTransform.DOKill(true);
        });
    }

    public void GameStart()
    {
        if (isControllable)
        {
            SoundManager.Instance.PlaySound("button_click", SoundType.SE);
            isControllable = false;
            scrollText.rectTransform.DOAnchorPosY(200, 1.5f).SetEase(Ease.OutBack).SetRelative();
            rightPanel.rectTransform.DOAnchorPosX(700, 1.5f).SetEase(Ease.OutBack).SetRelative();
            rightButton.rectTransform.DOAnchorPosX(700, 1.5f).SetEase(Ease.OutBack).SetRelative();
            foreach (var character in characters)
                character.DOFade(0, 0.5f).SetDelay(0.5f);
            shadow.DOFade(0, 0.5f).SetDelay(0.5f);
            shadow2.DOFade(0, 0.5f).SetDelay(0.5f);
            exitButton.rectTransform.DOAnchorPosX(-700, 1.5f).SetEase(Ease.OutBack).SetRelative().OnComplete(() =>
            {
                SceneManager.LoadScene("InGame");
            });
        }
    }
    public void Back()
    {
        if (isControllable)
        {
            SoundManager.Instance.PlaySound("button_click", SoundType.SE);
            isControllable = false;
            scrollText.rectTransform.DOAnchorPosY(200, 1.5f).SetEase(Ease.OutBack).SetRelative();
            rightPanel.rectTransform.DOAnchorPosX(700, 1.5f).SetEase(Ease.OutBack).SetRelative();
            rightButton.rectTransform.DOAnchorPosX(700, 1.5f).SetEase(Ease.OutBack).SetRelative();
            foreach (var character in characters)
                character.DOFade(0, 0.5f).SetDelay(0.5f);
            shadow.DOFade(0, 0.5f).SetDelay(0.5f);
            shadow2.DOFade(0, 0.5f).SetDelay(0.5f);
            exitButton.rectTransform.DOAnchorPosX(-700, 1.5f).SetEase(Ease.OutBack).SetRelative().OnComplete(() =>
            {
                SceneManager.LoadScene("Title");
            });
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isControllable)
            return;
        SoundManager.Instance.PlaySound("button_click", SoundType.SE);
        foreach (var character in characters)
            character.transform.DOKill();
        beginPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isControllable)
            return;
        float moveX = (Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(beginPos)).x;
        foreach (var character in characters)
        {
            character.transform.position += new Vector3(moveX, 0, 0);
        }
        beginPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isControllable)
            return;
        float spriteRange = 9999;
        float moveX = 0;
        SpriteRenderer rangeChar = null;
        foreach (var character in characters)
        {
            float range = Mathf.Abs(centerPos - character.transform.position.x);
            if (range < spriteRange)
            {
                spriteRange = range;
                moveX = centerPos - character.transform.position.x;
                rangeChar = character;
            }
        }
        foreach (var character in characters)
            character.transform.DOMoveX(moveX, 0.6f).SetRelative();

        int index = characters.IndexOf(rangeChar);
        CharacterManager.Instance.selectCharacter = (Character)index;
        UpdateCharacter();
    }

    protected void UpdateCharacter(bool isSkipping = false)
    {
        int index = (int)CharacterManager.Instance.selectCharacter;
        if (isSkipping)
        {
            float range = centerPos - characters[index].transform.position.x;
            foreach (var character in characters)
                character.transform.position += new Vector3(range, 0, 0);
        }
        nameText.text = names[index];
        loreText.text = lores[index];
    }
}
