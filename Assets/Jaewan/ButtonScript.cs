using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void Go_Ingame() 
    {
        SceneManager.LoadScene(2);
    }
    public void Go_Title() 
    {
        SceneManager.LoadScene(0);
    }
    public void Go_Select() 
    {
        SceneManager.LoadScene(1);
    }
}
