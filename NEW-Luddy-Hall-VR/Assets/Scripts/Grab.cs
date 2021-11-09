using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Grab : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    //public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    private GameObject collidingObject; // 1
    private GameObject objectInHand; // 2

    public delegate void GrabAction();
    public static event GrabAction GrabActionEvent;
    public delegate void ReleaseAction();
    public static event ReleaseAction ReleaseActionEvent;

    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        collidingObject = col.gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        // 1
        if (grabAction.GetLastStateDown(handType))
        {
          
            if (collidingObject)
            {
                GrabObject();
                if (GrabActionEvent != null) GrabActionEvent(); 
            }
        }

        // 2
        if (grabAction.GetLastStateUp(handType))
        {
            
            if (objectInHand)
            {
                ReleaseObject();
                if (ReleaseActionEvent != null) ReleaseActionEvent();
            }
            //print("Released");
        }


    }

    // 1
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
       
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        //Debug.Log("Staying");
        SetCollidingObject(other);

    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        //if (ControllerUseEvent != null) ControllerUseEvent("grab", handType.ToString(), "down");
        // 1
        objectInHand = collidingObject;
        collidingObject = null;
        // 2
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        //Debug.Log("Entered");


    }



    // 3
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {

        // 1
        if (GetComponent<FixedJoint>())
        {
            // 2
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // 3
            //objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            //objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
            objectInHand.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            objectInHand.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);

        }
        // 4
        objectInHand = null;
    }


}
