using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldCanvasScaler : MonoBehaviour
{

    public float pixelsPerUnit = 25f;
    // Start is called before the first frame update
    public void OnValidate()
    {
        transform.localScale = new Vector3(
                1/pixelsPerUnit,
                1/pixelsPerUnit,
                1f
            );
    }
}
