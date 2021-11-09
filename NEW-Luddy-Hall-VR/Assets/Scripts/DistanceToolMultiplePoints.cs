using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class DistanceToolMultiplePoints : MonoBehaviour
{
    public List<GameObject> m_Waypoints = new List<GameObject>();
    public float[] m_Distances;
    public float m_TotalDistance;

    private Vector3 m_StartPoint;
    private Vector3 m_EndPoint;

    void OnDrawGizmos()
    {
        m_TotalDistance = 0f;
        m_Distances = new float[m_Waypoints.Count];
        //Debug.Log(m_Waypoints.Count);
        Gizmos.color = Color.red;
        for (int i = 1; i < m_Waypoints.Count; i++)
        {
            Vector3 substart = m_Waypoints[i - 1].transform.position;
            Vector3 subend = m_Waypoints[i].transform.position;

            Gizmos.DrawLine(substart, subend);
            float distance = Vector3.Distance(substart, subend);
            m_Distances[i] = distance;
        }

        for (int i = 0; i < m_Distances.Length; i++)
        {
            m_TotalDistance += m_Distances[i];
        }
        //Vector3 previous = m_Waypoints[i - 1].transform.position;
        //Gizmos.DrawLine(m_Waypoints[0].transform.position, m_Waypoints[1].transform.position);
        //float distance1 = Vector3.Distance(m_Waypoints[0].transform.position, m_Waypoints[1].transform.position);
        //Gizmos.DrawLine(m_Waypoints[1].transform.position, m_Waypoints[2].transform.position);
        //float distance2 = Vector3.Distance(m_Waypoints[1].transform.position, m_Waypoints[2].transform.position);
        //Debug.Log(distance1 + distance2);
        //Debug.Log(Vector3.Distance(m_Waypoints[0].transform.position, m_Waypoints[1].transform.position));
        //Gizmos.DrawWireSphere(startPoint, gizmoRadius);
        //Gizmos.DrawWireSphere(endPoint, gizmoRadius);
        //Gizmos.DrawLine(startPoint, endPoint);
    }
}
