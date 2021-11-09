using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class ActionsTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean teleportAction; // 2
    public SteamVR_Action_Boolean grabAction; // 3
    public SteamVR_Action_Boolean snapTurnRight;
    public SteamVR_Action_Boolean snapTurnLeft;
    public SteamVR_Action_Vector2 freeFly;


    // Update is called once per frame
    void Update()
    {
        Debug.Log(freeFly.GetAxis(handType));
        //if (GetTeleportDown())
        //{
        //    print("Teleport " + handType);
        //}

        //if (GetGrab())
        //{
        //    print("Grab " + handType);
        //}

        //if (GetSnapTurnLeft())
        //{
        //    Debug.Log("turn left");
        //}

        //if (GetSnapTurnRight())
        //{
        //    Debug.Log("turn right");
        //}

    }

    //public bool GetTeleportDown() // 1
    //{
    //    return teleportAction.GetStateDown(handType);
    //}

    //public bool GetGrab() // 2
    //{
    //    return grabAction.GetState(handType);
    //}

    //public bool GetSnapTurnLeft() // 2
    //{
    //    return snapTurnLeft.GetState(handType);
    //}

    //public bool GetSnapTurnRight() // 2
    //{
    //    return snapTurnRight.GetState(handType);
    //}

}
