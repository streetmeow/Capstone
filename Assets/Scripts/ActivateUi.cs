using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateUi : MonoBehaviour
{
    public GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivateUI()
    {
        ui.SetActive(true);
    }
}
