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
                Description_Txt.text = "캐릭터 1의 설명입니다.";
                break;
           // case 1:
             //   Description_Txt.text = "캐릭터 2의 설명입니다.";
             //   break;
            //case 2:
             //   Description_Txt.text = "캐릭터 3의 설명입니다.";
             //   break;

        }
    }
}
