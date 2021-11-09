using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrushAndLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color m_InactiveColor;
    public Color m_BrushedColor;
    public GameObject m_Background;

    public delegate void Brush(bool isOn, int taskNumber);

    public static event Brush BrushEvent;

    public delegate void Click(bool isOn, int taskNumber);

    public static event Click ToggleClickEvent;

    private Toggle m_Toggle;

    private void OnEnable()
    {
        Highlight.BarBrushEvent += SetHighlightOnBarBrush;
    }

    private void OnDestroy()
    {
        Highlight.BarBrushEvent -= SetHighlightOnBarBrush;
    }

    private void SetHighlightOnBarBrush(bool isBrushed, int taskNumber)
    {
        //m_Toggle.isOn = DetermineTaskNumber() == taskNumber;
        if (taskNumber == DetermineTaskNumber())
        {
            SetAction(isBrushed);
        }
    }

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleClickEvent?.Invoke(m_Toggle.isOn, DetermineTaskNumber());
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        int taskNumber = GetComponent<ToggleEventHandler>().m_TaskNumber;
        SetAction(false);
        BrushEvent?.Invoke(false, DetermineTaskNumber());
    }

    private void SetAction(bool isActive)
    {
        transform.GetChild(2).GetComponent<Text>().color = m_InactiveColor;
        transform.GetChild(2).GetComponent<Text>().text = transform.GetChild(2).GetComponent<Text>().text;
        m_Background.GetComponent<Image>().enabled = isActive;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SetAction(true);
        BrushEvent?.Invoke(true, DetermineTaskNumber());
    }

    private int DetermineTaskNumber()
    {
        return GetComponent<ToggleEventHandler>().m_TaskNumber;
    }
}