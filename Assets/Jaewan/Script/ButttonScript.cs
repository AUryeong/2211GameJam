using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButttonScript : MonoBehaviour
{
    // 0번 : 타이틀
    // 1번 : 캐릭터 선택 창
    // 2번 : 인 게임 

    //선택창 가기
    public void GO_Select_Scene() 
    {
        SceneManager.LoadScene(1);
    }
    // 인게임 들어가기
    public void Go_Ingame() 
    {
        SceneManager.LoadScene(2);
    }
    // 타이틀 가기
    public void Go_Title() 
    {
        SceneManager.LoadScene(0);
    }

    [Header("Setting")]
    //설정창 열기
    [SerializeField] GameObject Setting_Panel;
    public void Setting_On() 
    {
        Setting_Panel.SetActive(true) ;
    }
    //설정창 닫기
    public void Setting_Off() 
    {
        Setting_Panel.SetActive(false);
    }
    [Header("Developers")]
    //개발자들 열기
    [SerializeField] GameObject developers;
    public void Developers_On() 
    {
        developers.SetActive(true);
    }
    public void Developers_Off() 
    {
        developers.SetActive(false);
    }
    [Header("Description")]
    [SerializeField] GameObject Description;
    public void Description_On() 
    {
        Description.SetActive(true);
    }
    public void Description_Off() 
    {
        Description.SetActive(false);
    }
    public void Game_Quit() 
    {
        Application.Quit();
    }
}
