using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public GameObject grenadePrefab;  // Bombanın prefabı
    public Transform throwPoint;      // Bombanın çıkış noktası
    public float throwForce = 10f;    // Fırlatma gücü
    [SerializeField]
    private GameObject currentGrenade; // **Şu an elimizde olan bomba**


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // 'G' tuşuna basınca bomba at
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        if (currentGrenade == null) return;

        // Bombanın Rigidbody'sini aç
        Rigidbody grenadeRb = currentGrenade.GetComponent<Rigidbody>();
        grenadeRb.isKinematic = false;
        grenadeRb.useGravity = true;

        currentGrenade.transform.SetParent(null);

        Vector3 throwDirection = Camera.main.transform.forward;
        grenadeRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        Grenade grenadeScript = currentGrenade.GetComponent<Grenade>();
        if (grenadeScript != null)
        {
            grenadeScript.Throw(throwDirection, throwForce);
        }

        // Artık elimizde bomba kalmadı
        currentGrenade = null;
    }
}
