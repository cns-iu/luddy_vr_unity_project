using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeCompletionTimes : MonoBehaviour
{
    public float m_TopMargin;
    public float m_SideMargin = .05f;
    public float m_BottomMargin;
    public float offset = 1;
    public GameObject[] m_GuideLines = new GameObject[10];

    //public GameObject[] tests = new GameObject[24];
    //public GameObject test1;
    //public GameObject test2;
    public Dataset m_DatasetInformation;

    public Canvas m_ParentCanvas;
    public GameObject[] m_Bars = new GameObject[24];
    public GameObject m_PR_Bar;

    private float m_EffectiveWidth;
    private float m_EffectiveHeight;

    // Start is called before the first frame update
    private void Start()
    {
        //Debug.Log(transform.localScale.x);
        //fill arraay with bars
        InitializeArray();

        //put each bar into their spot within the canvas
        ArrangeSpatially();

        //set the height for each bar in a data-driven manner
        StartCoroutine(SetHeight());

        //set guide lines
        //SetGuideLines();

        //set x axis labels
        SetXAxisLabels();

        //set completion time labels
        //SetCompletionTimeLabels();
    }

    private void SetXAxisLabels()
    {
        foreach (var item in m_Bars)
        {
            Text label = item.transform.GetChild(0).GetComponent<Text>();
            label.text = (System.Array.IndexOf(m_Bars, item) + 1).ToString();
        }
    }

    private void SetGuideLines()
    {
        // find highest value and next smallest number where n%5==0
        float maxTimeValue = Mathf.Max(m_DatasetInformation.m_ProcessedCompletionTimes.ToArray());
        float closestLine = Mathf.Floor(maxTimeValue) - (Mathf.Floor(maxTimeValue) % 5);
        //Debug.Log(closestLine);

        for (int i = 0; i < m_GuideLines.Length; i++)
        {
            RectTransform current = m_GuideLines[i].GetComponent<RectTransform>();
            //Debug.Log("current time: " + m_DatasetInformation.m_ProcessedCompletionTimes[i]);

            current.anchoredPosition = new Vector2(0f, Map(
                closestLine,
                0f,
                Mathf.Max(m_DatasetInformation.m_ProcessedCompletionTimes.ToArray()),
                0f,
                m_ParentCanvas.GetComponent<RectTransform>().rect.height - m_TopMargin
                ));
            //float offset = Map(
            //    closestLine,
            //    0f,
            //    Mathf.Max(m_DatasetInformation.m_ProcessedCompletionTimes.ToArray()),
            //    0f,
            //    m_ParentCanvas.GetComponent<RectTransform>().rect.height - m_TopMargin
            //    );
            //Debug.Log("offset: " + offset);
            //Debug.Log("m_EffectiveHeight: " + m_EffectiveHeight);
            //RectTransform current = m_GuideLines[i].GetComponent<RectTransform>();
            //current.anchoredPosition = new Vector3(
            //    0f,
            //    -m_EffectiveHeight / 2 + offset * i,
            //    0f
            //    );
        }
    }

    private void ArrangeSpatially()
    {
        Rect canvasRectTransform = GetComponent<RectTransform>().rect;
        m_EffectiveWidth = canvasRectTransform.width - m_SideMargin * 2;
        m_EffectiveHeight = canvasRectTransform.height - m_BottomMargin * 2;

        for (int i = 0; i < m_Bars.Length; i++)
        {
            m_Bars[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(
            //origin-half of width + margin
            -m_EffectiveWidth / 2 + ((m_EffectiveWidth) / m_Bars.Length) * (i + 0.5f),
            -m_EffectiveHeight / 2,
            0f
            );
        }

        //float effectiveWidth =
    }

    private void InitializeArray()
    {
        for (int i = 0; i < m_Bars.Length; i++)
        {
            m_Bars[i] = transform.GetChild(1).transform.GetChild(i).gameObject;
        }
    }

    private IEnumerator SetHeight()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < m_Bars.Length; i++)
        {
            RectTransform current = m_Bars[i].GetComponent<RectTransform>();
            //Debug.Log("current time: " + m_DatasetInformation.m_ProcessedCompletionTimes[i]);
            float time = m_DatasetInformation.m_ProcessedCompletionTimes[i];
            current.sizeDelta = new Vector2(current.sizeDelta.x, Map(
                time,
                0f,
                Mathf.Max(m_DatasetInformation.m_ProcessedCompletionTimes.ToArray()),
                0f,
                m_ParentCanvas.GetComponent<RectTransform>().rect.height - m_TopMargin));

            SetCompletionTimeLabels(current.gameObject, current.sizeDelta.y, 5f, time);
        }

        //SetXAxisLabels();
    }

    private void SetCompletionTimeLabels(GameObject gameObject, float barHeight, float offset, float time)
    {
        //get text
        Text label = gameObject.transform.GetChild(1).GetComponent<Text>();
        label.text = time.ToString("f1");
        //label.transform

        //set text y anchoredPos.y to height + some offset
        RectTransform rect = label.gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, barHeight + offset);
    }

    //private float RemapHeightValue(float value)
    //{
    //    //remap from range1(min1, max1) to range(min2, max2)
    //    //completionTimeRange(0, max(completionTimeArray)) to CanvasRange(0, canvasheight)

    //    float oldMax = Mathf.Max(m_DatasetInformation.m_ProcessedCompletionTimes.ToArray());
    //    float newMax =
    //    float result;
    //    result = Map(value, 0f, 17.58f, 0f, 92);
    //    return result;
    //}

    private float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}