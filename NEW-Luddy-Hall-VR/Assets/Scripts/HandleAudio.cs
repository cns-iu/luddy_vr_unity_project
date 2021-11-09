using UnityEngine;

public class HandleAudio : MonoBehaviour
{
    //public AudioSource m_AudioSource;
    public FlowManager m_FlowManager;
    public Logger m_Logger;

    public delegate void FirstCollision();

    public static event FirstCollision FirstCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Logger.m_Trial == Trial.Trial1)
        {
            if (m_FlowManager.m_CurrentlyPossibleNavMethod == PossibleNavigations.Walk)
            {
                FirstCollisionEvent?.Invoke();
            }
        }
    }
}