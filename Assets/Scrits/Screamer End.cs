using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamerEnd : MonoBehaviour
{
    public AudioClip screamerclip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Screamer()
    {
        if (audioSource != null && screamerclip != null)
        {
            audioSource.PlayOneShot(screamerclip);
        }
    }
    public void End()
    {
        Application.Quit();
    }
}
