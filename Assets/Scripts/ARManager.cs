using System.Collections;
using System.Collections.Generic;
using ARLocation;
using TMPro;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    public ObjectManager objectManager;
    public TextMeshProUGUI title;
    public TextMeshProUGUI body;

    // Start is called before the first frame update
    void Start()
    {
        GameObject game = objectManager.MakeObject("path");
        PlaceAtLocation place = game.GetComponent<PlaceAtLocation>();
        place.Location = new Location(37.54999160726763d, 127.01391690020739d);
        body.text = place.RawGpsDistance.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
