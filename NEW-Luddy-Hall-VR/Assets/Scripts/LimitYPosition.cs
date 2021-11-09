using UnityEngine;

public class LimitYPosition : MonoBehaviour
{
    public Transform m_Max;
    public Transform m_Min;

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y > m_Max.position.y)
        {
            this.transform.position = new Vector3(
                transform.position.x,
                m_Max.position.y,
                transform.position.z
                );
            //Debug.Log("out of bounds");
        }
        else if (this.transform.position.y < m_Min.position.y)
        {
            this.transform.position = new Vector3(
                transform.position.x,
                m_Min.position.y,
                transform.position.z
                );
            //Debug.Log("out of bounds");
        }
    }


}
