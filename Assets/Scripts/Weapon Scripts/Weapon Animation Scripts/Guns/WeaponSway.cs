using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float swayAmount = 0.1f; // Silahın ne kadar salınacağı
    [SerializeField] private float swaySpeed = 5f; // Salınımın hızı
    [SerializeField] private float returnSpeed = 3f; // Silahın eski pozisyonuna dönme hızı

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        // Başlangıç pozisyonunu kaydet
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleWeaponSway();
    }

    private void HandleWeaponSway()
    {
        // Fare hareketini al
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Fare hareketine göre hedef pozisyonu hesapla
        targetPosition = initialPosition + new Vector3(
            -mouseX * swayAmount,
            -mouseY * swayAmount,
            0);

        // Yavaşça hedef pozisyona yaklaş
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, swaySpeed * Time.deltaTime);

        // Silah pozisyonunu geri döndürmek için bir interpolasyon ekleyebiliriz
        if (Vector3.Distance(transform.localPosition, initialPosition) < 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, returnSpeed * Time.deltaTime);
        }
    }
}

