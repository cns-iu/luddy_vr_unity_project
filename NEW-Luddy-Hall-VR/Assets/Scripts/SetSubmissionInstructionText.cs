using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSubmissionInstructionText : MonoBehaviour
{
    public GameObject m_SubmitButton;
    
    // Start is called before the first frame update
    void Start()
    {
        Text text = GetComponent<Text>();
        float submitTime = m_SubmitButton.GetComponent<CollisionHandler>().m_ConfirmDuration;
        string numSeconds;
        if (submitTime != 1f)
        {
            numSeconds = "seconds";
        }
        else
        {
            numSeconds = "second";
        }
        text.text = "To finish this task, hold your controller against this sphere for " + submitTime + " " + numSeconds + ".";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
