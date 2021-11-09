using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlaySound : MonoBehaviour
{
    bool m_HasBeenPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_HasBeenPlayed)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            m_HasBeenPlayed = true;
        }
       
    }
}
