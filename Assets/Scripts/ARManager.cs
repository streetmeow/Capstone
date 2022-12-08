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
    public GameObject destPrefab;
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
    public Buildings building = new Buildings();
    private bool isFirst = true;
    private bool isTest = false;
    private GameObject destObj = null;

    private void Awake()
    {
        // Debug.Log("VAR" + 22222222.3d);
        gpsManager = GameObject.FindGameObjectWithTag("GPS").GetComponent<GPSManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPath();
        destText.text = "탐색 중";
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
        // distanceText.text = gpsManager.chosen_chk.Count.ToString();
        // destText.text = "true";
        StartCoroutine(CheckArrival(true));

    }

    public void TestArrival()
    {
        isTest = true;
    }

    public void HasArrived()
    {
        if (!building.IsClose(0.0007d) && !isTest) return;
        destText.text = "탐색 중";
        nextButtonText.text = "다음 장소로";
        title.text = building.building;
        body.text = building.explanation;
        gpsManager.hasArrived(lastInt);
        for (int i = 0; i < pathObjects.Count; i++)
        {
            Destroy(pathObjects[i]);
        }
        pathObjects.Clear();
        Destroy(destObj);
        destObj = null;
        building = null;
        StopCoroutine(CheckArrival(false));
        // StartCoroutine(CheckArrival());
    }

    IEnumerator CheckArrival(bool arrive)
    {
        yield return new WaitForSeconds(3);
        // destText.text = isFirst.ToString();
        // destText.text = "wow";
        if (arrive)
            {
                isFirst = false;
                building = new Buildings();
                lastName = gpsManager.chosen[gpsManager.getFirstNotGone()];
                lastInt = gpsManager.GetInd(lastName);
                lastExplanation = gpsManager.infoStrings[lastInt];
                building.building = lastName;
                building.explanation = lastExplanation;
                building.latitude = gpsManager.NodesForBuildings[lastInt].getLatitude();
                building.longitude = gpsManager.NodesForBuildings[lastInt].getLongitude();
                destText.text = lastName;
                var loc = new Location()
                {
                    Latitude = building.latitude,
                    Longitude = building.longitude,
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
                destObj = Instantiate(destPrefab);
                destObj.GetComponent<BuildingText>().SetBuilding(lastName, lastExplanation);
                destObj.GetComponent<BuildingText>().SetText("목적지");
                distanceText.text = building.Distance().ToString();
                PlaceAtLocation.AddPlaceAtComponent(destObj, loc, opts);
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
                distanceText.text = building.Distance().ToString();
            }
            if (!building.IsClose(0.0007d)) StartCoroutine(CheckArrival(false));
    }

}
