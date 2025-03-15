using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footstepSound;

    [SerializeField]
    private AudioClip[] footstepClip;

    private CharacterController characterController;

    [HideInInspector]
    public float volumeMin, volumeMax;

    private float accumulatedDistance;

    [HideInInspector]
    public float stepDistance;

    private Player player; // 'readonly' kaldırıldı

    private void Awake()
    {
        footstepSound = GetComponent<AudioSource>();

        characterController = GetComponentInParent<CharacterController>();

        // Player referansını alıyoruz
        player = GetComponentInParent<Player>();

        if (player == null)
        {
            Debug.LogError("PlayerFootsteps: Player script'i bulunamadı! Lütfen Player script'inin doğru nesneye bağlı olduğundan emin olun.");
        }
    }

    private void Update()
    {
        CheckToPlayerFootstepSound();
    }

    private void CheckToPlayerFootstepSound()
    {
        if (player == null || !player.IsPlayerGrounded()) // Null kontrolü ekledik
            return;

        if (characterController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;

            if (accumulatedDistance > stepDistance)
            {
                footstepSound.volume = Random.Range(volumeMin, volumeMax);
                footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)];
                footstepSound.Play();

                accumulatedDistance = 0f;
            }
        }
        else
        {
            accumulatedDistance = 0f;
        }
    }
}
