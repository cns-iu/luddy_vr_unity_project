using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialOnInput : MonoBehaviour
{
    public Material m_MaterialActive;
    public Material m_MaterialInactive;

    private void OnEnable()
    {
        Grab.GrabActionEvent += SetMaterialActive;
        Grab.ReleaseActionEvent += SetMaterialInactive;
    }

    private void OnDisable()
    {
        Grab.GrabActionEvent -= SetMaterialActive;
        Grab.ReleaseActionEvent -= SetMaterialInactive;
    }

    void SetMaterialActive()
    {
        this.GetComponent<Renderer>().material = m_MaterialActive;
    }

    void SetMaterialInactive()
    {
        this.GetComponent<Renderer>().material = m_MaterialInactive;
    }
}
