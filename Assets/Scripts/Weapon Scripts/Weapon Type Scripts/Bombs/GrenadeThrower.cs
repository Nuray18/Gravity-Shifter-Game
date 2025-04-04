using System.Collections;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public Transform cam;
    public Transform attackPoint;
    public GameObject grenadeObject; // Eldeki bomba (Prefab DEĞİL!)
    public GameObject grenadePrefab;

    public float throwForce = 20f;
    public float throwUpwardForce = 5f;
    public KeyCode throwKey = KeyCode.G;

    private bool hasGrenade = false;
    private bool readyToThrow = true;

    private void Start()
    {
        grenadeObject.SetActive(false); // **Başlangıçta bomba görünmez**
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && hasGrenade)
        {
            ThrowGrenade();
        }
    }

    private void ThrowGrenade()
    {
        readyToThrow = false;
        hasGrenade = false;

        grenadeObject.transform.SetParent(null);
        grenadeObject.transform.position = attackPoint.position;

        Rigidbody rb = grenadeObject.GetComponent<Rigidbody>();

        Vector3 throwDirection = cam.transform.forward * throwForce + cam.transform.up * throwUpwardForce;

        // **Önce kinematic ve gravity kapatılmalı**
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;

        rb.AddForce(throwDirection, ForceMode.Impulse);

        StartCoroutine(RespawnGrenade());
    }


    public void ResetGrenade()
    {
        hasGrenade = false;
        readyToThrow = true;
        grenadeObject.SetActive(false);
    }


    private IEnumerator RespawnGrenade()
    {
        yield return new WaitForSeconds(2f);

        if (grenadeObject == null) // **Eğer bomba yok olduysa yeniden oluştur**
        {
            grenadeObject = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            grenadeObject.transform.SetParent(transform);
        }

        hasGrenade = true;
        grenadeObject.transform.localPosition = Vector3.zero;
        grenadeObject.transform.localRotation = Quaternion.identity;

        Rigidbody rb = grenadeObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        grenadeObject.SetActive(false);
    }


    // **WeaponSwitch Bizi Çağıracak**
    public void SetGrenadeActive(bool isActive)
    {
        grenadeObject.SetActive(isActive);
        hasGrenade = isActive;
    }
}
