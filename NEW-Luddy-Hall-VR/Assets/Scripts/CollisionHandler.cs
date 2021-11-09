using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    public Material m_Inactive;
    public Material m_Active;
    public Material m_Confirmed;
    public float m_ConfirmDuration = 1.2f;
    public Slider m_ProgressSlider;
    public GameObject m_PanelSubmissionInProgress;

    private bool m_isHandOnButton = false;
    private float m_elapsedTime;
    private Renderer m_Renderer;

    public delegate void OnSubmission();
    public static event OnSubmission OnSubmissionEvent;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameController")
        {
            m_Renderer.material = m_Active;
            m_isHandOnButton = true;
            StartCoroutine(CountdownToConfirmed());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        m_isHandOnButton = false;
        m_Renderer.material = m_Inactive;
    }

    IEnumerator CountdownToConfirmed()
    {

        m_ProgressSlider.gameObject.SetActive(true);
        m_PanelSubmissionInProgress.SetActive(true);
        //Debug.Log("beginning countdown");

        while (m_elapsedTime <= m_ConfirmDuration)
        {
            if (m_isHandOnButton)
            {
                m_ProgressSlider.value = (m_elapsedTime + Time.deltaTime) / m_ConfirmDuration;
                m_elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                break;
            }
        }
        if (m_ProgressSlider.value == m_ProgressSlider.maxValue)
        {
            OnSubmissionEvent?.Invoke();
            m_Renderer.material = m_Inactive;
        }
        m_elapsedTime = 0f;
        m_ProgressSlider.value = 0f;
        m_ProgressSlider.gameObject.SetActive(false);
        m_PanelSubmissionInProgress.SetActive(false);
    }

}
