using System.Collections;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{
    public GameObject m_Pointer;
    public SteamVR_Action_Boolean m_TeleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_HasPosition = false;
    private bool m_IsTeleporting = false;
    private float m_FadeTime = 0.5f;

    public LineRenderer m_LineRenderer;

    public delegate void Teleport(string eventType, string side, string status);

    public static event Teleport TeleportEvent;

    public void TestMethod()
    {
    }

    // Start is called before the first frame update
    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        SwitchControl.ControlSwitchHandlerEvent += OnControlModeSwitchSetPointerInactive;
    }

    private void OnDisable()
    {
        SwitchControl.ControlSwitchHandlerEvent -= OnControlModeSwitchSetPointerInactive;
        m_Pointer.SetActive(false);
        m_LineRenderer.enabled = false;
    }

    private void OnControlModeSwitchSetPointerInactive(NavMethod currentMode)
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //Pointer
        m_HasPosition = UpdatePointer();
        m_Pointer.SetActive(m_HasPosition);
        SetLine();

        //Teleport
        if (m_TeleportAction.GetLastStateUp(m_Pose.inputSource))
        {
            TryTeleport();
        }
    }

    private void SetLine()
    {
        m_LineRenderer.enabled = m_HasPosition;
        if (m_HasPosition)
        {
            m_LineRenderer.startColor = Color.blue;
            m_LineRenderer.endColor = Color.blue;
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, m_Pointer.transform.position);
        }
    }

    private void TryTeleport()
    {
        //check for valid pos and if we are teleporting
        if (m_HasPosition == false || m_IsTeleporting)
        {
            return;
        }
        // get camera rig and head position
        Transform cameraRig = SteamVR_Render.Top().origin;
        Vector3 headPosition = SteamVR_Render.Top().head.position;
        // figure out translation
        Vector3 groundPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
        Vector3 translateVector = m_Pointer.transform.position - groundPosition;
        //move
        StartCoroutine(MoveRig(cameraRig, translateVector));
    }

    private IEnumerator MoveRig(Transform m_CameraRig, Vector3 translation)
    {
        //Debug.Log("[Teleporter] m_CameraRig.position: " + m_CameraRig.position);
        //Debug.Log("[Teleporter] translation: " + translation);
        //Flag
        m_IsTeleporting = true;

        //Fade to black
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        //Apply trabslation
        yield return new WaitForSeconds(m_FadeTime);
        TeleportEvent?.Invoke("teleport", "right", "inProgress");
        m_CameraRig.position += translation;

        //Fade to clear
        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);

        //de-flag
        m_IsTeleporting = false;
    }

    private bool UpdatePointer()
    {
        //Debug.Log("UpdatePointer() called");
        //Ray from controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        // if hit
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "TeleportSurface")
            {
                m_Pointer.transform.position = hit.point;
                return true;
            }
        }

        //if not a hit
        return false;
    }

    private void RemoveLineOnNavMethodChange(NavMethod currentMode)
    {
        m_HasPosition = false;
    }
}