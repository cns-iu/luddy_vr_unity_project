using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Material m_Pink;
    public Material m_Grey;

    public void SetPink()
    {
        SetMaterial(m_Pink);
        Debug.Log("called pink");
    }
    public void SetGrey()
    {
        SetMaterial(m_Grey);
        Debug.Log("called grey");
    }

    void SetMaterial(Material newMaterial)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = newMaterial;
    }
}
