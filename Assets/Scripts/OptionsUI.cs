using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OptionsUI : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager MusicManager;
    private TextMeshProUGUI soundVolumeText,musicVolumeText;
    private void Awake()
    {
        soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();
        transform.Find("soundPlusBtn").GetComponent<Button>().onClick.AddListener( () => {
            soundManager.IncreaseVolume();
            UpdateSoundText();
        });
        transform.Find("soundMinusBtn").GetComponent<Button>().onClick.AddListener( () => {
            soundManager.DecreaseVolume();
            UpdateSoundText();
        });
        transform.Find("musicPlusBtn").GetComponent<Button>().onClick.AddListener( () => {
            MusicManager.IncreaseVolume();
            UpdateMusicText();
        });
        transform.Find("musicMinusBtn").GetComponent<Button>().onClick.AddListener( () => {
            MusicManager.DecreaseVolume();
            UpdateMusicText();
        });
        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener( () => {
            Time.timeScale = 1f;
            GameSceneManager.LoadMainMenu();
        });
    }
    private void Start()
    {
        UpdateSoundText();
        gameObject.SetActive(false);
    }
    private void UpdateSoundText()
    {
        soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
    }
    private void UpdateMusicText()
    {
        musicVolumeText.SetText(Mathf.RoundToInt(MusicManager.GetVolume() * 10).ToString());
    }
    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
