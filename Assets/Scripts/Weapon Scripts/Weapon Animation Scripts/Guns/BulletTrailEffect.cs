using UnityEngine;

public class BulletTrailEffect : MonoBehaviour
{
    public ParticleSystem bulletTrailEffect;

    public void FireBulletTrail(Vector3 startPosition, Vector3 direction)
    {
        // Mermi izini başlat
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(direction);

        // Efekti oynat
        bulletTrailEffect.Play();
    }
}

