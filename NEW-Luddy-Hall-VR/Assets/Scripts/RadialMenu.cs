using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [Header("Scene")]
    public Transform m_SelectionTransform = null;
    public Transform m_CursorTransform = null;
    public AudioManager m_AudioManager;
    public int index;

    [Header("Events")]
    public RadialSection m_Top = null;
    public RadialSection m_Right = null;
    public RadialSection m_Bottom = null;
    public RadialSection m_Left = null;

    Vector2 m_TouchPosition = Vector2.zero;
    List<RadialSection> m_RadialSections = null;
    public RadialSection m_HighlightedSelection;
    readonly float m_DegreeIncrement = 90;
    //bool m_IsSelectionAllowed = false;

    ActiveNavigation activeNavigation;
    //public FlowManager m_FlowManager;

    public delegate void SetUserSelectedNavMethod(ActiveNavigation activeNavigation);
    public static event SetUserSelectedNavMethod SetUserSelectedNavMethodEvent;

    public delegate void UserChoice(string eventType, string side, string status);
    public static event UserChoice UserChoiceEvent;

    private void OnEnable()
    {
        //FlowManager.UpdateNavMethodEvent += CheckIfSelectionAllowed;
    }

    private void OnDisable()
    {
        //FlowManager.UpdateNavMethodEvent -= CheckIfSelectionAllowed;
    }

    private void Awake()
    {
        CreateAndSetupSections();
    }

    void CreateAndSetupSections()
    {
        m_RadialSections = new List<RadialSection>()
        {
            m_Top,
            m_Right,
            m_Bottom,
            m_Left
        };

        foreach (RadialSection section in m_RadialSections)
        {
            section.m_IconRenderer.sprite = section.m_Icon;
        }

    }

    private void Start()
    {
        //Show(false);
    }

    public void Show(bool value)
    {
        //if (m_IsSelectionAllowed)
        //{
        gameObject.SetActive(value);
        m_AudioManager.m_MenuUpSound.Play();
        
        //}
    }

    private void Update()
    {
        Vector2 direction = Vector2.zero + m_TouchPosition;
        float rotation = GetDegree(direction);

        SetCursorPosition();
        SetSelectionRotation(rotation);
        SetSelectedEvent(rotation);

    }

    float GetDegree(Vector2 direction)
    {
        float value = Mathf.Atan2(direction.x, direction.y);
        value *= Mathf.Rad2Deg;

        if (value < 0f)
            value += 360;

        return value;
    }

    void SetCursorPosition()
    {
        m_CursorTransform.localPosition = m_TouchPosition;
    }

    void SetSelectionRotation(float newRotation)
    {
        float snappedRotation = SnapRotation(newRotation);
        m_SelectionTransform.localEulerAngles = new Vector3(0f, 0f, -snappedRotation);
    }

    float SnapRotation(float rotation)
    {
        return GetNearestIncrement(rotation) * m_DegreeIncrement;
    }

    int GetNearestIncrement(float rotation)
    {
        return Mathf.RoundToInt(rotation / m_DegreeIncrement);
    }

    void SetSelectedEvent(float currentRotation)
    {
        index = GetNearestIncrement(currentRotation);

        if (index == 4)
        {
            index = 0;
        }

        m_HighlightedSelection = m_RadialSections[index];
    }

    public void SetTouchPosition(Vector2 newValue)
    {
        m_TouchPosition = newValue;
    }

    public void ActivateHighlightedSection()
    {
        m_HighlightedSelection.m_OnPress.Invoke();
        SetNavMethodFromSelectedEvent(index);
        m_AudioManager.m_SelectionSound.Play();
        
        //Debug.Log("onpress called");
        //RadialSection.OnSelectEvent(m_FlowManager);
    }

    void SetNavMethodFromSelectedEvent(int index)
    {
        switch (index)
        {
            case 0:
                activeNavigation = ActiveNavigation.Teleport;
                break;
            case 1:
                activeNavigation = ActiveNavigation.Freefly;
                break;
            case 2:
                break;
            case 3:
                activeNavigation = ActiveNavigation.Walk;
                break;
            default:
                break;
        }
        SetUserSelectedNavMethodEvent?.Invoke(activeNavigation);
        UserChoiceEvent?.Invoke("newUserChoice", "left", "inProgress");
        //Debug.Log(activeNavigation);
    }
}
