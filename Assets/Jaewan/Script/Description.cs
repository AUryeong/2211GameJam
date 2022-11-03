using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    [SerializeField] List<string> Description_Text = new List<string>();
    [SerializeField] Text Description_Txt;
    private void Update()
    {
        Description_Set();
    }
    void Description_Set()
    {
        int i = (int)DataManager.instance.sel_chr;
        switch (i)
        {
            case 0:
                Description_Txt.text = Description_Text[0];
                break;
            case 1:
                Description_Txt.text = Description_Text[1];
                break;
            case 2:
                Description_Txt.text = Description_Text[2];
                break;
        }
    }
}
