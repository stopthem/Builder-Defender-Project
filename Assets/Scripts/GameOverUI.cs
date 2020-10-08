using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance {get; private set;}
    private void Awake()
    {
        Instance = this;
        transform.Find("retryButton").GetComponent<Button>().onClick.AddListener( () => {
            GameSceneManager.LoadGameScene();
        });
        transform.Find("mainMenuButton").GetComponent<Button>().onClick.AddListener( () => {
            GameSceneManager.LoadMainMenu();
        });
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        transform.Find("survivedText").GetComponent<TextMeshProUGUI>().SetText("You Survived " + EnemyWaveManager.Instance.GetWaveNumber() + " Waves!");
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
