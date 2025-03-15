using System.Collections;
using UnityEngine;

public class RootLook : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Transform playerBody;

    private float mouseSensitivity = 100f;
    private float xRotation = 0f;

    private bool isCursorLocked = true;

    void Start()
    {
        LockCursor();  // Başlangıçta fareyi kilitle
    }

    void Update()
    {
        HandleCursorLockToggle();  // Escape tuşu ile fareyi aç/kilitle
        if (isCursorLocked)  // Fare kilitliyse hareketi kontrol et
        {
            HandleCameraMovement();
        }
    }

    // Fareyi kilitlemek için fonksiyon
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    // Fareyi serbest bırakmak için fonksiyon
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }

    // Escape tuşuna basıldığında fareyi aç/kilitleme işlemi
    private void HandleCursorLockToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorLocked)
                UnlockCursor();
            else
                LockCursor();
        }
    }

    // Kamerayı hareket ettirmek için fonksiyon
    private void HandleCameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // Yeni yerçekimine göre kamerayı döndürme işlemi
    public IEnumerator RotateCameraTowardsGravity(Vector3 newGravityDir)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(Camera.main.transform.up, -newGravityDir) * Camera.main.transform.rotation;

        while (Quaternion.Angle(Camera.main.transform.rotation, targetRotation) > 0.1f)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        Camera.main.transform.rotation = targetRotation;
    }
}
