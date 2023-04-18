using System;
using System.Collections.Generic;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Character
{
    Chunja,
    Yeonghee
}

public class GameManager : Singleton<GameManager>
{
    public Character selectCharacter;
    public bool isTestMode;
    public float time;

    
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = Application.platform == RuntimePlatform.Android ? 30 : 120;
            SetResolution(Camera.main);
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                SetResolution(Camera.main);
            };

            var requestConfiguration = new RequestConfiguration
               .Builder()
               .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
               .build();

            MobileAds.SetRequestConfiguration(requestConfiguration);

            LoadFrontAd();

            PlayGamesPlatform.Activate();
            PlayGamesPlatform.DebugLogEnabled = true;
            Social.localUser.Authenticate((success) =>
            {
                Debug.Log(success + "앨리스");
            });

        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SetResolution(Camera.main);
    }

    private void SetResolution(Camera changeCamera)
    {
        if (changeCamera == null) return;
        int setWidth = 1920;
        int setHeight = 1080;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        float screenMultiplier = (float)setWidth / setHeight;
        float deviceMultiplier = (float)deviceWidth / deviceHeight;

        if (screenMultiplier < deviceMultiplier)
        {
            float newWidth = screenMultiplier / deviceMultiplier;
            changeCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = deviceMultiplier / screenMultiplier;
            changeCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";//테스트용 애드몹 아이디
    const string frontID = "ca-app-pub-5595998256695432/8584678748";//출시 후 사용 애드몹 아이디
    InterstitialAd frontAd;

    public void LoadFrontAd()
    {
        InterstitialAd.Load(isTestMode ? frontTestID : frontID, GetAdRequest(), (front, b) =>
        {
            frontAd = front;
#if UNITY_EDITOR
            Debug.Log("광고 로딩 완료" + b);
#endif
        });
    }

    public void ShowFrontAd()
    {
        if (time > 0 && Time.unscaledTime - time < 150) return;

        time = Time.unscaledTime;
        frontAd.Show();
        LoadFrontAd(); // 재로딩
    }
}