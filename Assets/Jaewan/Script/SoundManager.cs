using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource sdmanager;
    void Start()
    {
        sdmanager = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }
    void Title_Bgm() 
    { 
    
    }
}
