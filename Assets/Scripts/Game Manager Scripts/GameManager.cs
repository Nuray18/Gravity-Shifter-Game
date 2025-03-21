﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private NavMeshRebuilder navMeshRebuilder;

    [HideInInspector]
    public PlayerHealthUI playerHealthUI; // Tüm sahneler için ortak UI

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Sahne yüklendiğinde çağrılacak
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != SceneNames.MainMenu)
        {
            navMeshRebuilder = FindObjectOfType<NavMeshRebuilder>();
            RestartLighting();
        }
        else
        {
            navMeshRebuilder = null;
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    private void RestartLighting()
    {
        DynamicGI.UpdateEnvironment();
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.ambientLight = Color.white; // Ortam ışığını sıfırla
        LightmapSettings.lightmapsMode = LightmapsMode.CombinedDirectional;
    }

    public void RestartGame()
    {
        PlayerManager.instance.isRestarting = true;

        // Düşmanları temizle ve sıfırla
        EnemyManager.instance?.ClearEnemies();
        EnemyManager.instance?.ResetEnemies();

        // NavMesh ve ışıklandırmayı yenile
        navMeshRebuilder?.RebuildNavMeshes();
        RestartLighting();

        PlayerManager.instance.RestartPlayer();

        Time.timeScale = 1f;

        if (SceneLoader.instance != null)
        {
            SceneLoader.LoadScene(SceneNames.Level1);
        }
        else
        {
            Debug.LogError("SceneLoader instance is NULL! Make sure SceneLoader is in the scene.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Hafıza sızıntısını önlemek için event'ten çık
    }
}


//#if UNITY_EDITOR
//    Debug.Log("RestartLighting called!");
//#endif
