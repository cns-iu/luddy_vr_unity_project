using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerReflective : MonoBehaviour
{
    public LoggerReflective m_Logger;
    public AudioSource m_Source;
    // Start is called before the first frame update
    void Start()
    {
        if (m_Logger.m_ReflectionPhasePart == LoggerReflective.ReflectionPhasePart.Intro)
        {
            StartCoroutine(PlayAfterDelay());
        }
    }

    IEnumerator PlayAfterDelay() {
        yield return new WaitForSeconds(1f);
        m_Source.Play();
    }
}
