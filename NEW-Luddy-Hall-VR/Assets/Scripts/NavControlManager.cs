using UnityEngine;

public class NavControlManager : MonoBehaviour
{
    public Locomotion m_Locomotion;
    public Teleporter m_Teleporter;
    public FreeFlyer m_Freeflyer;
    public InputManager m_InputManager;

    private void OnEnable()
    {
        FlowManager.UpdatePossibleNavMethodEvent += SetPossibleNavMethod;
    }

    private void OnDisable()
    {
        FlowManager.UpdatePossibleNavMethodEvent -= SetPossibleNavMethod;
    }

    void SetPossibleNavMethod(PossibleNavigations possibleNavMethod)
    {
        switch (possibleNavMethod)
        {
            case PossibleNavigations.Walk:
                m_Locomotion.enabled = true;
                m_Teleporter.enabled = false;
                m_Freeflyer.enabled = false;
                break;
            case PossibleNavigations.Teleport:
                m_Locomotion.enabled = false;
                m_Teleporter.enabled = true;
                m_Freeflyer.enabled = false;
                break;
            case PossibleNavigations.Freefly:
                m_Locomotion.enabled = false;
                m_Teleporter.enabled = false;
                m_Freeflyer.enabled = true;
                break;
            case PossibleNavigations.All:
                m_InputManager.enabled = true;
                m_Locomotion.enabled = false;
                m_Teleporter.enabled = false;
                m_Freeflyer.enabled = false;
                break;
            default:
                break;
        }

    }
}
