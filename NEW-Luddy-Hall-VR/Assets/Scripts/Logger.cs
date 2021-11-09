using System.Collections;
using System.IO;
using UnityEngine;

internal enum Proximity
{ Close, Medium, Far }

internal enum LeverDifference
{ Small, Medium, Big }

public enum Trial { Trial1, Trial2 }

public enum Cohort { Control, Experiment }

public class Logger : MonoBehaviour
{
    public string m_UserName;
    public Trial m_Trial = Trial.Trial1;
    public Cohort m_Cohort = Cohort.Control;
    public bool m_IsLoggingOn = true;
    public float logInterval = .1f;
    public Transform m_Headset;
    public Transform m_CameraRig;
    public Transform m_LeftController;
    public Transform m_RightController;
    public FlowManager m_FlowManager;
    public TaskManager m_TaskManager;
    public Teleporter m_Teleporter;
    public ResetPlayerPosition m_ResetPlayerPosition;
    public Transform m_SpawnPosition;
    public float m_TeleportLogSteps = 10f;
    public float[] m_ProximityThresholds = new float[3];
    public float[] m_DifferenceThresholds = new float[3];
    public bool m_IsExperimentRunning = true;

    private float m_CurrentTaskCompletionTime;
    private string m_DateTime;
    private bool isRunning = false;
    private float m_ElapsedTime;
    private string m_CurrentPossibleNav;
    private string m_ExperimentPhase;
    private string m_UserSelectedNavMethod;
    private int m_TaskNumber;
    private GameObject m_ActiveTask;
    private float m_CurrentLeverValue;
    private float m_CurrentTargetLeverNumber;
    private float m_LeverDifference;
    private float m_DistanceToCurrentIndicatorCube;
    private Proximity m_ProximityToCurrentIndicatorCube = Proximity.Far;
    private LeverDifference m_LeverDifferenceEnum = LeverDifference.Big;
    private Vector3 m_PreviousPositionCameraRig;
    private Vector3 m_PreviousPositionHeadset;
    private float m_TotalDistanceTraveledCameraRig;
    private float m_TotalDistanceTraveledHeadset = 0f;
    private GameObject m_TeleportPosition;
    private string m_EventType; //"submission", "button"
    private string m_Side; //"right", "left", ""
    private string m_Status; //"down", "up", ""

    private string m_CurrentTaskCubePositionX = "";
    private string m_CurrentTaskCubePositionY = "";
    private string m_CurrentTaskCubePositionZ = "";

    private void OnEnable()
    {
        FlowManager.EndOfSessionEvent += StopLogging;
        CollisionHandler.OnSubmissionEvent += SubmissionListener;
        CollisionHandler.OnSubmissionEvent += ResetCurrentTaskCompletionTime;

        Teleporter.TeleportEvent += ControllerInputListener;
        //FreeFlyer.FreeFlyEvent += ControllerInputListener;
        //Locomotion.WalkEvent += ControllerInputListener;
        RadialMenu.UserChoiceEvent += ControllerInputListener;
        //Teleporter.TeleportEvent += CaptureTeleportData;
    }

    private void OnDisable()
    {
        FlowManager.EndOfSessionEvent -= StopLogging;
        CollisionHandler.OnSubmissionEvent -= SubmissionListener;
        CollisionHandler.OnSubmissionEvent -= ResetCurrentTaskCompletionTime;

        Teleporter.TeleportEvent -= ControllerInputListener;
        //FreeFlyer.FreeFlyEvent -= ControllerInputListener;
        //Locomotion.WalkEvent -= ControllerInputListener;
        RadialMenu.UserChoiceEvent -= ControllerInputListener; ;
        //Teleporter.TeleportEvent -= CaptureTeleportData;
    }

    private void Start()
    {
        m_DateTime = GiveDateTime();
        SetPreviousPositionCameraRig();
        SetPreviousPositionHeadset();
        m_TeleportPosition = new GameObject();
        m_TeleportPosition.transform.position = Vector3.zero;

        if (m_IsLoggingOn)
        {
            SetUpCSV();
        }
    }

    private void StopLogging()
    {
        m_IsExperimentRunning = false;
    }

    private void Update()
    {
        UpdateData();
        if (m_IsExperimentRunning)
        {
            if (m_IsLoggingOn)
            {
                if (!isRunning) StartCoroutine(LogMessage());
            }
        }
    }

    private void SetPreviousPositionCameraRig()
    {
        m_PreviousPositionCameraRig = m_CameraRig.position;
    }

    private void SetPreviousPositionHeadset()
    {
        m_PreviousPositionHeadset = m_Headset.position;
    }

