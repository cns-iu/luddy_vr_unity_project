using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public TaskManager m_TaskManager;
    public FlowManager m_FlowManager;
    public AudioSource m_SelectionSound;
    public AudioSource m_MenuUpSound;
    public AudioSource[] m_TutorialSounds = new AudioSource[7];
    public float m_Delay = 1f;
    public Logger m_Logger;
    public bool m_IsAnyTutorialAudioPlaying = false;

    //Subscribe to FLowManager events
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        FlowManager.UpdatePossibleNavMethodEvent -= HandleEventsForAudioOnNavMethodChange;
        FlowManager.UpdatePhaseEvent -= HandleEventsForAudioOnPhaseChange;
        FlowManager.EndOfSessionEvent -= PlayFinalMessage;
        HandleAudio.FirstCollisionEvent -= HandleAudioOnFirstWayPointCollision;
    }

    private void Start()
    {
        if (m_Logger.m_Trial == Trial.Trial1)
        {
            FlowManager.UpdatePossibleNavMethodEvent += HandleEventsForAudioOnNavMethodChange;
            FlowManager.UpdatePhaseEvent += HandleEventsForAudioOnPhaseChange;
            FlowManager.EndOfSessionEvent += PlayFinalMessage;
            HandleAudio.FirstCollisionEvent += HandleAudioOnFirstWayPointCollision;
            PlayMessage(0);
        }
    }

    private void HandleEventsForAudioOnNavMethodChange(PossibleNavigations currentNavMethod)
    {
        if (m_FlowManager.m_CurrentPhase == ExperimentPhase.Tutorial)
        {
            //Debug.Log("here now");
            switch (currentNavMethod)
            {
                case PossibleNavigations.Walk:
                    break;

                case PossibleNavigations.Teleport:
                    //Debug.Log("now tepelort");
                    PlayMessage(3);
                    break;

                case PossibleNavigations.Freefly:
                    //Debug.Log("now freefly");
                    PlayMessage(4);
                    break;

                case PossibleNavigations.All:
                    //Debug.Log("now all");
                    PlayMessage(5);
                    break;

                case PossibleNavigations.End:
                    break;

                default:
                    break;
            }
        }
    }

    private void HandleEventsForAudioOnPhaseChange(ExperimentPhase currentPhase)
    {
        if (currentPhase == ExperimentPhase.Complexity && m_TaskManager.m_TaskNumber == 1 && m_FlowManager.m_CurrentlyPossibleNavMethod == PossibleNavigations.Walk)
        {
            PlayMessage(2);
        }
    }

    private void HandleAudioOnFirstWayPointCollision()
    {
        PlayMessage(1);
    }

    private void PlayFinalMessage()
    {
        PlayMessage(m_TutorialSounds.Length - 1);
    }

    private void PlayMessage(int index)
    {
        StartCoroutine(PlayAudioAfterDelay(index));
        //Debug.Log("message just started playing");
    }

    private IEnumerator PlayAudioAfterDelay(int index)
    {
        yield return new WaitForSeconds(m_Delay);
        m_IsAnyTutorialAudioPlaying = true;
        m_TutorialSounds[index].Play();
        StartCoroutine(SetIsAudioPlayingBool(m_TutorialSounds[index].clip.length));
    }

    private IEnumerator SetIsAudioPlayingBool(float clipLength)
    {
        //Debug.Log("m_IsAnyTutorialAudioPlaying: " + m_IsAnyTutorialAudioPlaying);
        yield return new WaitForSeconds(clipLength);
        m_IsAnyTutorialAudioPlaying = false;
        //Debug.Log("m_IsAnyTutorialAudioPlaying: " + m_IsAnyTutorialAudioPlaying);
    }
}