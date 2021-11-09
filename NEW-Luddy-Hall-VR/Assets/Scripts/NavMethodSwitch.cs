using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class NavMethodSwitch : MonoBehaviour
{
    public SnapTurner m_SnapTurner;
    public Teleporter m_Teleporter;
    public Locomotion m_Locomotion;
    public FreeFlyer m_FreeFlyer;

    public SteamVR_Action_Boolean m_NavMethodSwitch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_NavMethodSwitch.GetState(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("pop menu up");
        }
    }
}
