using System;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CatScene");
    }
}