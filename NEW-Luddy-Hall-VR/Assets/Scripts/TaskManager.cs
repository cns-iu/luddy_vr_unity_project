using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameObject[] m_Tasks = new GameObject[4];
    public int m_TaskNumber = 0;
    public bool m_IsGodModeOn = false;
    public GameObject m_ActiveTask;

    public delegate void UpdateTask(int index);

    public static event UpdateTask UpdateTaskEvent;

    private void Awake()
    {
        AssignActiveTask();
    }

    private void OnEnable()
    {
        CollisionHandler.OnSubmissionEvent += HandleTask;
    }

    private void OnDisable()
    {
        CollisionHandler.OnSubmissionEvent -= HandleTask;
    }

    private void HandleTask()
    {
        m_Tasks[m_TaskNumber].gameObject.SetActive(false);
        m_TaskNumber++;
        if (m_TaskNumber >= m_Tasks.Length)
        {
            m_TaskNumber = 0;
        }
        AssignActiveTask();
        m_Tasks[m_TaskNumber].gameObject.SetActive(true);
        if (UpdateTaskEvent != null)
        {
            UpdateTaskEvent(m_TaskNumber);
        }
    }

    private void AssignActiveTask()
    {
        m_ActiveTask = m_Tasks[m_TaskNumber];
    }

    private void Update()
    {
        SkipTasks();
    }

    private void SkipTasks()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (m_IsGodModeOn)
            {
                HandleTask();
            }
        }
    }
}