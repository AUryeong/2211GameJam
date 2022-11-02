using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_chr : MonoBehaviour
{
    [SerializeField] public Chr chr;
    SpriteRenderer spriteRenderer;
    public Select_chr[] chars;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (DataManager.instance.sel_chr == chr) Onselect();
        else OnDeselect();
    }

    private void OnMouseUpAsButton()
    {
        DataManager.instance.sel_chr = chr;
        Onselect();
        for (int i =0; i < chars.Length; i++) 
        {
            if (chars[i] != this) chars[i].OnDeselect();
        }
    }
    void Onselect() 
    {
        Color color_w = Color.white;
        spriteRenderer.color = color_w;
    }
    void OnDeselect() 
    {
        Color color_b = Color.black;
        spriteRenderer.color = color_b;
    }
   
}
