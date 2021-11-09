using UnityEngine;
using UnityEngine.UI;

public class ShowHideAll : MonoBehaviour
{
    public Toggle[] m_Toggles;
    public bool m_IsOnButton;

    private Button m_ThisButton;

    // Start is called before the first frame update
    private void Start()
    {
        m_ThisButton = GetComponent<Button>();
        //m_ThisButtonText.text

        m_ThisButton.onClick.AddListener(delegate
        {
            foreach (var item in m_Toggles)
            {
                item.isOn = m_IsOnButton;
            }
        }
        );
    }
}