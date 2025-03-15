using UnityEngine;

public class LeaveGameButton : MonoBehaviour
{
    public GameObject backupCamera;

    public void LeaveToMainMenu()
    {
        // camera
        if (backupCamera != null)
        {
            SceneLoader.backupCamera = backupCamera;
        }
        else
        {
            Debug.LogWarning("Restart: Backup kamera NULL!");
        }

        // Scene load
        if (SceneLoader.instance != null)
        {
            SceneLoader.LoadScene(SceneNames.MainMenu);
        }
        else
        {
            Debug.LogError("SceneLoader instance is NULL! Make sure SceneLoader is in the scene.");
        }

        // camera
        if (backupCamera != null)
        {
            backupCamera.SetActive(false);
        }
    }
}
