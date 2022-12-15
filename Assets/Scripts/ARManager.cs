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
    // public TextMeshProUGUI distanceText;
    public GameObject finishObj;
    public TextMeshProUGUI finishText;

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
        // gps 매니저 오브젝트에 불러오기
        gpsManager = GameObject.FindGameObjectWithTag("GPS").GetComponent<GPSManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 시작 3초 후 다음 목적지로의 경로 출력
        SetPath();
        destText.text = "탐색 중";
    }

    public void SetText(Buildings buildings)
    {
        // 빌딩 정보 띄울 때 사용
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
        // 길 세팅
        if (nextButtonText.text != "다음 장소로") return;
        isFirst = true;
        StartCoroutine(CheckArrival(true));

    }

    public void TestArrival()
    {
        // 디버깅용, 의미없음
        isTest = true;
    }

    public void HasArrived()
    {
        /*
         * 일정 범위 이하 도착 및 목적지 패널 선택하면 불러와지며,
         * 다음 목적지로의 경로가 셋팅됨
         */
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
        /*
         * 목적지까지의 경로 및 신규 목적지 등의 설정을 담당.
         * 3초마다 불러와져서 실시간 위치 기반으로 경로를 계속 제작함.
         * 부정확한 gps 의 정확성 개선을 위함.
         */
        yield return new WaitForSeconds(3);
        if (arrive)
            {
                isFirst = false;
                building = new Buildings();
                if (gpsManager.getFirstNotGone() == -1)
                {
                    ActivateFinish();
                    yield break;
                }
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
                // distanceText.text = building.Distance().ToString();
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
                // distanceText.text = building.Distance().ToString();
            }
            if (!building.IsClose(0.0007d)) StartCoroutine(CheckArrival(false));
    }

    void ActivateFinish()
    {
        nextButtonText.text = "안내 종료";
        destText.text = "안내 종료";
        // distanceText.text = "";
        finishObj.SetActive(true);
        finishText.text = DateTime.Now.ToString("yyyy년 MM월 dd일");
    }

}
