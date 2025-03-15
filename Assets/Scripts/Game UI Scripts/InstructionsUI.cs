using System.Collections;
using UnityEngine;
using TMPro;

public class InstructionsUI : MonoBehaviour
{
    public TMP_Text instructionsText; // UI Text bileşeni
    public float displayDuration = 10f; // Talimatların görüneceği süre (saniye)

    private CanvasGroup canvasGroup; // CanvasGroup referansı

    private void Start()
    {
        // 1. Adım: TextMeshPro GameObject'ine CanvasGroup ekliyoruz.
        // instructionsText GameObject'inden CanvasGroup bileşenini alıyoruz.
        canvasGroup = instructionsText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Eğer CanvasGroup yoksa, bir tane ekliyoruz.
            canvasGroup = instructionsText.gameObject.AddComponent<CanvasGroup>();
        }

        // 2. Adım: Yazıyı tamamen görünür yapıyoruz.
        canvasGroup.alpha = 1f; // Şu anda yazı görünür.

        // 3. Adım: Yazıyı ekranda göstermeye başlıyoruz.
        instructionsText.gameObject.SetActive(true);

        // 4. Adım: Belirli bir süre sonra (10 saniye) yazıyı gizlemeye başlıyoruz.
        Invoke("HideInstructions", displayDuration); // 10 saniye sonra HideInstructions fonksiyonu çağrılır.
    }

    private void HideInstructions()
    {
        // 5. Adım: Fade-out işlemini başlatıyoruz.
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        // 6. Adım: Fade-out işlemi için süreyi belirliyoruz.
        float duration = 1f; // Fade-out işlemi 1 saniye sürecek.
        float elapsed = 0f;

        // 7. Adım: Alpha değerini yavaşça 0'a çekiyoruz (yazı kaybolacak).
        while (elapsed < duration)
        {
            // Alpha değerini her frame'de azaltıyoruz.
            canvasGroup.alpha = 1f - (elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Bir sonraki frame'e geçmek için bekliyoruz.
        }

        // 8. Adım: Alpha değeri sıfır olduğunda tamamen görünmez oluyor.
        canvasGroup.alpha = 0f;

        // 9. Adım: Yazıyı tamamen gizliyoruz.
        instructionsText.gameObject.SetActive(false);
    }
}
