using UnityEngine;

public enum ExperimentPhase { Tutorial, Complexity, Plateau }

public enum PossibleNavigations { Walk, Teleport, Freefly, All, End }

public enum ActiveNavigation { Walk, Teleport, Freefly, NoChoiceMadeYet, None }

public class FlowManager : MonoBehaviour
{
    public TaskManager m_TaskManager;
    public ExperimentPhase m_CurrentPhase = ExperimentPhase.Tutorial;
    public PossibleNavigations m_CurrentlyPossibleNavMethod = PossibleNavigations.Walk;
    public ActiveNavigation m_ActiveNavigation = ActiveNavigation.Walk;

    public int m_ExperimentPhaseCounter = 0;
    public bool m_PlayAudio = true;
    public GameObject AudioPlayer;

    public delegate void UpdatePhase(ExperimentPhase currentPhase);

    public static event UpdatePhase UpdatePhaseEvent;

    public delegate void UpdatePossibleNavMethod(PossibleNavigations currentNavMethod);

    public static event UpdatePossibleNavMethod UpdatePossibleNavMethodEvent;

    public delegate void UpdateUserSelectedNavMethod(ActiveNavigation activeNavigation);

    public static event UpdateUserSelectedNavMethod UpdateUserSelectedNavMethodEvent;

    public delegate void EndOfSession();

    public static event EndOfSession EndOfSessionEvent;

    private void OnEnable()
    {
        //TaskManager.UpdateTaskEvent += SetPlayerPosition;
        TaskManager.UpdateTaskEvent += SetExperimentPhase;
        TaskManager.UpdateTaskEvent += SetPossibleNavMethod;
        RadialMenu.SetUserSelectedNavMethodEvent += SetCurrentUserSelectedNavMethod;
        UpdatePossibleNavMethodEvent += SetCurrentActiveNavMethod;
    }

    private void OnDisable()
    {
        //TaskManager.UpdateTaskEvent -= SetPlayerPosition;
        TaskManager.UpdateTaskEvent -= SetExperimentPhase;
        TaskManager.UpdateTaskEvent -= SetPossibleNavMethod;
        RadialMenu.SetUserSelectedNavMethodEvent -= SetCurrentUserSelectedNavMethod;
        UpdatePossibleNavMethodEvent -= SetCurrentActiveNavMethod;
    }

    private void SetExperimentPhase(int taskNumber)
    {
        if (taskNumber == 0)
        {
            m_CurrentPhase = ExperimentPhase.Tutorial;
        }
        //else if (taskNumber >= 1 && taskNumber < 5)
        else
        {
            m_CurrentPhase = ExperimentPhase.Complexity;
        }
        //else
        //{
        //    m_CurrentPhase = ExperimentPhase.Plateau;
        //}

        UpdatePhaseEvent?.Invoke(m_CurrentPhase);
    }

    private void SetPossibleNavMethod(int taskNumber)
    {
        if (taskNumber == 0)
        {
            if (m_CurrentlyPossibleNavMethod == PossibleNavigations.All)
            {
                EndOfSessionEvent?.Invoke();
            }

            m_CurrentlyPossibleNavMethod++;

            UpdatePossibleNavMethodEvent?.Invoke(m_CurrentlyPossibleNavMethod);
        }
    }

    private void SetCurrentUserSelectedNavMethod(ActiveNavigation currentActiveNavigation)
    {
        m_ActiveNavigation = currentActiveNavigation;
    }

    private void SetCurrentActiveNavMethod(PossibleNavigations currentNavMethod)
    {
        switch (currentNavMethod)
        {
            case PossibleNavigations.Walk:
                m_ActiveNavigation = ActiveNavigation.Walk;
                break;

            case PossibleNavigations.Teleport:
                m_ActiveNavigation = ActiveNavigation.Teleport;
                break;

            case PossibleNavigations.Freefly:
                m_ActiveNavigation = ActiveNavigation.Freefly;
                break;

            case PossibleNavigations.All:
                m_ActiveNavigation = ActiveNavigation.NoChoiceMadeYet;
                break;

            case PossibleNavigations.End:
                break;

            default:
                break;
        }
    }

    private void Start()
    {
        EnableOrDisableAudio();
    }

    private void EnableOrDisableAudio()
    {
        AudioPlayer.GetComponent<AudioManager>().enabled = m_PlayAudio;
    }
}