using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static GameObject backupCamera;  // Yedek kamera referansı
    public static SceneLoader instance; // Singleton instance

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    // LoadScene functionu oyuna ozel yapilmis sceneload yapan functur bu funcu leavegamebutton, restartbutton vb. leri icin olusturulmustur.
    public static IEnumerator LoadSceneCoroutine(string sceneName)
    {
        if (backupCamera != null)
        {
            backupCamera.SetActive(true);
        }

        if (sceneName == SceneNames.MainMenu) // MainMenu'ye dönüş
        {
            yield return PersistentSceneLoader.instance.StartCoroutine(PersistentSceneLoader.UnloadPersistentScene());
            //Debug.Log("PersistentSceneLoader.instance: " + (PersistentSceneLoader.instance != null ? "Var" : "YOK!"));
            DestroyPersistentObjects();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!asyncLoad.isDone)
            {
                yield return null; // MainMenu yüklenene kadar bekle
            }
        }
        else
        {
            yield return instance.StartCoroutine(UnloadAllLevels());

            yield return PersistentSceneLoader.instance.StartCoroutine(PersistentSceneLoader.EnsurePersistentSceneLoaded());

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null; // Yükleme bitene kadar bekle
            }
        }

        yield return new WaitForEndOfFrame(); // Kamera değişimlerinin düzgün çalışması için bir frame bekle

        // Eğer MainMenu yüklendiyse backup kamerayı kapat
        if (backupCamera != null)
        {
            backupCamera.SetActive(false);
        }
    }

    private static IEnumerator UnloadAllLevels()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != SceneNames.PersistentScene && scene.name != SceneNames.MainMenu)
            {
                // Sahne yüklüyse, unload et
                if (scene.isLoaded)
                {
                    yield return SceneManager.UnloadSceneAsync(scene);
                }
            }
        }
    }

    public static void DestroyPersistentObjects()
    {
        GameObject[] dontDestroyObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in dontDestroyObjects)
        {
            if (obj.scene.buildIndex == -1 && obj != backupCamera)
            {
                Destroy(obj);
            }
        }

        // Backup kamera da sahne geçişlerinde düzgün sıfırlanmalı
        

    }
}
