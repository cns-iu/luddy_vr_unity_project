using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimedEventTrigger : MonoBehaviour
{
    public GameObject marker;
    public Canvas ca;
    public ParticleSystem ps;
    public GameObject sensor;
    //public float waitTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            marker.SetActive(true);
            ca.gameObject.SetActive(true);
            ps.gameObject.SetActive(true);
        }
    }
}
