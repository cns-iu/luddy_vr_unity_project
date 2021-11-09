using System.Collections;
using UnityEngine;
using Valve.VR;

public enum NavMethod { Walk, Teleport, Freefly }

public class SwitchControl : MonoBehaviour
{
    public NavMethod m_CurrentMode = NavMethod.Walk;
    public SteamVR_Action_Boolean m_MenuAction;

    public delegate void ControlSwitchHandler(NavMethod currentMode);
    public static event ControlSwitchHandler ControlSwitchHandlerEvent;

    public float m_FadeTime = 2;

    int counter = 0;

    private void Update()
    {
        if (counter == 2)
        {
            counter = -1;
        }

        if (m_MenuAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            counter++;
            SetNavMethod(counter);
            StartCoroutine(FadeBetweenControlModes());
            ControlSwitchHandlerEvent?.Invoke(m_CurrentMode);
        }
    }

    void SetNavMethod(int counter)
    {
        m_CurrentMode = (NavMethod)counter;
        //Debug.Log("switch to " + m_CurrentMode);
    }

    private IEnumerator FadeBetweenControlModes()
    {
      
        //Fade to black
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        //Apply trabslation
        yield return new WaitForSeconds(m_FadeTime);
       
        //Fade to clear
        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

    }
}
