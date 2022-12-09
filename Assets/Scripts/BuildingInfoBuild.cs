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
        // 건물 정보 오브젝트에 로드
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
        // 씬 로드 시 자동으로 위치 감지 함수 시작, 3초마다 불러와짐
        StartCoroutine(CheckLocation());
    }

    IEnumerator CheckLocation()
    { 
        /*
         * 위치 감지를 통해 일정 범위 내 건물 정보 감지함.
         * 일정 거리 이내 건물일 경우 해당 건물 정보 열람 가능한 안내판을 띄우고,
         * 빼광 등 특이한 장소의 경우 지나가는 것만으로도 창을 띄움
         * 일정 거리 벗어나면 화면을 가리지 못하도록 안내판을 화면에서 삭제함
         */
        yield return new WaitForSeconds(3f);
        bool questGenerated = false;
        foreach (var kv in buildingData.ToList())
        {
            if (kv.Key.IsQuest() && !questGenerated && kv.Key.IsClose(0.0005d) && kv.Value == null 
                && !arManager.canvas.activeSelf)
            {
                questGenerated = true;
                arManager.SetText(kv.Key);
                buildingData[kv.Key] = new GameObject();
            } 
            else if (kv.Key.IsClose(0.0005d) && kv.Value == null 
                     && kv.Key.building != arManager.building.building)
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
                // ObjectInfo oi = obj.GetComponent<ObjectInfo>();
                obj.GetComponent<BuildingText>().SetText(kv.Key.building);
                // oi.Name = kv.Key.building;
                // oi.Explanation = kv.Key.explanation;
                obj.GetComponent<BuildingText>().SetBuilding(kv.Key.building, kv.Key.explanation);
                PlaceAtLocation.AddPlaceAtComponent(obj, loc, opts);
                buildingData[kv.Key] = obj;
            } 
            else if (!kv.Key.IsClose(0.0005d) && kv.Value != null)
            {
                GameObject tempObj = kv.Value;
                buildingData[kv.Key] = null;
                Destroy(tempObj);
            }
            if (kv.Value != null && kv.Key.building == arManager.building.building)
            {
                GameObject tempObj = kv.Value;
                buildingData[kv.Key] = null;
                Destroy(tempObj);
                if (kv.Key.IsQuest()) buildingData[kv.Key] = new GameObject();
            }
        }
        StartCoroutine(CheckLocation());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
