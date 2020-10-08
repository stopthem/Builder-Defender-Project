using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
