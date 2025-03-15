using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameButton : MonoBehaviour
{
    public GameObject pausePanel; // Pause panel GameObject'i
    public Button pauseButton; // Pause butonu
    public Button backButton; // Back butonu oyuna devam etmek icin

    private bool isPaused = false; // Oyun duraklatma durumu

    void Start()
    {
        // Pause butonuna tıklama fonksiyonunu ekle
        pauseButton.onClick.AddListener(PauseGame);

        // Resume butonuna tıklama fonksiyonunu ekle
        backButton.onClick.AddListener(BackToGame);

        // Başlangıçta pause paneli gizli olsun
        pausePanel.SetActive(false);
    }

    // Oyun duraklatıldığında çağrılacak fonksiyon
    public void PauseGame()
    {
        pausePanel.SetActive(true); // Pause panelini aktif et
        Time.timeScale = 0f; // Oyun zamanını duraklat
        isPaused = true; // Pause durumu aktif
    }

    // Oyun devam ettiğinde çağrılacak fonksiyon
    public void BackToGame()
    {
        pausePanel.SetActive(false); // Pause panelini gizle
        Time.timeScale = 1f; // Oyun zamanını eski haline getir
        isPaused = false; // Pause durumu pasif
    }
}
