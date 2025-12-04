using UnityEngine;
using System.IO;
using TMPro;
using System;

public class SettingsManager : MonoBehaviour
{
    public TMP_Text debug;
    public event Action OnLoadComplete;
    public static SettingsManager Instance;

    public Settings settings;
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    private string filePath;
    public bool IsFullScreen
    {
        get => settings.isFullScreen;
        set
        {
            settings.isFullScreen = value;
            Screen.fullScreen = value;
        }
    }

    public float BGM
    {
        get => settings.bgm;
        set
        {
            settings.bgm = value;
            BGMSource.volume = value;
        }
    }

    public float SFX
    {
        get => settings.sfx;
        set
        {
            settings.sfx = value;
            SFXSource.volume = value;
        }
    }

    public int FPS
    {
        get => settings.fps;
        set
        {
            settings.fps = value;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = value;
        }
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, "game_settings.json");

        LoadSettings();
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, json);
        ApplyAllSettings();
        debug.text = $"설정 저장 완료: {filePath}";
    }

    public void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<Settings>(json);
        }
        else
        {
            settings = new Settings();
            SaveSettings();
        }
        ApplyAllSettings();
        OnLoadComplete?.Invoke();
    }

    public void ApplyAllSettings()
    {
        IsFullScreen = settings.isFullScreen;
        FPS = settings.fps;
        BGM = settings.bgm;
        SFX = settings.sfx;

        debug.text = $"모든 설정 적용 완료";
    }
}