using UnityEngine;

public class HandleCollisionWithPlayer : MonoBehaviour
{
    MeshRenderer[] meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        TaskManager.UpdateTaskEvent += ActivateWaypoint;
    }

    private void OnDisable()
    {
        TaskManager.UpdateTaskEvent -= ActivateWaypoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        SetRendererAndCollider(false);
    }

    void ActivateWaypoint(int index)
    {
        SetRendererAndCollider(true);
    }

    void SetRendererAndCollider(bool isActive) {
        foreach (var item in meshRenderers)
        {
            item.enabled = isActive;
        }
        GetComponent<SphereCollider>().enabled = isActive;
    }
}
