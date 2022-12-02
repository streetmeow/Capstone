using System;
using System.Collections;
using System.Collections.Generic;
using ARLocation;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject pathPrefab;
    public GameObject destinationPrefab;
    public GameObject questPrefab;
    public Camera cam;

    private GameObject[] paths;
    private GameObject[] destinations;
    private GameObject[] quests;
    private GameObject[] targetPool;

    private void Awake()
    {
        paths = new GameObject[100];
        destinations = new GameObject[50];
        quests = new GameObject[50];
        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = Instantiate(pathPrefab);
            paths[i].SetActive(false);
        }

        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i] = Instantiate(destinationPrefab);
            destinations[i].SetActive(false);
        }

        for (int i = 0; i < quests.Length; i++)
        {
            quests[i] = Instantiate(questPrefab);
            quests[i].SetActive(false);
        }
    }

    public GameObject MakeObject(string type)
    {
        switch (type)
        {
            case "path":
                targetPool = paths;
                break;
            case "destination":
                targetPool = destinations;
                break;
            case "quest":
                targetPool = quests;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                targetPool[i].GetComponent<ARLocationManager>().Camera = cam;
                return targetPool[i];
            }
        }

        return null;
    }

    public void DestinationReset()
    {
        for (int i = 0; i < destinations.Length; i++)
        {
            destinations[i].SetActive(false);
        }

        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].SetActive(false);
        }
    }
}
