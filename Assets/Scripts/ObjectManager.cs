using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    /*
     * 오브젝트풀링을 구현해보려고 하였으나, gps 기반 정보 활용상 문제가 생겨 사장됨
     */
    public GameObject pathPrefab;
    public GameObject destinationPrefab;
    public GameObject questPrefab;
    public Camera cam;

    private GameObject[] paths;
    private GameObject[] destinations;
    private GameObject[] quests;
    private GameObject[] targetPool;
    private bool[] targetActivated;
    private bool[] pathActivated;
    private bool[] destinationActivated;
    private bool[] questActivated;

    private void Awake()
    {
        paths = new GameObject[100];
        destinations = new GameObject[50];
        quests = new GameObject[50];
        pathActivated = new bool[paths.Length];
        destinationActivated = new bool[destinations.Length];
        questActivated = new bool[quests.Length];
        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            // Debug.Log(pathPrefab);
            paths[i] = Instantiate(pathPrefab);
            pathActivated[i] = false;
            paths[i].GetComponent<PlaceAtLocation>().Location = new Location();
        }

        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i] = Instantiate(destinationPrefab);
            destinationActivated[i] = false;
            destinations[i].GetComponent<PlaceAtLocation>().Location = new Location();
        }

        for (int i = 0; i < quests.Length; i++)
        {
            quests[i] = Instantiate(questPrefab);
            questActivated[i] = false;
            quests[i].GetComponent<PlaceAtLocation>().Location = new Location();
        }
    }

    public GameObject MakeObject(string type, Location loc)
    {
        switch (type)
        {
            case "path":
                targetPool = paths;
                targetActivated = pathActivated;
                break;
            case "destination":
                targetPool = destinations;
                targetActivated = destinationActivated;
                break;
            case "quest":
                targetPool = quests;
                targetActivated = questActivated;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetActivated[i])
            {
                targetPool[i].GetComponent<PlaceAtLocation>().Location = loc;
                Debug.Log(targetPool[i]);
                targetActivated[i] = true;
                return targetPool[i];
            }
        }

        return null;
    }

    public void DestinationReset()
    {
        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i].GetComponent<PlaceAtLocation>().Location = new Location();
            destinationActivated[i] = false;
        }

        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].GetComponent<PlaceAtLocation>().Location = new Location();
            pathActivated[i] = false;
        }
    }
}
