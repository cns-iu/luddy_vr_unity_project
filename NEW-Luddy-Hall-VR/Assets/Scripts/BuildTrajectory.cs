using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildTrajectory : MonoBehaviour
{
    public float m_PercentageofDataShown = 1;

    //public Condition m_Condition = Condition.standup;
    public string m_Filename;

    public Color[] m_Colors = new Color[4];
    public int[] m_ColumnNumbers = new int[9];
    public Dataset m_DatasetInformation;

    //public Dictionary<string, int> m_ColumnNamesAndNumber = new Dictionary<string, int>();
    public bool m_NeedKidneyRotation = false;

    //public Dictionary<float, Vector3> m_KidneyrotationDict = new Dictionary<float, Vector3>();
    public List<GameObject> m_RotationList = new List<GameObject>();

    public GameObject m_HeadMark;
    public Sprite head;
    public GameObject m_RHandMark;
    public GameObject m_LHandMark;
    public GameObject m_SliverMark;
    public GameObject m_KidneyRotationObject;
    public GameObject m_ParentObject;
    public Vector3 m_ResizeScale;
    public Transform m_RepositionTransform;
    public GameObject m_PR_TeleportLine;

    //public GameObject m_LuddyHallParent;

    public List<GameObject> m_Marks = new List<GameObject>();

    //public bool m_ShowTutorialAndPlateau = true;

    // Start is called before the first frame update
    private void Awake()
    {
        ReadCSV();
    }

    private void ReadCSV()
    {
        int counter = 0;
        StreamReader streamReader = new StreamReader(Application.dataPath + "/Data/Experiment/" + m_Filename + ".csv");
        bool endOfFile = false;
        string dataString = streamReader.ReadLine();
        while (!endOfFile)
        {
            dataString = streamReader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                //GetFirstandLastTimeStamps();
                break;
            }
            string[] dataValues = dataString.Split(',');
            //Debug.Log(dataValues.ToString
            Vector3 spawnPosition1 = new Vector3(
                    ToFloat(dataValues[m_ColumnNumbers[0]]), // standup: 6, tabletop: same, desktop: same
                    ToFloat(dataValues[m_ColumnNumbers[1]]), // standup: 7, tabletop: same, desktop: same
                    ToFloat(dataValues[m_ColumnNumbers[2]]) // standup: 8, tabletop: same, desktop: same
                    );

            m_DatasetInformation.m_RawTaskCompletionTimeStamps.Add(new float[] { Convert.ToInt32(dataValues[m_ColumnNumbers[10]]), float.Parse(dataValues[m_ColumnNumbers[13]]) });
            
            
            
            //Debug.Log(m_DatasetInformation.m_RawTaskCompletionTimeStamps.Count);
            //Debug.Log();

            //Vector3 spawnPosition2 = new Vector3(
            //        ToFloat(dataValues[m_ColumnNumbers[3]]), // standup: 9, tabletop: same
            //        ToFloat(dataValues[m_ColumnNumbers[4]]), // standup: 10, tabletop: same
            //        ToFloat(dataValues[m_ColumnNumbers[5]]) // standup: 11, tabletop: same
            //        );

            //Vector3 spawnPosition3 = new Vector3(
            //        ToFloat(dataValues[m_ColumnNumbers[6]]), // standup: 12, tabletop: same
            //        ToFloat(dataValues[m_ColumnNumbers[7]]), // standup: 13, tabletop: same
            //        ToFloat(dataValues[m_ColumnNumbers[8]]) // standup: 14, tabletop: same
            //        );
            //Vector3 spawnPosition4 = new Vector3(
            //        ToFloat(dataValues[m_ColumnNumbers[9]]), // standup: 17, tabletop: 23
            //        ToFloat(dataValues[m_ColumnNumbers[10]]), // standup: 18, tabletop: 24
            //        ToFloat(dataValues[m_ColumnNumbers[11]]) // standup: 19, tabletop: 25
            //        );
            //Vector3 kidneyRotation = new Vector3();
            //if (m_NeedKidneyRotation)
            //{
            //    kidneyRotation = new Vector3(
            //        ToFloat(dataValues[17]),
            //        ToFloat(dataValues[18]),
            //        ToFloat(dataValues[19])
            //        );
            //    //Debug.Log(kidneyRotation);
            //}
            //Debug.Log(counter % (1 / m_PercentageofDataShown));
            if (counter % (1 / m_PercentageofDataShown) == 0)
            {
                GameObject headMark = SpawnSphere(m_HeadMark, spawnPosition1);
                AddData(headMark, dataValues[m_ColumnNumbers[9]], dataValues[m_ColumnNumbers[10]], dataValues[m_ColumnNumbers[11]], dataValues[m_ColumnNumbers[12]]);  // standup: (5,4,2), tabletop: same
                //headMark.transform.parent = m_LuddyHallParent.transform;
                //GameObject leftHand = SpawnSphere(m_LHandMark, spawnPosition2);
                //AddData(leftHand, dataValues[m_ColumnNumbers[12]], dataValues[m_ColumnNumbers[13]], dataValues[m_ColumnNumbers[14]]);  // standup: (5,4,2), tabletop: same

                //GameObject rightHand = SpawnSphere(m_RHandMark, spawnPosition3);
                //AddData(rightHand, dataValues[m_ColumnNumbers[12]], dataValues[m_ColumnNumbers[13]], dataValues[m_ColumnNumbers[14]]);  // standup: (5,4,2), tabletop: same

                //GameObject sliverCube = SpawnCube(m_SliverMark, spawnPosition4);
                //AddData(sliverCube, dataValues[m_ColumnNumbers[12]], dataValues[m_ColumnNumbers[13]], dataValues[m_ColumnNumbers[14]], dataValues[m_ColumnNumbers[15]]);  // standup: (5,4,2, 38), tabletop: (5,4,2, 44)
            }

            counter++;
        }
        DrawLinesBetweenTeleports();
        ResizeEntireVis();
        RepositionEntireVis();
        //HideTutorialAndPlateauIfDesired();
    }

    private void ComputeCompletionTimes()
    {
        
    }

    private void DrawLinesBetweenTeleports()
    {
        for (int i = 0; i < m_Marks.Count-1; i++)
        {
            Record record = m_Marks[i].GetComponent<Record>();
            if (record.m_ActiveNavigation == ActiveNavigation.Teleport
                && m_Marks[i + 1].GetComponent<Record>().m_ActiveNavigation == ActiveNavigation.Teleport
                && record.m_TaskNumber == m_Marks[i + 1].GetComponent<Record>().m_TaskNumber
                && record.m_TotalDistanceTraveled != m_Marks[i + 1].GetComponent<Record>().m_TotalDistanceTraveled)
            {
                GameObject teleportLine = Instantiate(m_PR_TeleportLine);

                teleportLine.AddComponent<Record>();
                Record lineRecord = teleportLine.GetComponent<Record>();
                lineRecord.m_TotalDistanceTraveled = record.m_TotalDistanceTraveled;
                lineRecord.m_TaskNumber = record.m_TaskNumber;
                lineRecord.m_ActiveNavigation = record.m_ActiveNavigation;
                //teleportLineRecord.m_TotalDistanceTraveled = record.m_TotalDistanceTraveled;

                teleportLine.GetComponent<LineRenderer>().SetPositions(
                    new Vector3[] {
                    m_Marks[i].transform.position,
                    m_Marks[i+1].transform.position
                    }
                    );
                teleportLine.transform.parent = m_ParentObject.transform;
            }
            //else
            //{
            //    Debug.Log("not applicable");
            //}
        }
    }

    private void ResizeEntireVis()
    {
        m_ParentObject.transform.localScale = m_ResizeScale;
    }

    private void RepositionEntireVis()
    {
        m_ParentObject.transform.position = m_RepositionTransform.position;
    }

    //private void HideTutorialAndPlateauIfDesired()
    //{
    //    if (!m_ShowTutorialAndPlateau)
    //    {
    //        foreach (var item in m_Marks)
    //        {
    //            if (item.GetComponent<Record>().m_ActiveNavigation == DataOrigin.Plateau || item.GetComponent<Record>().m_ActiveNavigation == DataOrigin.Tutorial)
    //            {
    //                item.SetActive(false);
    //            }
    //        }
    //    }
    //}

    private void AddData(GameObject mark, string activeNavMethod, string taskNumber, string totalDistanceTraveled, string possibleNavMethod)
    {
        mark.transform.parent = m_ParentObject.transform;
        mark.AddComponent(typeof(Record));
        //mark.GetComponent<Record>().m_TimeStamp = float.Parse(timestamp);
        mark.GetComponent<Record>().m_ActiveNavigation = (ActiveNavigation)Enum.Parse(typeof(ActiveNavigation), activeNavMethod);
        mark.GetComponent<Record>().m_TotalDistanceTraveled = float.Parse(totalDistanceTraveled);
        mark.GetComponent<Record>().m_PossibleNavigations = (PossibleNavigations)Enum.Parse(typeof(PossibleNavigations), possibleNavMethod);
        mark.GetComponent<Record>().m_TaskNumber = Convert.ToInt32(taskNumber) + 6 * (int)mark.GetComponent<Record>().m_PossibleNavigations;

        mark.GetComponent<Renderer>().material.color = GetColor(mark);
        m_Marks.Add(mark);
    }

    private Color GetColor(GameObject mark)
    {
        switch (mark.GetComponent<Record>().m_ActiveNavigation)
        {
            case ActiveNavigation.Walk:
                return m_Colors[0];

            case ActiveNavigation.Teleport:
                return m_Colors[1];

            case ActiveNavigation.Freefly:
                return m_Colors[2];

            case ActiveNavigation.NoChoiceMadeYet:
                return m_Colors[3];

            default:
                return m_Colors[3];
        }
    }

    // overload for tissue block marks
    //private void AddData(GameObject mark, string timestamp, string dataOrigin, string taskNumber, string angle)
    //{
    //    mark.transform.parent = m_ParentObject.transform;
    //    mark.AddComponent(typeof(Record));
    //    mark.GetComponent<Record>().m_TimeStamp = float.Parse(timestamp);
    //    //mark.GetComponent<Record>().m_Phase = (DataOrigin)Enum.Parse(typeof(DataOrigin), dataOrigin);
    //    mark.GetComponent<Record>().m_TaskNumber = Convert.ToInt32(taskNumber);
    //    mark.GetComponent<MeshRenderer>().material.color = Color.Lerp(m_StartColor, m_EndColor, float.Parse(angle) / 180f);
    //    m_Marks.Add(mark);
    //}

    //overload for kidney rotation in Tabletop
    //private void AddData(GameObject kidneyRotationObject, string timestamp, string dataOrigin, string taskNumber, Vector3 rotation)
    //{
    //    kidneyRotationObject.transform.parent = m_ParentObject.transform;
    //    kidneyRotationObject.AddComponent(typeof(Record));
    //    kidneyRotationObject.GetComponent<Record>().m_TimeStamp = float.Parse(timestamp);
    //    kidneyRotationObject.GetComponent<Record>().m_Phase = (DataOrigin)Enum.Parse(typeof(DataOrigin), dataOrigin);
    //    kidneyRotationObject.GetComponent<Record>().m_TaskNumber = Convert.ToInt32(taskNumber);

    //    kidneyRotationObject.AddComponent(typeof(KidneyRotationRecord));
    //    kidneyRotationObject.GetComponent<KidneyRotationRecord>().m_Rotation = rotation;
    //    m_RotationList.Add(kidneyRotationObject);
    //}

    private float ToFloat(string input)
    {
        return float.Parse(input);
    }

    private GameObject SpawnSphere(GameObject mark, Vector3 spawnPosition)
    {
        GameObject create = Instantiate(mark, spawnPosition, Quaternion.identity);
        return (create);
    }

    private GameObject SpawnCube(GameObject mark, Vector3 spawnPosition)
    {
        GameObject create = Instantiate(mark, spawnPosition, Quaternion.identity);
        return (create);
    }

    //private GameObject SpawnKidneyRotationObject(GameObject kidneyRotation, Vector3 spawnRotation)
    //{
    //    GameObject create =
    //    return (create);
    //}

    //private void GetFirstandLastTimeStamps()
    //{
    //    //Debug.Log(marks[100].GetComponent<Record>().m_TimeStamp);
    //    List<float> timestamps = new List<float>();
    //    foreach (var item in m_Marks)
    //    {
    //        //Debug.Log(item.GetComponent<Record>().m_TimeStamp);
    //        float value = item.GetComponent<Record>().m_TimeStamp;
    //        timestamps.Add(value);
    //    }
    //    //Debug.Log(timestamps.Count);
    //    m_DatasetInformation.m_LastTimeStamp = Mathf.Max(timestamps.ToArray());
    //    m_DatasetInformation.m_FirstTimeStamp = Mathf.Min(timestamps.ToArray());
    //    m_DatasetInformation.m_TimeRange = m_DatasetInformation.m_LastTimeStamp - m_DatasetInformation.m_FirstTimeStamp;
    //    //Debug.Log(Mathf.Min(timestamps.ToArray()));
    //    //Debug.Log( m_DatasetInformation.lastTimeStamp);
    //}

    //public GameObject m_Sphere;
    //public GameObject m_ParentObject;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    ReadCSV();
    //    SetInitialParameters();
    //}

    //void SetInitialParameters()
    //{
    //    m_ParentObject.transform.position= new Vector3(-36.4f, 44f, -13.2f);
    //    m_ParentObject.transform.localScale = new Vector3(0.09179848f, 0.09179848f, 0.09179852f);
    //}

    //void ReadCSV()
    //{
    //    StreamReader streamReader = new StreamReader(Application.dataPath + "/Data/trajectories/remapped-height.csv");
    //    bool endOfFile = false;
    //    string dataString = streamReader.ReadLine();
    //    while (!endOfFile)
    //    {
    //        dataString = streamReader.ReadLine();
    //        if (dataString == null)
    //        {
    //            endOfFile = true;
    //            break;
    //        }
    //        string[] dataValues = dataString.Split(',');
    //        //Debug.Log(dataValues.ToString());

    //        if (true)
    //        {
    //            float[] elements = new float[3];
    //            elements[0] = float.Parse(dataValues[15]);
    //            elements[1] = float.Parse(dataValues[16]);
    //            elements[2] = float.Parse(dataValues[17]);

    //            Vector3 spawnPosition = new Vector3(
    //                 elements[0],
    //                  elements[1],
    //                   elements[2]
    //                );

    //            GameObject sphere = SpawnSphere(spawnPosition);
    //            //sphere.AddComponent<GraphicSymbol>();
    //            //sphere.GetComponent<GraphicSymbol>().m_Timestamp
    //            sphere.transform.parent = m_ParentObject.transform;
    //        }
    //    }
    //}

    //GameObject SpawnSphere(Vector3 spawnPosition)
    //{
    //    return (Instantiate(m_Sphere, spawnPosition, Quaternion.identity));
    //}
}