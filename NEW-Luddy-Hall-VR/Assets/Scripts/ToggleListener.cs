using System.Collections.Generic;
using UnityEngine;

public enum SymbolType { Mark, Line }

public class ToggleListener : MonoBehaviour
{
    public ActiveNavigation m_ActiveNavigation;
    public SymbolType m_Type;

    //public DataOrigin m_PhaseType;
    public int m_TaskNumber;

    public bool m_IsNavMethodVisible = true;

    //public bool m_IsInTimeRange = true;
    //public bool m_IsPhaseVisible = true;
    public bool m_IsTaskNumberVisible = true;

    private void OnEnable()
    {
        ToggleEventHandler.ToggleByActiveNavigationMethodEvent += SetVisibility;
        //ToggleEventHandler.ToggleByPhaseEvent += SetVisibility;
        ToggleEventHandler.ToggleByTaskNumberEvent += SetVisibility;
        //TimeControl.TimeRangeUpdateEvent += SetVisibility;
    }

    private void OnDestroy()
    {
        ToggleEventHandler.ToggleByActiveNavigationMethodEvent -= SetVisibility;
        //ToggleEventHandler.ToggleByPhaseEvent -= SetVisibility;
        ToggleEventHandler.ToggleByTaskNumberEvent -= SetVisibility;
        //TimeControl.TimeRangeUpdateEvent -= SetVisibility;
    }

    private void Start()
    {
        //m_PhaseType = GetComponent<Record>().m_Phase;
        m_TaskNumber = GetComponent<Record>().m_TaskNumber;
        m_ActiveNavigation = GetComponent<Record>().m_ActiveNavigation;
    }

    private void SetVisibility(int taskNumber, bool isOn)
    {
        //Debug.LogFormat("SetVisibility called with ars: {0}, {1}; expression evaluates to {2}", taskNumber, isOn, taskNumber == m_TaskNumber);
        if (taskNumber == m_TaskNumber)
        {
            //Debug.Log("received: " + taskNumber);
            m_IsTaskNumberVisible = isOn;

            SetBySymbolType();
            //if (GetComponent<MeshRenderer>().enabled == false)
            //{
            //    //Debug.LogFormat("Going dark, my taskNumber is {0}", m_TaskNumber);
            //}

            //CheckFlags(new List<bool> { m_IsPhaseVisible, m_IsInTimeRange, m_IsNavMethodVisible }));
        }
    }

    private void SetVisibility(ActiveNavigation activeNavigation, bool isOn)
    {
        //Debug.LogFormat("called SetVisibility() with args: {0}, {1} ", type, isOn);
        if (activeNavigation == m_ActiveNavigation)
        {
            m_IsNavMethodVisible = isOn;
            SetBySymbolType();
            //GetComponent<MeshRenderer>().enabled = (m_IsEncodingVisible && CheckFlags(new List<bool> { m_IsPhaseVisible, m_IsInTimeRange, m_IsTaskNumberVisible }));
        }
    }

    private void SetBySymbolType()
    {
        switch (m_Type)
        {
            case SymbolType.Mark:
                GetComponent<MeshRenderer>().enabled = (m_IsTaskNumberVisible && m_IsNavMethodVisible);
                break;

            case SymbolType.Line:
                GetComponent<LineRenderer>().enabled = (m_IsTaskNumberVisible && m_IsNavMethodVisible);
                break;

            default:
                break;
        }
    }

    //private void SetVisibility(float converted)
    //{
    //    //Debug.LogFormat("called SetVisibility() with args: {0}", converted);
    //    //float timestamp = GetComponent<Record>().m_TimeStamp;
    //    //m_IsInTimeRange = timestamp <= converted;
    //    GetComponent<Renderer>().enabled = (m_IsInTimeRange && CheckFlags(new List<bool> { m_IsNavMethodVisible, m_IsPhaseVisible, m_IsTaskNumberVisible }));
    //}

    //private void SetVisibility(DataOrigin phase, bool isOn)
    //{
    //    //Debug.LogFormat("called SetVisibility() with args: {0}, {1} ", phase, isOn);
    //    if (m_PhaseType == phase)
    //    {
    //        m_IsPhaseVisible = isOn;
    //        GetComponent<MeshRenderer>().enabled = (m_IsPhaseVisible && CheckFlags(new List<bool> { m_IsEncodingVisible, m_IsInTimeRange, m_IsTaskNumberVisible }));
    //    }
    //}

    private bool CheckFlags(List<bool> flags)
    {
        for (int i = 0; i < flags.Count; i++)
        {
            if (flags[i] == false)
            {
                return false;
            }
        }
        return true;
    }
}