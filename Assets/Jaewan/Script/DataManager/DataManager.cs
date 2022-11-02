using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Chr 
{ 
    chr_1, chr_2 , chr_3
}
public class DataManager : MonoBehaviour
{
    public Chr sel_chr;
    public static DataManager instance;

    [SerializeField] 
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
    }
  
}
