using UnityEngine;

public class BackupCameraManager : MonoBehaviour
{
    private static BackupCameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Kamera sahneler arasında yok olmasın
        }
        else
        {
            Destroy(gameObject); // Eğer zaten varsa, yeni oluşanı sil
        }
    }
}
