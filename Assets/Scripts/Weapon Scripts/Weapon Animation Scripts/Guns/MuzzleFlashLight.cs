using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashLight : MonoBehaviour
{
    public Light muzzleFlashLight;

    void Start()
    {
        // Başlangıçta ışığı kapat
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.enabled = false;
        }
    }

    public void Flash()
    {
        if (muzzleFlashLight != null)
        {
            // Işığı kısa süreliğine aç
            StartCoroutine(FlashLightRoutine());
        }
    }

    private IEnumerator FlashLightRoutine()
    {
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(0.05f); // 0.05 saniye sonra ışığı kapat
        muzzleFlashLight.enabled = false;
    }
}
