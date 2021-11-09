using UnityEngine;
using Valve.VR;

public class FreeFlyer : MonoBehaviour
{
    public GameObject m_Goal;
    public SteamVR_Action_Vector2 m_freeFlyAction;
    public float speedPerSecond = 2f;

    public delegate void FreeFly(string eventType, string side, string status);
    public static event FreeFly FreeFlyEvent;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private Vector2 speedVector;
    private float currentUserInputValue;
    private float currentSpeed;

    

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    private void Update()
    {
        speedVector = m_freeFlyAction.GetAxis(m_Pose.inputSource);
        currentUserInputValue = speedVector[1];
        currentSpeed = speedPerSecond * currentUserInputValue * Time.deltaTime;
        Ray ray = Ray();
        TryFreeFly(currentSpeed, ray);
    }

    private void TryFreeFly(float speed, Ray ray)
    {
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;
        //Debug.Log("Ray direction: " + ray.direction);
        //cameraRig.transform.position = Vector3.MoveTowards(cameraRig.transform.position, ray.direction, speed);
        cameraRig.transform.Translate(ray.direction * speed, Space.World);
        FreeFlyEvent?.Invoke("freefly", "right", "inProgress");
    }

    private Ray Ray()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
        return ray;
    }
}