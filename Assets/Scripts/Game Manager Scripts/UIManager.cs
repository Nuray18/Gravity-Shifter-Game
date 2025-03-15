using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // UI sahne değiştiğinde kaybolmaz
        }
        else
        {
            Destroy(gameObject); // Eğer sahnede zaten bir UI varsa, yenisini yok et
        }
    }


}
