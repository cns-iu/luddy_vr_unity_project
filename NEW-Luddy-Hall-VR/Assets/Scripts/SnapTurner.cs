using System.Collections;
using UnityEngine;
using Valve.VR;

public class SnapTurner : MonoBehaviour
{
    public SteamVR_Action_Boolean m_SnapTurnLeftAction;
    public SteamVR_Action_Boolean m_SnapTurnRightAction;

    private float m_rotationAngle = 25f;
    private bool m_isTurning;
    private float m_FadeTime = .2f;
    private SteamVR_Behaviour_Pose m_Pose = null;
    public bool isSnapTurnAllowed = true;

    void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    void Update()
    {
        //Teleport
        if (m_SnapTurnLeftAction.GetLastStateDown(m_Pose.inputSource) && isSnapTurnAllowed)
        {
            //Debug.Log("snapping left");
            TrySnapTurn(-m_rotationAngle);
        }
        if ((m_SnapTurnRightAction.GetLastStateDown(m_Pose.inputSource)) && isSnapTurnAllowed)
        {
            //Debug.Log("snapping right");
            TrySnapTurn(m_rotationAngle);
        }
    }

    public void TrySnapTurn(float dir)
    {
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;

        StartCoroutine(TurnCamera(cameraRig, new Vector3(0f,dir,0f)));
    }

    private IEnumerator TurnCamera(Transform cameraRig, Vector3 rotation)
    {
        //Flag 
        m_isTurning = true;

        //Fade to black
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        //Apply trabslation
        yield return new WaitForSeconds(m_FadeTime);
        cameraRig.Rotate(rotation);

        //Fade to clear
        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

        //de-flag
        m_isTurning = false;
    }
}
