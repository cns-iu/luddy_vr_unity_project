using UnityEngine;
using UnityEngine.UI;

public class ClickEventHandler : MonoBehaviour
{
    public Button m_Button;

    public delegate void ButtonClick(string buttonName, string state);

    public static event ButtonClick ButtonClickEvent;

    private void Start()
    {
        m_Button = GetComponent<Button>();

        m_Button.onClick.AddListener(delegate
        {
            ButtonClickEvent?.Invoke(gameObject.name, true.ToString());
        });
    }
}