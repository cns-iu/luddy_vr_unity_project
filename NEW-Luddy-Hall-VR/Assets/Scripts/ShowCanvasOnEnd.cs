using UnityEngine;

public class ShowCanvasOnEnd : MonoBehaviour
{
    CanvasGroup m_CanvasGroup;

    private void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        FlowManager.EndOfSessionEvent += SetActive;
    }

    private void OnDisable()
    {
        FlowManager.EndOfSessionEvent -= SetActive;
    }

    void SetActive()
    {
        m_CanvasGroup.alpha = 1;
    }
}