    private void UpdateData()
    {
        m_CurrentPossibleNav = m_FlowManager.m_CurrentlyPossibleNavMethod.ToString();
        m_ExperimentPhase = m_FlowManager.m_CurrentPhase.ToString();
        m_UserSelectedNavMethod = m_FlowManager.m_ActiveNavigation.ToString();
        m_ElapsedTime += Time.deltaTime;
        m_CurrentTaskCompletionTime += Time.deltaTime;
        //Debug.Log(m_CurrentTaskCompletionTime);
        if (!m_ResetPlayerPosition.m_IsResettingActive)
        {
            m_TotalDistanceTraveledCameraRig += Vector3.Distance(m_PreviousPositionCameraRig, m_CameraRig.position);
            m_TotalDistanceTraveledHeadset += Vector3.Distance(m_PreviousPositionHeadset, m_Headset.position);
            //Debug.Log("m_TotalDistanceTraveledCameraRig: " + m_TotalDistanceTraveledCameraRig);
            //Debug.Log("m_TotalDistanceTraveledHeadset: " + m_TotalDistanceTraveledHeadset);

            SetPreviousPositionCameraRig();
            SetPreviousPositionHeadset();
        }
        else
        {
            //m_PreviousPosition = m_SpawnPosition.position;
            SetPreviousPositionCameraRig();
            SetPreviousPositionHeadset();
        }

        UpdateTaskData();
        //Debug.Log("distance to current cube: " + m_DistanceToCurrentIndicatorCube);
    }

    private void UpdateTaskData()
    {
        m_TaskNumber = m_TaskManager.m_TaskNumber;
        m_ActiveTask = m_TaskManager.m_ActiveTask;

        TaskControl currentTaskControl = m_ActiveTask.GetComponent<TaskControl>();
        m_CurrentLeverValue = currentTaskControl.m_LeverValue;
        m_CurrentTargetLeverNumber = currentTaskControl.m_TargetLeverNumber;
        m_LeverDifference = Mathf.Abs(m_CurrentTargetLeverNumber - m_CurrentLeverValue);
        m_LeverDifferenceEnum = SetLeverDifference(m_LeverDifference);
        //Debug.Log("difference as float: " + m_LeverDifference);
        //Debug.Log("difference as enum: " + m_LeverDifferenceEnum);
        Transform m_CurrentIndicatorCube = currentTaskControl.m_IndicatorCube.transform;
        m_DistanceToCurrentIndicatorCube = Vector3.Distance(m_CurrentIndicatorCube.position, m_Headset.position);
        m_ProximityToCurrentIndicatorCube = SetProximityValue(m_DistanceToCurrentIndicatorCube);
        m_CurrentTaskCubePositionX = m_CurrentIndicatorCube.position.x.ToString();
        m_CurrentTaskCubePositionY = m_CurrentIndicatorCube.position.y.ToString();
        m_CurrentTaskCubePositionZ = m_CurrentIndicatorCube.position.z.ToString();
    }

    public void SubmissionListener()
    {
        m_EventType = "submission";
        LogAndReset();
    }

    private void ResetCurrentTaskCompletionTime()
    {
        m_CurrentTaskCompletionTime = 0f;
    }

    public void ControllerInputListener(string eventType, string side, string status)
    {
        m_EventType = eventType;
        m_Side = side;
        m_Status = status;

        //Add a record
        LogAndReset();
    }

    private void LogAndReset()
    {
        AddRecord(GetPath());
        ResetButtonData();
    }

    private void ResetButtonData()
    {
        string empty = "";
        m_EventType = empty;
        m_Side = empty;
        m_Status = empty;
    }

    private LeverDifference SetLeverDifference(float difference)
    {
        if (difference <= m_DifferenceThresholds[1])
        {
            return LeverDifference.Small;
        }
        else if (difference > m_DifferenceThresholds[1] && difference <= m_DifferenceThresholds[2])
        {
            return LeverDifference.Medium;
        }
        else
        {
            return LeverDifference.Big;
        }
    }

    private Proximity SetProximityValue(float distance)
    {
        if (distance <= m_ProximityThresholds[1])
        {
            return Proximity.Close;
        }
        else if (distance > m_ProximityThresholds[1] && distance <= m_ProximityThresholds[2])
        {
            return Proximity.Medium;
        }
        else
        {
            return Proximity.Far;
        }
    }

    public IEnumerator LogMessage()
    {
        //Debug.Log(m_TotalDistanceTraveled);
        //Debug.Log(m_CurrentTaskCompletionTime);
        AddRecord(GetPath());
        isRunning = true;
        yield return new WaitForSeconds(logInterval);
        isRunning = false;
    }

