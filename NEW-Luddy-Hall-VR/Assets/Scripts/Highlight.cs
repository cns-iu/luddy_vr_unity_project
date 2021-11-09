using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Highlight : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color m_InactiveColor;
    public Color m_BrushedColor;
    public Color m_ClickedColor;
    //public VisualizeCompletionTimes m_VisualizeCompletionTimes;
    public delegate void BarBrush(bool isBrushed, int taskNumber);
    public static event BarBrush BarBrushEvent;

    private int m_TaskNumber;
    private Image m_Image;
    bool m_IsCorrespondingToggleOn;

    private void OnEnable()
    {
        BrushAndLink.BrushEvent += SetHighlight;
        //BrushAndLink.ToggleClickEvent += SetSelectedColor;
    }



    private void OnDestroy()
    {
        BrushAndLink.BrushEvent -= SetHighlight;
        //BrushAndLink.ToggleClickEvent -= SetSelectedColor;
    }

    private void SetSelectedColor(bool isOn, int taskNumber)
    {
        //m_IsCorrespondingToggleOn = isOn;

        if (CompareTaskNumbers(taskNumber))
        {
            if (isOn)
            {
                m_Image.color = m_ClickedColor;
            }
            else
            {
                m_Image.color = m_InactiveColor;
            }
        }
    }

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Image.color = m_InactiveColor;
        m_TaskNumber = transform.transform.GetSiblingIndex();
    }

    private void SetHighlight(bool isOn, int taskNumber)
    {
        if (CompareTaskNumbers(taskNumber))
        {
            if (isOn)
            {
                m_Image.color = m_BrushedColor;
            }
            else
            {
                m_Image.color = m_InactiveColor;
            }
        }
    }

    private bool CompareTaskNumbers(int taskNumber)
    {
        return taskNumber == m_TaskNumber;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //m_Image.color = m_InactiveColor;
        //BarClickEvent?.Invoke(m_TaskNumber);
        //Debug.Log(m_TaskNumber);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (m_IsCorrespondingToggleOn == false)
        //{
            m_Image.color = m_BrushedColor;
            BarBrushEvent?.Invoke(true, m_TaskNumber);
        //}
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (m_IsCorrespondingToggleOn == false)
        //{
            m_Image.color = m_InactiveColor;
            BarBrushEvent?.Invoke(false, m_TaskNumber);
        //}
        
    }  
}