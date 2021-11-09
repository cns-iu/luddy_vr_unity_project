using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LeftHandControls : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean snapTurnLeftAction;
    public SteamVR_Action_Boolean snapTurnRightAction;
    public SteamVR_Action_Boolean menuAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetSnapTurnLeft())
        {
            Debug.Log("GetSnapTurnLeft");
        }
        if (GetSnapTurnRight())
        {
            Debug.Log("GetSnapTurnRight");
        }
        if (GetMenu())
        {
            Debug.Log("GetMenu");
        }
    }

     bool GetSnapTurnLeft()
    {
        return snapTurnLeftAction.GetStateDown(handType);
    }

    bool GetSnapTurnRight()
    {
        return snapTurnRightAction.GetStateDown(handType);
    }

    bool GetMenu()
    {
        return menuAction.GetStateDown(handType);
    }
}
