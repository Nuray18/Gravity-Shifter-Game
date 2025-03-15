using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public static PlayerManager Instance
    {
        get { return instance; }
    }
    public GameObject player;
    public Vector3 startingPosition; // starting position when game restarts or starts

    public bool isRestarting = false;
    
    private void Awake()
    {
        // Singleton yapısı
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // PlayerManager, sahne geçişlerinde yok edilmez
            SceneManager.sceneLoaded += OnSceneLoaded; // Sahne yüklendiğinde çağrılacak fonksiyon
        }
        else
        {
            Destroy(gameObject); // Aynı PlayerManager birden fazla sahnede varsa, yalnızca biri kalır.
        }
    }

    //void Update()
    //{
    //    Debug.Log("Player pozisyonu: " + player.transform.position);
    //}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isRestarting = false; // Restart tamamlandı, tekrar sahne değiştirirken çalışmasın
    }


    // bu playeri yeniden baslatir (restart veya level sceneleri gecisleri icin)
    public void RestartPlayer()
    {
        if (player != null)
        {
            Debug.Log("Restarting player...");

            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ResetVelocity(); // Player scriptindeki funcu cagirir
                playerScript.ResetGravity();
            }

            // 1. Sağlık sistemini sıfırla
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
            }

            // 2. CharacterController ve Rigidbody'yi devre dışı bırak
            CharacterController controller = player.GetComponent<CharacterController>();
            Rigidbody rb = player.GetComponent<Rigidbody>();

            if (controller != null) controller.enabled = false;
            //if (rb != null) rb.isKinematic = true; // Fizik motorunu devre dışı bırak

            // 3. Pozisyonu ve rotasyonu sıfırla
            player.transform.position = startingPosition;
            player.transform.rotation = Quaternion.identity;

            Debug.Log("Yeni pozisyon: " + player.transform.position);

            if (!IsGrounded(player))
            {
                Debug.LogWarning("Player zeminde değil, aşağı çekiliyor!");
                player.transform.position += Vector3.down * 0.5f;
            }

            // 4. CharacterController ve Rigidbody'yi tekrar etkinleştir
            if (controller != null) controller.enabled = true;
            //if (rb != null) rb.isKinematic = false; // Fizik motorunu tekrar aç

            Debug.Log("Player sıfırlandı.");
        }
        else
        {
            Debug.LogError("Player objesi bulunamadı! Restart başarısız.");
        }
    }

    private bool IsGrounded(GameObject obj)
    {
        return Physics.Raycast(obj.transform.position, Vector3.down, 1.1f);
    }

    // Oyuncuyu Don't Destroy'dan çıkar ve sil (main menuye donerken cikartilir)
    public void RemovePlayerFromDontDestroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Hafıza sızıntısını önlemek için event'ten çık
    }

}
