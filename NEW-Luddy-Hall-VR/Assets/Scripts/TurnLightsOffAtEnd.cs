using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightsOffAtEnd : MonoBehaviour
{
   
    private void OnEnable()
    {
        FlowManager.EndOfSessionEvent += SetInactive;
    }

    private void OnDisable()
    {
        FlowManager.EndOfSessionEvent -= SetInactive;
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
