using UnityEngine;
using UnityEngine.UI;

public class ShowHideAllToggle : MonoBehaviour
{
    public Toggle[] m_TogglesToSet = new Toggle[6];

    // Start is called before the first frame update
    private void Start()
    {
        Toggle thisToggle = GetComponent<Toggle>();
        thisToggle.onValueChanged.AddListener(delegate
        {
            foreach (var item in m_TogglesToSet)
            {
                item.isOn = GetComponent<Toggle>().isOn;
            }
        });
    }
}