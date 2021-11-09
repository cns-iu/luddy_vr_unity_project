using System.Collections;
using UnityEngine;
using Valve.VR;

public class RightHandControls : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    
    public SteamVR_Action_Boolean grabAction; // 3
    public SteamVR_Action_Boolean menuAction;

    public GameObject m_Pointer;
    public SteamVR_Action_Boolean m_TeleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_HasPosition = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TryTeleport())
        {
            Debug.Log("teleporting now");
        }
        if (GetGrab())
        {
            Debug.Log("grabbing now");
        }
        if (GetMenu())
        {
            Debug.Log("menu now");
        }

        //Pointer
        m_HasPosition = UpdatePointer();
        m_Pointer.SetActive(m_HasPosition);

        //Teleport
        if (m_TeleportAction.GetLastStateUp(m_Pose.inputSource))
        {
            TryTeleport();
        }

    }

    bool TryTeleport()
    {
        return m_TeleportAction.GetStateDown(handType);
    }

    bool GetGrab()
    {
        return grabAction.GetStateDown(handType);
    }

    bool GetMenu()
    {
        return menuAction.GetStateDown(handType);
    }



    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
    {
        yield return null;
    }

    private bool UpdatePointer()
    {
        Debug.Log("UpdatePointer() called");
        //Ray from controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        // if hit
        if (Physics.Raycast(ray, out hit))
        {
            m_Pointer.transform.position = hit.point;
            return true;
        }

        //if not a hit
        return false;
    }
}
