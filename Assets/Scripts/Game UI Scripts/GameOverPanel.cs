using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("GameOverPanel için CanvasGroup bileşeni eklenmeli!");
        }
        else
        {
            HideGameOver();
        }
    }

    public void ShowGameOver()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Cursor.visible = true;   // Mouse'u görünür yap
            Cursor.lockState = CursorLockMode.None; // Mouse'u serbest bırak
        }
    }

    public void HideGameOver()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Cursor.visible = false;  // Mouse'u gizle
            Cursor.lockState = CursorLockMode.Locked; // Mouse'u tekrar kilitle
        }
    }
}
