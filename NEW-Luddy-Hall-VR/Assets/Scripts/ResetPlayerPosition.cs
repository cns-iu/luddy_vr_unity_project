using System.Collections;
using UnityEngine;
using Valve.VR;

public class ResetPlayerPosition : MonoBehaviour
{
    public Transform m_SpawnPosition;
    public Transform m_CameraRig;
    public float m_FadeTime = .5f;
    public bool m_IsResettingActive = false;

    private void OnEnable()
    {
        TaskManager.UpdateTaskEvent += SetPlayerPosition;
        ConfigurationScript.ResetPlayerEvent += SetPlayerPosition;
    }

    private void OnDisable()
    {
        TaskManager.UpdateTaskEvent -= SetPlayerPosition;
        ConfigurationScript.ResetPlayerEvent -= SetPlayerPosition;
    }

    private void SetPlayerPosition(int taskNumber)
    {
        StartCoroutine(MovePlayer());
    }

    private IEnumerator MovePlayer()
    {
        m_IsResettingActive = true;
        //Fade to black
        SteamVR_Fade.Start(Color.black, m_FadeTime, true);

        //Apply trabslation
        yield return new WaitForSeconds(m_FadeTime);

        m_CameraRig.position = m_SpawnPosition.position;

        //yield return new WaitForSeconds(m_FadeTime);
        //Fade to clear
        SteamVR_Fade.Start(Color.clear, m_FadeTime, true);
        yield return new WaitForSeconds(.3f);
        m_IsResettingActive = false;
    }
}