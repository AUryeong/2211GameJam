using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    [SerializeField] Text Description_Txt;
    void Description_Set()
    {
        switch (DataManager.instance.sel_chr)
        {
            case 0:
                Description_Txt.text = "ĳ���� 1�� �����Դϴ�.";
                break;
           // case 1:
             //   Description_Txt.text = "ĳ���� 2�� �����Դϴ�.";
             //   break;
            //case 2:
             //   Description_Txt.text = "ĳ���� 3�� �����Դϴ�.";
             //   break;

        }
    }
}
