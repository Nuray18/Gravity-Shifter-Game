using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGameButton : MonoBehaviour
{
    public GameObject backupCamera; // Yedek kamera referansı (Inspector'dan atanacak)
    public void RestartGameFromBeginning()
    {
        // Yedek kamerayı SceneLoader'a aktar
        if (backupCamera != null)
        {
            Debug.Log("Restart: Backup kamera atanıyor...");
            SceneLoader.backupCamera = backupCamera;
        }
        else
        {
            Debug.LogWarning("Restart: Backup kamera NULL!");
        }

        GameManager.instance.RestartGame();

        if (backupCamera != null)
        {
            Debug.Log("Restart: Backup kamera devre dışı bırakıldı.");
            backupCamera.SetActive(false);
        }
    }
}