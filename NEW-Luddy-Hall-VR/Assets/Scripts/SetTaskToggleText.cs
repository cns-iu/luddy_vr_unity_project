using UnityEngine;
using UnityEngine.UI;

public class SetTaskToggleText : MonoBehaviour
{
    public Text m_Text;

    // Start is called before the first frame update
    private void Awake()
    {
        m_Text.text = "Task " + (GetComponent<ToggleEventHandler>().m_TaskNumber + 1).ToString();
    }
}