using UnityEngine;
using Valve.VR;

public class ScaleVisualization : MonoBehaviour
{
    public GameObject m_VisualizationParent;
    public float m_SmoothingFactor = 0.02f;
    public float[] m_MinMaxScales = new float[2];
    public SteamVR_Action_Vector2 m_TouchPadTouchVertical;
    // Start is called before the first frame update
    void OnEnable()
    {
        m_TouchPadTouchVertical.onAxis += PerformScaling;
    }

    void PerformScaling(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        //Vector3 oldScale = m_VisualizationParent.transform.localScale;
        //Vector3 newScale = new Vector3(
        //    oldScale.x + axis.y,
        //    oldScale.y + axis.y,
        //    oldScale.z + axis.y
        //    ) * m_SmoothingFactor;
        //if (newScale.x < m_MinMaxScales[0])
        //{
        //    newScale = new Vector3(
        //    m_MinMaxScales[0],
        //    m_MinMaxScales[0],
        //   m_MinMaxScales[0]
        //    ) ;
        //}
        //else if (newScale.x > m_MinMaxScales[1])
        //{
        //    newScale = new Vector3(
        //   m_MinMaxScales[1],
        //    m_MinMaxScales[1],
        //    m_MinMaxScales[1]
        //    ) ;
        //}
        m_VisualizationParent.transform.localScale = new Vector3(
            Mathf.Lerp(m_MinMaxScales[0], 
            m_MinMaxScales[1],
            axis.y),
            Mathf.Lerp(m_MinMaxScales[0],
            m_MinMaxScales[1],
            axis.y),
            Mathf.Lerp(m_MinMaxScales[0],
            m_MinMaxScales[1],
            axis.y)
            ); 
            

    }
}
