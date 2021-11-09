//using Boo.Lang;
using System.Collections.Generic;
using UnityEngine;

public class Dataset : MonoBehaviour
{
    //public float m_FirstTimeStamp;
    //public float m_LastTimeStamp;
    //public float m_TimeRange;
    //public int m_CurrentTaskNumber;
    //public GameObject m_MarksParent;
    //public List<GameObject> m_CurrentlyVisibleMarks = new List<GameObject>();

    public List<float[]> m_RawTaskCompletionTimeStamps = new List<float[]>();

    //public Dictionary<int, float>  = new Dictionary<int, float>();
    private List<float[]> m_EndPoints = new List<float[]>();

    //public List<float> m_RawTaskCompletionTimeStamps = new List<float>();
    public List<float> m_ProcessedCompletionTimes = new List<float>();

    private void Start()
    {
        //foreach (var item in m_RawTaskCompletionTimeStamps)
        //{
        //    //Debug.Log(item[0] + ", " + item[1]);
        //}
        GetTimestampsAtTaskSubmission();
        ComputeIndividualCompletionTimes();
    }

    private void ComputeIndividualCompletionTimes()
    {
        m_ProcessedCompletionTimes.Add(m_EndPoints[0][1]);

        for (int i = 1; i < m_EndPoints.Count; i++)
        {
            float current = m_EndPoints[i][1];
            float previous = m_EndPoints[i - 1][1];
            m_ProcessedCompletionTimes.Add(current - previous);
        }
    }

    private void GetTimestampsAtTaskSubmission()
    {
        for (int i = 0; i < m_RawTaskCompletionTimeStamps.Count; i++)
        {
            float[] current = m_RawTaskCompletionTimeStamps[i];
            if (i + 1 < m_RawTaskCompletionTimeStamps.Count)
            {
                float[] next = m_RawTaskCompletionTimeStamps[i + 1];
                if (current[0] != next[0])
                {
                    m_EndPoints.Add(current);
                }
            }
            else
            {
                m_EndPoints.Add(m_RawTaskCompletionTimeStamps[m_RawTaskCompletionTimeStamps.Count - 1]);

                //foreach (var item in endPoints)
                //{
                //    Debug.Log(item[0] + ", " + item[1]);
                //}
                //Debug.Log(endPoints);
            }
        }
    }

    //private void OnEnable()
    //{
    //    TimeControl.TimeRangeUpdateEvent += DetermineTaskNumber;
    //}

    //private void OnDestroy()
    //{
    //    TimeControl.TimeRangeUpdateEvent -= DetermineTaskNumber;
    //}

    //private void DetermineTaskNumber(float converted)
    //{
    //    UpdateCurrentlyVisibleMarks();
    //    m_CurrentTaskNumber = m_CurrentlyVisibleMarks[m_CurrentlyVisibleMarks.Count-1].GetComponent<Record>().m_TaskNumber;
    //}

    //private List<GameObject> UpdateCurrentlyVisibleMarks()
    //{
    //    m_CurrentlyVisibleMarks.Clear();
    //    for (int i = 0; i < m_MarksParent.transform.childCount; i++)
    //    {
    //        GameObject mark = m_MarksParent.transform.GetChild(i).gameObject;
    //        if (mark.GetComponent<MeshRenderer>().enabled)
    //        {
    //            m_CurrentlyVisibleMarks.Add(mark);
    //        }
    //    }
    //    Debug.Log(m_CurrentlyVisibleMarks.Count);

    //    return m_CurrentlyVisibleMarks;
    //}
}