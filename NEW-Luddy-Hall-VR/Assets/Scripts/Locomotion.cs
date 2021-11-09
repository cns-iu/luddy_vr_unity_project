using UnityEngine;
using Valve.VR;

public class Locomotion : MonoBehaviour
{
    public float m_Gravity = 30f;
    public float m_Sensitivity = 0.1f;
    public float m_maxSpeed = 1f;
    public SteamVR_Action_Vector2 m_MoveValue = null;
    public SteamVR_Behaviour_Pose m_Pose = null;

    public delegate void Walk(string eventType, string side, string status);
    public static event Walk WalkEvent;

    float m_Speed = 0f;
    public CharacterController m_CharacterController = null;
    Transform m_CameraRig = null;
    Transform m_Head = null;

    private void OnEnable()
    {
        m_CharacterController.enabled = true;
    }

    private void OnDisable()
    {
        m_CharacterController.enabled = false;
    }

    private void Start()
    {
        m_CameraRig = SteamVR_Render.Top().origin;
        m_Head = SteamVR_Render.Top().head;
    }

    private void Update()
    {
        HandleHeight();
        CalculateMovement();
    }

    public void HandleHeight()
    {
        //get the head in local space
        float headHeight = Mathf.Clamp(m_Head.localPosition.y, 1, 2);
        m_CharacterController.height = headHeight;

        //cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = m_CharacterController.height / 2;
        newCenter.y += m_CharacterController.skinWidth;

        //move capsule in local space
        newCenter.x = m_Head.localPosition.x;
        newCenter.z = m_Head.localPosition.z;

        ////rotate
        //newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        //apply
        m_CharacterController.center = newCenter;
    }

    void CalculateMovement()
    {
        //figure out movement orientation
        Vector3 movement = Vector3.zero;
        Quaternion orientation = CalculateOrientation();

        //if not moving
        if (m_MoveValue.axis.magnitude == 0)
        {
            m_Speed = 0;
        }
        //add clamp
        //Debug.Log(m_MoveValue.GetAxis(m_Pose.inputSource).magnitude);
        m_Speed = m_MoveValue.GetAxis(m_Pose.inputSource).magnitude * m_maxSpeed;
        //m_Speed = Mathf.Clamp(m_Speed, -m_maxSpeed, m_maxSpeed);
        //orientation
        movement += orientation * (m_Speed * Vector3.forward);

        //gravity
        movement.y -= m_Gravity * Time.deltaTime;

        //apply
        m_CharacterController.Move(movement * Time.deltaTime);
        WalkEvent?.Invoke("walk", "right", "inProgress");
    }

    Quaternion CalculateOrientation()
    {
        float rotation = Mathf.Atan2(m_MoveValue.axis.x, m_MoveValue.axis.y);
        rotation *= Mathf.Rad2Deg;
        Vector3 orientationEuler = new Vector3(0f, m_Head.eulerAngles.y + rotation, 0f);
        return Quaternion.Euler(orientationEuler);
    }

    //void SnapRotation()
    //{
    //    float snapValue = 0f;
    //    if (true)
    //    {

    //    }
    //}



}
