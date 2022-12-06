using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ARLocation;
using UnityEngine;

public class BuildingInfoBuild : MonoBehaviour
{
    public GPSManager gpsManager;
    public GameObject buildingPrefab;
    public Dictionary<Buildings, GameObject> buildingData = new Dictionary<Buildings, GameObject>();
    public ARManager arManager;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject gpsObject = GameObject.FindGameObjectWithTag("GPS");
        gpsManager = gpsObject.GetComponent<GPSManager>();
        for (int i = 0; i < 30; i++)
        {
            buildingData.Add(new Buildings()
            {
                building = gpsManager.BLDGSeq[i],
                explanation = gpsManager.infoStrings[i],
                latitude = gpsManager.NodesForBuildings[i].getLatitude(),
                longitude = gpsManager.NodesForBuildings[i].getLongitude()
            }, null);
        }
    }

    private void Start()
    {
        StartCoroutine(CheckLocation());
    }

    IEnumerator CheckLocation()
    {
        yield return new WaitForSeconds(3f);
        bool questGenerated = false;
        foreach (var kv in buildingData.ToList())
        {
            if (kv.Key.IsQuest() && !questGenerated && kv.Key.IsClose(0.0006d) && kv.Value == null 
                && !arManager.canvas.activeSelf)
            {
                questGenerated = true;
                arManager.SetText(kv.Key);
                buildingData[kv.Key] = new GameObject();
            } 
            else if (!kv.Key.IsQuest() && kv.Key.IsClose(0.0008d) && kv.Value == null)
            {
                var loc = new Location()
                {
                    Latitude = kv.Key.latitude,
                    Longitude = kv.Key.longitude,
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
                GameObject obj = Instantiate(buildingPrefab);
                ObjectInfo oi = obj.GetComponent<ObjectInfo>();
                oi.Name = kv.Key.building;
                oi.Explanation = kv.Key.explanation;
                PlaceAtLocation.AddPlaceAtComponent(obj, loc, opts);
                buildingData[kv.Key] = obj;
            } 
            else if (!kv.Key.IsQuest() && !kv.Key.IsClose(0.0008d) && kv.Value != null)
            {
                GameObject tempObj = kv.Value;
                buildingData[kv.Key] = null;
                Destroy(tempObj);
            }
        }
        StartCoroutine(CheckLocation());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
