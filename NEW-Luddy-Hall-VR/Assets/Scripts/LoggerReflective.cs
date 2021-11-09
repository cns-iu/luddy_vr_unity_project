using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoggerReflective : MonoBehaviour
{
    public enum ReflectionPhasePart { Intro, Main };

    public ReflectionPhasePart m_ReflectionPhasePart;
    public string m_UserName;
    public bool isDataLogOn = true;
    private const string m_COHORT = "Experiment";
    private const string m_REFLECTIVE = "Reflective";
    public GameObject m_HeadSet;
    public GameObject m_ControllerLeft;
    public GameObject m_ControllerRight;
    public GameObject m_CameraRig;
    public float logInterval = .1f;
    public Toggle[] m_LegendToggles;

    private float m_TotalDistanceTraveledHeadset;
    private float m_TotalDistanceTraveledCameraRig;
    private Vector3 m_PreviousPositionHeadset;
    private Vector3 m_PreviousPositionCameraRig;
    private bool isExperimentRunning = true;

    //private const string m_ExperimentState = "Reflective";
    private float elapsedTime;

    private string m_DateTime;
    private bool isRunning = false;
    private bool m_CanStartAddingDistance = false;
    //private TimeControl m_TimeControl;

    private void OnEnable()
    {
        //ClickEventHandler.ButtonClickEvent += ButtonListener;
    }

    private void OnDestroy()
    {
        //ClickEventHandler.ButtonClickEvent -= ButtonListener;
    }

    private void OnEndOfSessionStopLogging()
    {
        isExperimentRunning = false;
    }

    //public void ButtonListener(string buttonName, string state)
    //{
    //    m_ButtonName = buttonName;
    //    m_State = state;
    //    AddRecord(GetPath());
    //    ResetButtonData();
    //}

    private void Start()
    {
        //get start time/date for file name
        m_DateTime = GiveDateTime();

        //initialize variables for user input
        InitializeVariables();

        //set up CSV file if logging is on
        if (isDataLogOn)
        {
            SetupCSV();
        }

        //start countdown until we can add up distance traveled so we do not count the initial jump from the origin to the actual start
        StartCoroutine(CountdownUntilDistanceCanBeAdded());
    }

    private void Update()
    {
        UpdateData();

        if (isDataLogOn && isExperimentRunning)
        {
            if (!isRunning) StartCoroutine(LogMessage());
        }

        m_PreviousPositionHeadset = m_HeadSet.transform.position;
        m_PreviousPositionCameraRig = m_CameraRig.transform.position;
    }

    public void UpdateData()
    {
        elapsedTime += Time.deltaTime;

        if (m_CanStartAddingDistance)
        {
            m_TotalDistanceTraveledHeadset += Vector3.Distance(m_HeadSet.transform.position, m_PreviousPositionHeadset);
            m_TotalDistanceTraveledCameraRig += Vector3.Distance(m_CameraRig.transform.position, m_PreviousPositionCameraRig);
        }
    }

    private IEnumerator CountdownUntilDistanceCanBeAdded()
    {
        yield return new WaitForSeconds(0.3f);
        m_CanStartAddingDistance = true;
    }

    public IEnumerator LogMessage()
    {
        AddRecord(GetPath());
        isRunning = true;
        yield return new WaitForSeconds(logInterval);
        isRunning = false;
    }

    //private void ResetButtonData()
    //{
    //    m_ButtonName = "";
    //    m_State = "";
    //}

    public void AddRecord(string filepath)
    {
        using (StreamWriter file = new StreamWriter(filepath, true))
        {
            file.WriteLine(
           FormatMessage()
            );
        }
    }

    private string GetTaskLabelsWithNumbers()
    {
        string result = "";

        for (int i = 0; i < m_LegendToggles.Length; i++)
        {
            float taskNumber = m_LegendToggles[i].GetComponent<ToggleEventHandler>().m_TaskNumber;
            result += ("Task" + taskNumber.ToString() + "IsVisible" + ",");
        }

        return result;
    }

    private string GetLegendToggleStatus()
    {
        string result = "";

        foreach (var entry in m_LegendToggles)
        {
            result += entry.isOn + ",";
        }

        return result;
    }

    private string FormatMessage()
    {
        return
            //general info
            m_UserName + ","
            + m_REFLECTIVE + ","
            + m_ReflectionPhasePart + ","
            //+ m_Condition + ","
            + elapsedTime + ","
            + m_TotalDistanceTraveledHeadset + ","
            + m_TotalDistanceTraveledCameraRig + ","

            //position
            + GetPosition(m_HeadSet) + ","
            + GetPosition(m_ControllerLeft) + ","
            + GetPosition(m_ControllerRight) + ","

            //rotation
            + GetRotation(m_HeadSet) + ","
            + GetRotation(m_ControllerLeft) + ","
            + GetRotation(m_ControllerRight) + ","

            // get status for all filters
            + GetLegendToggleStatus()
            ;
    }

    private void InitializeVariables()
    {
        //m_TimeControl = controllerLeft.GetComponent<TimeControl>();
    }

    public void SetupCSV()
    {
        using (StreamWriter file = new StreamWriter(GetPath(), true))

        {
            file.WriteLine(
            //general info
            "userName" + ","
           + "ExperimentState" + ","
           + "ReflectionPhasePart" + ","
           + "elapsedTime" + ","
           + "totalDistanceTraveledHeadset" + ","
           + "totalDistanceTraveledCameraRig" + ","
           //position
           + "headsetPosX" + ","
           + "headsetPosY" + ","
           + "headsetPosZ" + ","
           + "ControllerLeftPosX" + ","
           + "ControllerLeftPosY" + ","
           + "ControllerLeftPosZ" + ","
           + "ControllerRightPosX" + ","
           + "ControllerRightPosY" + ","
           + "ControllerRightPosZ" + ","
           //rotation
           + "headsetRotEulerX" + ","
           + "headsetRotEulerY" + ","
           + "headsetRotEulerZ" + ","
           + "headsetRotInspectorX" + ","
           + "headsetRotInspectorY" + ","
           + "headsetRotInspectorZ" + ","
           + "controllerLeftRotEulerX" + ","
           + "controllerLeftRotEulerY" + ","
           + "controllerLeftRotEulerZ" + ","
           + "controllerLeftRotInspectorX" + ","
           + "controllerLeftRotInspectorY" + ","
           + "controllerLefttRotInspectorZ" + ","
           + "controllerRightRotEulerX" + ","
           + "controllerRightRotEulerY" + ","
           + "controllerRightRotEulerZ" + ","
           + "controllerRightRotInspectorX" + ","
           + "controllerRightRotInspectorY" + ","
           + "controllerRighttRotInspectorZ" + ","
            + GetTaskLabelsWithNumbers()
           );
        }
    }

    private string GetPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Data/" + m_COHORT + "/" + m_UserName + " " + m_COHORT + " " + m_REFLECTIVE + " " + m_ReflectionPhasePart + " " + m_DateTime + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
#endif
    }

    private string GiveDateTime()
    {
        string dateTimeAtStart;
        dateTimeAtStart = System.DateTime.Now.ToString();
        dateTimeAtStart = dateTimeAtStart.Replace(':', '_');
        dateTimeAtStart = dateTimeAtStart.Replace('/', '.');
        return dateTimeAtStart;
    }

    private string GetPosition(GameObject gameObject)
    {
        string result =
            gameObject.transform.position.x + ","
            + gameObject.transform.position.y + ","
            + gameObject.transform.position.z;
        return result;
    }

    private string GetRotation(GameObject gameObject)
    {
        string result =
            gameObject.transform.rotation.eulerAngles.x + ","
            + gameObject.transform.rotation.eulerAngles.y + ","
            + gameObject.transform.rotation.eulerAngles.z + ","
            + UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x + ","
            + UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).y + ","
            + UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).z;

        return result;
    }
}