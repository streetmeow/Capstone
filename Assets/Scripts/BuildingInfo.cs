using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings
{
    public String building { get; set; }
    public String explanation { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }

    public bool IsClose(double distance)
    {
        double gpsLat = Input.location.lastData.latitude * 1d;
        double gpsLong = Input.location.lastData.longitude * 1d;
        double value = Math.Pow(Math.Pow(latitude - gpsLat, 2d) + Math.Pow(longitude - gpsLong, 2d), 0.5d);
        if (value > distance) return false;
        return true;
    }

    public bool IsQuest()
    {
        if (building == "청룡연못" || building == "자이언트구장" || building == "중앙광장" || building == "중앙마루" ||
            building == "의혈탑") return true;
        return false;
    }

}
