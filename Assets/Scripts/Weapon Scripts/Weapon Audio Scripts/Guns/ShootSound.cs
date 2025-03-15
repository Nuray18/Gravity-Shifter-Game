using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSound : MonoBehaviour
{
    public AudioClip shootSound; // Assign in Inspector
    public AudioClip reloadSound; // Assign in Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound); // Plays the shooting sound
        }
    }

    public void PlayReloadSound()
    {
        if (reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound); // Plays the reloading sound
        }
    }

}
