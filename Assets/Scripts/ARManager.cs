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

    private GameObject[] paths;
    private Vector2 testVec;
    private List<Buildings> buildingInfo;
    private List<Buildings> questInfo;
    private Camera cam;
    private List<ARRaycastHit> hits;

    private void Awake()
    {
        buildingInfo = new List<Buildings>();
        questInfo = new List<Buildings>();
        hits = new List<ARRaycastHit>();
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        var loc = new Location()
        {
            Latitude = 37.54999160726763D,
            Longitude = 127.01391690020739D,
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
        GameObject pl = Instantiate(p);
        PlaceAtLocation.AddPlaceAtComponent(pl, loc, opts);
        Debug.Log("VAR");
        buildingInfo.Add(
            new Buildings
                { building = "test1", explanation = "", latitude = 37.55003711556202d, longitude = 127.01403592011496d });
        questInfo.Add(
            new Buildings
                { building = "test1", explanation = "", latitude = 37.54942944204559d, longitude = 127.01334874325624d });
        StartCoroutine(CheckLocation());
        // GameObject game = objectManager.MakeObject("path", loc);
        // PlaceAtLocation place = game.GetComponent<PlaceAtLocation>();
        // body.text = place.RawGpsDistance.ToString();
        // GameObject pl = Instantiate(p);
    }

    IEnumerator CheckLocation()
    {
        yield return new WaitForSeconds(3f);
        double gpsLat = Input.location.lastData.latitude;
        double gpsLong = Input.location.lastData.longitude;
        for (int i = 0; i < buildingInfo.Count; i++)
        {
            double dist = CheckDistanceFromGps(buildingInfo[i].latitude, buildingInfo[i].longitude, gpsLat, gpsLong);
            if (dist < 0.001d)
            {
                body.text = dist.ToString();
            }
        }
        for (int i = 0; i < questInfo.Count; i++)
        {
            double dist = CheckDistanceFromGps(questInfo[i].latitude, questInfo[i].longitude, gpsLat, gpsLong);
            if (dist < 0.002d)
            {
                title.text = dist.ToString();
                questInfo.RemoveAt(i);
                break;
            }
        }
        StartCoroutine(CheckLocation());
    }

    double CheckDistanceFromGps(double lat, double longV, double gpsLat, double gpsLong)
    {
        double value = Math.Pow(Math.Pow(lat - gpsLat, 2d) + Math.Pow(longV - gpsLong, 2d), 0.5d);
        return value;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButton(0))
        // {
        //     var ray = cam.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hitInfo;
        //     arRaycastManager.Raycast(Input.mousePosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.FeaturePoint);
        //     
        //     if (Physics.Raycast(ray, out hitInfo))
        //     {
        //         if (hitInfo.collider.CompareTag("Player"))
        //         {
        //             canvas.SetActive(true);
        //         }
        //     }
        // }
    }
}
