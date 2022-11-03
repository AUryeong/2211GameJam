using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Reflection;

public class Scroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("이름")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] List<string> names = new List<string>();
    [Header("설명")]
    [SerializeField] TextMeshProUGUI loreText;
    [SerializeField][TextArea(2, 9)] List<string> lores = new List<string>();
    Vector2 beginPos;
    float centerPos = -4.31f;
    [SerializeField] List<SpriteRenderer> characters;

    void Awake()
    {
        CharacterManager.Instance.selectCharacter = SaveManager.Instance.saveData.character;
        UpdateCharacter(true);
    }

    public void GameStart()
    {
        SceneManager.LoadScene("InGame");
    }
    public void Back()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        foreach (var character in characters)
            character.transform.DOKill();
        beginPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float moveX = (Camera.main.ScreenToWorldPoint(eventData.position) - Camera.main.ScreenToWorldPoint(beginPos)).x;
        foreach (var character in characters)
        {
            character.transform.position += new Vector3(moveX, 0, 0);
        }
        beginPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
