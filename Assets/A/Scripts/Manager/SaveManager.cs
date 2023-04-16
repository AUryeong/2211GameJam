using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SaveData
{
    public float maxTimer = 0;
    public int sfxVolume = 100;
    public int bgmVolume = 100;
    public Character character = Character.Chunja;
}

public class SaveManager : Singleton<SaveManager>
{
    SaveData _saveData;
    public SaveData saveData
    {
        get
        {
            if (_saveData == null)
                LoadGameData();
            return _saveData;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        SaveGameData();
    }

    protected override void Awake()
    {
        base.Awake();
        if (this == Instance)
        {
            LoadGameData();
            DontDestroyOnLoad(gameObject);
            CharacterManager.Instance.selectCharacter = saveData.character;
        }
    }

    private void LoadGameData()
    {
        string s = PlayerPrefs.GetString("SaveData", "none");
        if (s == "none")
            _saveData = new SaveData();
        else
            _saveData = JsonUtility.FromJson<SaveData>(s);
    }

    private void SaveGameData()
    {
        saveData.character = CharacterManager.Instance.selectCharacter;
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(saveData));
    }
}
