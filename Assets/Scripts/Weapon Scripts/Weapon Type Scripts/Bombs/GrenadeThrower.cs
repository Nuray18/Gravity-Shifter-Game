using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public GameObject grenadePrefab; // Bomban?n prefab?
    public Transform throwPoint;
    public float throwForce = 15f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // Sa? t?k ile bomba at
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        // Bombayı oluştur
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        
        // Rigidbody ekle ve fırlatma gücü uygula
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.VelocityChange);
        }
    }

}
