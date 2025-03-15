using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2); // sceneManager scripti
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


