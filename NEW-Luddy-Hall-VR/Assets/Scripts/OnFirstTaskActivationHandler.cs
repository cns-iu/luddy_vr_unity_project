using System.Collections;
using UnityEngine;

public class OnFirstTaskActivationHandler : MonoBehaviour
{
    public bool m_AreWallsActive = true;
    public AudioSource[] m_AudioSources = new AudioSource[4];
    public int m_AudioCounter;
    public float m_ClipLength;
    public Logger m_Logger;

    private void OnEnable()
    {
        FlowManager.UpdatePossibleNavMethodEvent += NewNavHandler;
        //SetWalls(true);
    }

    private void OnDisable()
    {
        FlowManager.UpdatePossibleNavMethodEvent -= NewNavHandler;
    }

    private void Start()
    {
        if (m_Logger.m_Trial == Trial.Trial1)
        {
            SetWalls(true);
            StartCoroutine(DisableWallsAfterAudioClip(m_AudioSources[m_AudioCounter].clip.length));
        }
      
        
    }

    private void NewNavHandler(PossibleNavigations currentNavMethod)
    {
        if (m_Logger.m_Trial == Trial.Trial1)
        {
            SetWalls(true);
            m_AudioCounter++;
            if (m_AudioCounter == m_AudioSources.Length)
            {
                return;
            }
            float duration = m_AudioSources[m_AudioCounter].clip.length;
            m_ClipLength = duration;
            StartCoroutine(DisableWallsAfterAudioClip(duration));
        }
       
    }

    private void SetWalls(bool areWallsOn)
    {
        gameObject.GetComponent<Collider>().enabled = areWallsOn;
        m_AreWallsActive = areWallsOn;
    }

    private IEnumerator DisableWallsAfterAudioClip(float duration)
    {
        //Debug.Log("called with duration: " + duration);
        yield return new WaitForSeconds(duration);
        SetWalls(false);
    }
}