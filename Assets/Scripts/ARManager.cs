using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARManager : MonoBehaviour
{
    public ObjectManager objectManager;
    public TextMeshProUGUI title;
    public TextMeshProUGUI body;
    public TextMeshProUGUI nextButtonText;
    public GameObject pathPrefab;
    public GameObject p;
    public GameObject canvas;
    public ARRaycastManager arRaycastManager;
    public TextMeshProUGUI destText;
    public TextMeshProUGUI distanceText;

    private Vector2 testVec;
    private GPSManager gpsManager;
    private int lastInt;
    private List<GPS> pathList = new List<GPS>();
    private List<GameObject> pathObjects = new List<GameObject>();
    private String lastName;
    private String lastExplanation;
    private Buildings building = new Buildings();
    private bool isFirst = true;
    private bool isTest = false;

    private void Awake()
    {
        // Debug.Log("VAR" + 22222222.3d);
        gpsManager = GameObject.FindGameObjectWithTag("GPS").GetComponent<GPSManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPath();
    }

    public void SetText(Buildings buildings)
    {
        canvas.SetActive(true);
        title.text = buildings.building;
        body.text = buildings.explanation;
        if (buildings.IsQuest())
        {
            nextButtonText.text = "닫기";
        }
        else
        {
            nextButtonText.text = "다음 장소로";
        }
    }

    public void SetPath()
    {
        if (nextButtonText.text != "다음 장소로") return;
        isFirst = true;
        StartCoroutine(CheckArrival());

    }

    public void TestArrival()
    {
        isTest = true;
    }

    IEnumerator CheckArrival()
    {
        yield return new WaitForSeconds(3);
        distanceText.text = building.Distance().ToString();
        if ((building.IsClose(0.0006d) && !isFirst) || isTest)
        {
            Debug.Log("isFirst " + isFirst);
            isTest = false;
            canvas.SetActive(true);
            title.text = lastName;
            body.text = lastExplanation;
            nextButtonText.text = "다음 장소로";
            gpsManager.hasArrived(lastInt);
            for (int i = 0; i < pathObjects.Count; i++)
            {
                Destroy(pathObjects[i]);
            }
            pathObjects.Clear();
        }
        else
        {
            if (isFirst)
            {
                Debug.Log("gps first " + gpsManager.getFirstNotGone());
                isFirst = false;
                lastName = gpsManager.chosen[gpsManager.getFirstNotGone()];
                lastInt = gpsManager.GetInd(lastName);
                lastExplanation = gpsManager.infoStrings[lastInt];
                building.building = lastName;
                building.explanation = lastExplanation;
                building.latitude = gpsManager.NodesForBuildings[lastInt].getLatitude();
                building.longitude = gpsManager.NodesForBuildings[lastInt].getLongitude();
                destText.text = lastName;
            }
            if (pathObjects.Count > 0)
            {
                for (int i = 0; i < pathObjects.Count; i++)
                {
                    Destroy(pathObjects[i]);
                }
                pathObjects.Clear();
            }
            pathList.Clear();
            pathList = gpsManager.OutOfPath();
            for (int i = 0; i < pathList.Count; i++)
            {
                // Debug.Log(pathList[i].getLatitude() + ", " + pathList[i].getLongitude());
                var loc = new Location()
                {
                    Latitude = pathList[i].getLatitude(),
                    Longitude = pathList[i].getLongitude(),
                    Altitude = 0,
                    AltitudeMode = AltitudeMode.DeviceRelative
                };
                var opts = new PlaceAtLocation.PlaceAtOptions()
                {
                    HideObjectUntilItIsPlaced = true,
                    MaxNumberOfLocationUpdates = 2,
                    MovementSmoothing = 0.1f,
                    UseMovingAverage = false
                };
                GameObject obj = Instantiate(pathPrefab);
                PlaceAtLocation.AddPlaceAtComponent(obj, loc, opts);
                pathObjects.Add(obj);
            }
            StartCoroutine(CheckArrival());
        }
    }

}
