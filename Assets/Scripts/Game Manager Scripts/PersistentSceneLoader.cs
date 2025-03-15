using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneLoader : MonoBehaviour
{
    public static PersistentSceneLoader instance;
    public static bool isPersistentSceneLoaded = false; // Eklenen kontrol değişkeni
    // one time 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Bu GameObject ve bileşenleri sahne geçişlerinde kalır.
            SceneManager.sceneLoaded += OnSceneLoaded; // Sahne değiştiğinde çağrılacak

            StartCoroutine(EnsurePersistentSceneLoaded()); // Asenkron yükleme
        }
        else
        {
            // Zaten bir örnek varsa, bu fazladan olanı yok et.
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneNames.MainMenu && isPersistentSceneLoaded)
        {
            StartCoroutine(UnloadPersistentScene());
        }
    }

    public static IEnumerator EnsurePersistentSceneLoaded()
    {
        if (!isPersistentSceneLoaded || !SceneManager.GetSceneByName(SceneNames.PersistentScene).isLoaded)
        {
            yield return SceneManager.LoadSceneAsync(SceneNames.PersistentScene, LoadSceneMode.Additive);
            isPersistentSceneLoaded = true;
        }
    }

    public static IEnumerator UnloadPersistentScene()
    {
        if (isPersistentSceneLoaded && SceneManager.GetSceneByName(SceneNames.PersistentScene).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(SceneNames.PersistentScene);
            isPersistentSceneLoaded = false;
        }
    }
}
