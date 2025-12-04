using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용을 위해 필수
using System; // Math.Round 사용을 위해 필요

public class SettingsUI : MonoBehaviour
{
    [Header("Controls")]
    public Toggle fullScreenToggle;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TMP_Dropdown fpsDropdown;

    [Header("Value Labels")]
    // 슬라이더 옆에 있는 텍스트 오브젝트를 여기에 연결하세요
    public TextMeshProUGUI bgmValueText;
    public TextMeshProUGUI sfxValueText;

    private readonly int[] fpsOptions = { 30, 60, 144, -1 };

    void Start()
    {
        fullScreenToggle.onValueChanged.AddListener(OnFullScreenChanged);
        bgmSlider.onValueChanged.AddListener(OnBgmChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        fpsDropdown.onValueChanged.AddListener(OnFpsChanged);

        SettingsManager.Instance.OnLoadComplete += RefreshUI;

        RefreshUI();
    }

    void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OnLoadComplete -= RefreshUI;
        }
    }
    public void RefreshUI()
    {
        var settings = SettingsManager.Instance.settings;

        // 1. 값 반영 시 OnValueChanged 이벤트가 또 발생해서 무한루프나 중복 실행되는 것을 방지
        // (간단한 로직에서는 괜찮지만, 복잡해지면 리스너를 잠시 끄는 게 좋습니다)

        // 토글 갱신
        fullScreenToggle.isOn = settings.isFullScreen;

        // 슬라이더 갱신
        bgmSlider.value = settings.bgm;
        sfxSlider.value = settings.sfx;

        // 텍스트 갱신
        UpdateVolumeText(bgmValueText, settings.bgm);
        UpdateVolumeText(sfxValueText, settings.sfx);

        // Dropdown 갱신
        int fpsIndex = 0;
        for (int i = 0; i < fpsOptions.Length; i++)
        {
            if (fpsOptions[i] == settings.fps) { fpsIndex = i; break; }
        }
        fpsDropdown.value = fpsIndex;
        fpsDropdown.RefreshShownValue();
    }

    // --- 이벤트 핸들러 ---

    public void OnFullScreenChanged(bool isFull)
    {
        SettingsManager.Instance.settings.isFullScreen = isFull;
    }

    public void OnFpsChanged(int index)
    {
        SettingsManager.Instance.settings.fps = fpsOptions[index];
    }

    public void OnBgmChanged(float value)
    {
        SettingsManager.Instance.settings.bgm = value;
        UpdateVolumeText(bgmValueText, SettingsManager.Instance.BGM);
    }

    public void OnSfxChanged(float value)
    {
        SettingsManager.Instance.settings.sfx = value;
        UpdateVolumeText(sfxValueText, SettingsManager.Instance.SFX);
    }

    private void UpdateVolumeText(TextMeshProUGUI label, float value)
    {
        if (label != null)
        {
            label.text = (value * 100).ToString("F0");
        }
    }
}