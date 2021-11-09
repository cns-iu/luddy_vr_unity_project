using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SetUIOnControlSwitch : MonoBehaviour
{

    public Text m_NavTooltip;
    public Text m_NavIndicator;
    public GameObject m_TooltipSwitchMode;
    public Image IndicatorPanel;
    public Color m_AnimationInactive;
    public Color m_AnimationActive;
    public float m_FadeTime = .2f;


    private void Awake()
    {
        m_TooltipSwitchMode.SetActive(false);
    }

    private void OnEnable()
    {
        FlowManager.UpdatePossibleNavMethodEvent += SetTooltip;
        FlowManager.UpdatePossibleNavMethodEvent += ActivateSelectionToolTip;
        RadialMenu.SetUserSelectedNavMethodEvent += SetTooltipOnUserSelection;
    }

    private void OnDisable()
    {
        FlowManager.UpdatePossibleNavMethodEvent -= SetTooltip;
        FlowManager.UpdatePossibleNavMethodEvent -= ActivateSelectionToolTip;
        RadialMenu.SetUserSelectedNavMethodEvent -= SetTooltipOnUserSelection;
    }

    void SetTooltip(PossibleNavigations currentNavMethod)
    {
        switch (currentNavMethod)
        {
            case PossibleNavigations.Walk:
                m_NavTooltip.text = "Touch to " + currentNavMethod.ToString().ToLower();
                break;
            case PossibleNavigations.Teleport:
                m_NavTooltip.text = "Click to " + currentNavMethod.ToString().ToLower();
                break;
            case PossibleNavigations.Freefly:
                m_NavTooltip.text = "Touch to " + currentNavMethod.ToString().ToLower();
                break;
            case PossibleNavigations.All:
                m_NavTooltip.text = "";
                FlowManager.UpdatePossibleNavMethodEvent -= SetTooltip;
                break;
            default:
                break;
        }
        StartCoroutine(AnimateIndicator());
        SetNavIndicator(currentNavMethod);
    }

    void SetTooltipOnUserSelection(ActiveNavigation activeNavigation)
    {
        switch (activeNavigation)
        {
            case ActiveNavigation.Walk:
                m_NavTooltip.text = "Touch to " + activeNavigation.ToString().ToLower();
                break;
            case ActiveNavigation.Teleport:
                m_NavTooltip.text = "Click to " + activeNavigation.ToString().ToLower();
                break;
            case ActiveNavigation.Freefly:
                m_NavTooltip.text = "Touch to " + activeNavigation.ToString().ToLower();
                break;
            default:
                break;
        }
        //StartCoroutine(AnimateIndicator());
        SetNavIndicatorOnUserSelection(activeNavigation);
    }

    IEnumerator AnimateIndicator()
    {
        IndicatorPanel.color = m_AnimationActive;
        yield return new WaitForSeconds(m_FadeTime);
        IndicatorPanel.color = m_AnimationInactive;
    }

    void SetNavIndicator(PossibleNavigations currentNavMethod)
    {
        m_NavIndicator.text = currentNavMethod.ToString();
    }

    void SetNavIndicatorOnUserSelection(ActiveNavigation activeNavigation)
    {
        m_NavIndicator.text = activeNavigation.ToString();
    }

    void ActivateSelectionToolTip(PossibleNavigations currentNavMethod)
    {
        m_TooltipSwitchMode.SetActive(currentNavMethod == PossibleNavigations.All);
    }

}
