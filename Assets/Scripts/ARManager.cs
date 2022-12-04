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
    private BuildingInfoBuild buildingInfoBuild;

    private void Awake()
    {
        buildingInfo = new List<Buildings>();
        questInfo = new List<Buildings>();
        hits = new List<ARRaycastHit>();
        cam = Camera.main;
        buildingInfoBuild = GetComponent<BuildingInfoBuild>();
        Debug.Log(buildingInfoBuild.buildingData);
    }

    // Start is called before the first frame update
    void Start()
    {
        buildingInfo.Add(
            new Buildings
                { building = "test1", explanation = "", latitude = 37.55003711556202d, longitude = 127.01403592011496d });
        questInfo.Add(
            new Buildings
                { building = "test1", explanation = "", latitude = 37.54942944204559d, longitude = 127.01334874325624d });
        // GameObject game = objectManager.MakeObject("path", loc);
        // PlaceAtLocation place = game.GetComponent<PlaceAtLocation>();
        // body.text = place.RawGpsDistance.ToString();
        // GameObject pl = Instantiate(p);
    }

    void SetText(Buildings buildings)
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

}
