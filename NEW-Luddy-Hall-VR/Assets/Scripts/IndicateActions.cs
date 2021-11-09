using UnityEngine;
using UnityEngine.UI;

public class IndicateActions : MonoBehaviour
{
    public Text m_Action;

    private void OnEnable()
    {
        SwitchControl.ControlSwitchHandlerEvent += SetText;
    }

    private void OnDestroy()
    {
        SwitchControl.ControlSwitchHandlerEvent -= SetText;
    }

    void SetText(NavMethod currentMode)
    {
        string result = "";
        switch (currentMode)
        {
            case NavMethod.Walk:
                result = "Touch to ";
                break;
            case NavMethod.Teleport:
                result = "Click to ";
                break;
            case NavMethod.Freefly:
                result = "Touch to ";
                break;
            default:
                break;
        }
        m_Action.text = result;
    }
}
