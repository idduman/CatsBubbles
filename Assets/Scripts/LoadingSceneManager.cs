using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CatScene");
    }
}