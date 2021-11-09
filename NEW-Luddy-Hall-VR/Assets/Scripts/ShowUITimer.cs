using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowUITimer : MonoBehaviour
{
    public CanvasGroup m_CanvasGroup;
    public float duration = 2f;

    private void OnEnable()
    {
        SwitchControl.ControlSwitchHandlerEvent += StartTimer;
    }

    private void OnDisable()
    {
        SwitchControl.ControlSwitchHandlerEvent -= StartTimer;
    }

    void StartTimer(NavMethod currentMode)
    {
        StartCoroutine(Timer(duration));
    }

    IEnumerator Timer(float duration) {
        m_CanvasGroup.alpha = 1;
        yield return new WaitForSeconds(duration);
        m_CanvasGroup.alpha = 0;
    }

}
