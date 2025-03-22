using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    public GameObject backupCamera; // Yedek kamera referansı (Inspector'dan atanacak)

    public GameObject gameOverPanel;

    public GameObject pausePanel; // Pause panel GameObject'i
    public Button pauseButton; // Pause butonu
    public Button backButton; // Back butonu oyuna devam etmek icin

    private bool isPaused = false; // Oyun duraklatma durumu

    void Start()
    {
        // Pause butonuna tıklama fonksiyonunu ekle
        if (pauseButton != null) pauseButton.onClick.AddListener(PauseGame);

        // Resume butonuna tıklama fonksiyonunu ekle
        if (backButton != null) backButton.onClick.AddListener(BackToGame);

        // Başlangıçta pause paneli gizli olsun
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void RestartGameFromBeginning()
    {
        // Yedek kamerayı SceneLoader'a aktar
        if (backupCamera != null)   SceneLoader.backupCamera = backupCamera;

        GameManager.instance.RestartGame();

        if (backupCamera != null)   backupCamera.SetActive(false);
        
    }

    public void LeaveToMainMenu()
    {
        // camera
        if (backupCamera != null)   SceneLoader.backupCamera = backupCamera;
        // Scene load
        if (SceneLoader.instance != null)   SceneLoader.LoadScene(SceneNames.MainMenu);
        // camera
        if (backupCamera != null)   backupCamera.SetActive(false);
    }

    // Oyun duraklatıldığında çağrılacak fonksiyon
    public void PauseGame()
    {
        pausePanel.SetActive(true); // Pause panelini aktif et
        Time.timeScale = 0f; // Oyun zamanını duraklat
        isPaused = true; // Pause durumu aktif
    }

    public void BackToGame()
    {
        pausePanel.SetActive(false); // Pause panelini gizle
        Time.timeScale = 1f; // Oyun zamanını eski haline getir
        isPaused = false; // Pause durumu pasif
    }

    public void ShowGameOverScreen()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
