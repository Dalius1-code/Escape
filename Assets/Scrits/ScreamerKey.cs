using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamerKey : MonoBehaviour, IInteractable
{
    public AudioClip screamerclip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (audioSource != null && screamerclip != null)
        {
            audioSource.PlayOneShot(screamerclip);
        }
    }
}