using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingText : MonoBehaviour
{
    public TextMeshProUGUI text;

    public ObjectInfo objectInfo;
    // Start is called before the first frame update
    public void SetText(String str)
    {
        text.text = str;
    }

    public void SetBuilding(String title, String body)
    {
        objectInfo.Name = title;
        objectInfo.Explanation = body;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
