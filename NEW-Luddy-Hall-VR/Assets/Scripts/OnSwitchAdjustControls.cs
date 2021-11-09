using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface ISwitchable
{

}

public class OnSwitchAdjustControls : MonoBehaviour, ISwitchable
{

    [Header("Scripts for control")]
    //[SerializeField]
    //public List<ISwitchable> m_ListOfControlScripts = new List<ISwitchable>(4);
    public Locomotion m_Locomotion;
    public Teleporter m_Teleport;
    public FreeFlyer m_Freefly;

    

private void OnEnable()
    {
        SwitchControl.ControlSwitchHandlerEvent += EnableCorrectControls;
        SwitchControl.ControlSwitchHandlerEvent += SetUpUI;
    }

    private void OnDisable()
    {
        SwitchControl.ControlSwitchHandlerEvent -= EnableCorrectControls;
        SwitchControl.ControlSwitchHandlerEvent -= SetUpUI;
    }

    void EnableCorrectControls(NavMethod currentMode)
    {
        switch (currentMode)
        {
            case NavMethod.Walk:
                m_Locomotion.enabled = true;
                m_Teleport.enabled = false;
                m_Freefly.enabled = false;
                break;
            case NavMethod.Teleport:
                m_Locomotion.enabled = false;
                m_Teleport.enabled = true;
                m_Freefly.enabled = false;
                break;
            case NavMethod.Freefly:
                m_Locomotion.enabled = false;
                m_Teleport.enabled = false;
                m_Freefly.enabled = true;
                break;
            default:
                break;
        }
    }

    void SetUpUI(NavMethod currentMode) { }

}
