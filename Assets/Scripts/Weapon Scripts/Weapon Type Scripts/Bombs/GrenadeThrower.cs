using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public GameObject grenadePrefab; // Bomban?n prefab?

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // Sa? t?k ile bomba at
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
        Grenade grenadeScript = grenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Throw(transform.forward); // Bombay? ileri f?rlat
        }
    }

}
