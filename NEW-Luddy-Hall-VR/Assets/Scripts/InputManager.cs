using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour
{
    [Header("Actions")]
    public SteamVR_Action_Boolean m_Touch = null;
    public SteamVR_Action_Boolean m_Press = null;
    public SteamVR_Action_Vector2 m_TouchPosition = null;

    [Header("Scene Objects")]
    public RadialMenu m_RadialMenu;



    private void OnEnable()
    {
        FlowManager.UpdatePossibleNavMethodEvent += SubscribeToActions;
    }

    private void OnDestroy()
    {
        m_Touch.onChange -= Touch;
        m_Touch.onStateUp -= PressRelease;
        m_TouchPosition.onAxis -= Position;
        FlowManager.UpdatePossibleNavMethodEvent -= SubscribeToActions;
    }

    void SubscribeToActions(PossibleNavigations currentNavMethod)
    {
        if (currentNavMethod == PossibleNavigations.All)
        {
            m_Touch.onChange += Touch;
            m_TouchPosition.onAxis += Position;
            m_Touch.onStateUp += PressRelease;
        }
        
    }

    void Position(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        m_RadialMenu.SetTouchPosition(axis);
    }

    void Touch(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        m_RadialMenu.Show(newState);
    }

    void PressRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        m_RadialMenu.ActivateHighlightedSection();

    }

}
