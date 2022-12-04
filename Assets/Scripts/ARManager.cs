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

    private Vector2 testVec;
    private GPSManager gpsManager;
    private int lastInt;
    private List<GPS> pathList = new List<GPS>();
    private List<GameObject> pathObjects = new List<GameObject>();
    private String lastName;
    private String lastExplanation;
    private Buildings building;

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
        lastInt = gpsManager.getFirstNotGone();
        lastName = gpsManager.BLDGSeq[lastInt];
        lastExplanation = gpsManager.infoStrings[lastInt];
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
            building.building = lastName;
            building.explanation = lastExplanation;
            building.latitude = gpsManager.NodesForBuildings[i].getLatitude();
            building.longitude = gpsManager.NodesForBuildings[i].getLongitude();
            PlaceAtLocation.AddPlaceAtComponent(obj, loc, opts);
            pathObjects.Add(obj);
            Debug.Log(building.building + " " + building.explanation + " " + building.latitude + " " + building.longitude);
        }

        StartCoroutine(CheckArrival());

    }

    IEnumerator CheckArrival()
    {
        yield return new WaitForSeconds(3);
        if (building.IsClose(0.0006d))
        {
            canvas.SetActive(true);
            title.text = lastName;
            body.text = lastExplanation;
            nextButtonText.text = "다음 장소로";
            gpsManager.hasArrived(lastName);
            for (int i = 0; i < pathObjects.Count; i++)
            {
                Destroy(pathObjects[i]);
            }
            pathObjects.Clear();
        }
        else
        {
            StartCoroutine(CheckArrival());
        }
    }

}
