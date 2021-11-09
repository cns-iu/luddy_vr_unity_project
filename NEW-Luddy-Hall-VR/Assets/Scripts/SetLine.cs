using UnityEngine;

public class SetLine : MonoBehaviour
{
    LineRenderer m_LineRenderer;
    public Transform m_StartLine;
    public Transform m_EndLine;




    // Start is called before the first frame update
    void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_LineRenderer.SetPosition(0, m_StartLine.position);
        m_LineRenderer.SetPosition(1, m_EndLine.position);
    }
}
