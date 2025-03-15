using UnityEngine;
using UnityEngine.EventSystems; // UI tıklamalarını kontrol etmek için gerekli

public class Gun : MonoBehaviour
{
    // ScriptableObjects ile çalışan veri
    public GunData gunData;

    public Camera fpCamera;

    [SerializeField]
    private LayerMask enemyBodyLayer;

    // Componentler
    public ParticleSystem muzzleFlash;

    // Diğer scriptlere referans
    public MuzzleFlashLight muzzleFlashLight;
    public BulletTrailEffect bulletTrailEffect;

    private ShootSound shootSound;

    private void Awake()
    {
        shootSound = GetComponent<ShootSound>();
    }

    private void Update()
    {
        // Sol fare tıklamasını kontrol et
        if (Input.GetButtonDown("Fire1"))
        {
            // Eğer UI üzerindeki bir butona tıklandıysa ateş etmeyi engelle
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; // Ateş etme işlemini durdur
            }

            Shoot(); // Eğer UI'ye tıklanmadıysa ateş et
        }
    }

    private void Shoot()
    {
        muzzleFlash.Play();
        muzzleFlashLight.Flash();

        Vector3 muzzlePosition = muzzleFlash.transform.position;
        Vector3 shootDirection = muzzleFlash.transform.forward;

        bulletTrailEffect.FireBulletTrail(muzzlePosition, shootDirection);

        shootSound.PlayShootSound();

        RaycastHit hit;
        // Sadece EnemyBody katmanını kontrol etmek için LayerMask kullanıyoruz
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, gunData.range, enemyBodyLayer))
        {
            IHealth enemyHealth = hit.collider.GetComponentInParent<IHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(gunData.damage);
            }
        }
    }
}
