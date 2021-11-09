using UnityEngine;

public class TaskControl : MonoBehaviour
{
    public ParticleSystem m_ParticleSystem;
    public Transform m_Lever;
    public Transform m_Max;
    public Transform m_Min;
    public GameObject m_IndicatorCube;
    public float m_LeverValue;
    [Range(0, 1)]
    public float m_TargetLeverNumber;

    Material m_IndicatorCubeMaterial;
    float m_DefaultLeverPosition;

    private void OnEnable()
    {
        CollisionHandler.OnSubmissionEvent += ResetLever;
    }

    private void OnDisable()
    {
        CollisionHandler.OnSubmissionEvent -= ResetLever;
    }

    // Update is called once per frame
    void Awake()
    {
        m_DefaultLeverPosition = (m_Max.transform.position.y + m_Min.transform.position.y) / 2;
        m_IndicatorCubeMaterial = m_IndicatorCube.GetComponent<Renderer>().sharedMaterial;
        m_IndicatorCubeMaterial.color = SetColorFromT(m_TargetLeverNumber);
    }

    void ResetLever()
    {
        m_Lever.position = new Vector3(m_Lever.position.x, m_DefaultLeverPosition, m_Lever.position.z);
    }

    void Update()
    {
        var main = m_ParticleSystem.main;
        float d = Mathf.Abs(m_Lever.position.y - m_Min.position.y);
        m_LeverValue = Remap(
            d,
            0f, Mathf.Abs(Vector3.Distance(m_Max.position, m_Min.position)),
            0,
            1);
        main.startColor = SetColorFromT(m_LeverValue);
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    Color SetColorFromT(float t)
    {
        return Color.Lerp(Color.blue, Color.red, t);
    }
}
