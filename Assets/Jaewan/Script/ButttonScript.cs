using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButttonScript : MonoBehaviour
{
    // 0�� : Ÿ��Ʋ
    // 1�� : ĳ���� ���� â
    // 2�� : �� ���� 

    //����â ����
    public void GO_Select_Scene() 
    {
        SceneManager.LoadScene(1);
    }
    // �ΰ��� ����
    public void Go_Ingame() 
    {
        SceneManager.LoadScene(2);
    }
    // Ÿ��Ʋ ����
    public void Go_Title() 
    {
        SceneManager.LoadScene(0);
    }

    [Header("Setting")]
    //����â ����
    [SerializeField] GameObject Setting_Panel;
    public void Setting_On() 
    {
        Setting_Panel.SetActive(true) ;
    }
    //����â �ݱ�
    public void Setting_Off() 
    {
        Setting_Panel.SetActive(false);
    }
    [Header("Developers")]
    //�����ڵ� ����
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