    public void SetUpCSV()
    {
        using (StreamWriter file = new StreamWriter(GetPath(), true))
        {
            file.WriteLine(
                "userName" + ","
                + "cohort" + ","
                + "trial" + ","
                + "PossibleNavigations" + ","
                + "ExperimentPhase" + ","
                + "UserSelectedNavMethod" + ","
                + "TaskNumber" + ","
                + "ActiveTask" + ","
                //+ "CurrentLeverValue" + ","
                //+ "CurrentTargetLeverNumber" + ","
                //+ "LeverDifference" + ","
                + "DistanceToCurrentIndicatorCube" + ","
                + "ProximityToCurrentIndicatorCube" + ","
                + "CurrentTargetCubeIndicator.x" + ","
                + "CurrentTargetCubeIndicator.y" + ","
                + "CurrentTargetCubeIndicator.z" + ","
                + "elapsedTime" + ","
                + "TotalDistanceTraveledCameraRig" + ","
                + "m_TotalDistanceTraveledHeadset" + ","
                + "headset x" + ","
                + "headset y" + ","
                + "headset z" + ","
                + "Controller (left) x" + ","
                + "Controller (left) y" + ","
                + "Controller (left) z" + ","
                + "Controller (right) x" + ","
                + "Controller (right) y" + ","
                + "Controller (right) z" + ","
                + "headset.euler.x" + ","
                + "headset.euler.y" + ","
                + "headset.euler.z" + ","
                + "headset.InspectorRotation.x" + ","
                + "headset.InspectorRotation.y" + ","
                + "headset.InspectorRotation.z" + ","
                + "leftController.euler.x" + ","
                + "leftController.euler.y" + ","
                + "leftController.euler.z" + ","
                + "leftController.InspectorRotation.x" + ","
                + "leftController.InspectorRotation.y" + ","
                + "leftController.InspectorRotation.z" + ","
                + "rightController.euler.x" + ","
                + "rightController.euler.y" + ","
                + "rightController.euler.z" + ","
                + "rightController.InspectorRotation.x" + ","
                + "rightController.InspectorRotation.y" + ","
                + "rightController.InspectorRotation.z" + ","
                + "cameraRig x" + ","
                + "cameraRig y" + ","
                + "cameraRig z" + ","
                + "cameraRig.euler.x" + ","
                + "cameraRig.euler.y" + ","
                + "cameraRig.euler.z" + ","
                + "cameraRig.InspectorRotation.x" + ","
                + "cameraRig.InspectorRotation.y" + ","
                + "cameraRig.InspectorRotation.z" + ","
                + "m_EventType" + ","
                + "m_Side" + ","
                + "m_Status" + ","
                + "m_CurrentTaskCompletionTime"
                //+ ","
                //+ "TeleportPositionStep.x" + ","
                //+ "TeleportPositionStep.y" + ","
                //+ "TeleportPositionStep.z"
                );
        }
    }

    public void AddRecord(string filepath)
    {
        using (StreamWriter file = new StreamWriter(filepath, true))
        {
            file.WriteLine(
                m_UserName + ","
                + m_Cohort + ","
                + m_Trial + ","
                + m_CurrentPossibleNav + ","
                + m_ExperimentPhase + ","
                + m_UserSelectedNavMethod + ","
                + m_TaskNumber + ","
                + m_ActiveTask.ToString().Substring(0, 5) + ","
                //+ m_CurrentLeverValue + ","
                //+ m_CurrentTargetLeverNumber + ","
                //+ m_LeverDifference + ","
                + m_DistanceToCurrentIndicatorCube + ","
                + m_ProximityToCurrentIndicatorCube + ","
                + m_CurrentTaskCubePositionX + ","
                + m_CurrentTaskCubePositionY + ","
                + m_CurrentTaskCubePositionZ + ","
                + m_ElapsedTime + ","
                + m_TotalDistanceTraveledCameraRig + ","
                + m_TotalDistanceTraveledHeadset + ","
                //position
                + GetPosition(m_Headset.gameObject) + ","
                + GetPosition(m_LeftController.gameObject) + ","
                + GetPosition(m_RightController.gameObject) + ","

            //rotation
            + GetRotation(m_Headset.gameObject) + ","
            + GetRotation(m_LeftController.gameObject) + ","
            + GetRotation(m_RightController.gameObject) + ","
            + GetPosition(m_CameraRig.gameObject) + ","
            + GetRotation(m_CameraRig.gameObject) + ","
            + m_EventType + ","
            + m_Side + ","
            + m_Status + ","
            + m_CurrentTaskCompletionTime
                //+ ","
                //+ m_TeleportPosition.transform.position.x + ","
                //+ m_TeleportPosition.transform.position.y + ","
                //+ m_TeleportPosition.transform.position.z
                );
        }
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

    private string GetPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Data/" + m_Cohort + "/" + m_UserName + " " + m_Cohort + " " + m_Trial + " " + m_DateTime + ".csv";
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
}