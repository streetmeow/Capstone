using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS // Road의 각 점을 담는 클래스
{
    private double latitude, longitude;

    public GPS(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    /*public override bool Equals(object obj)
    {
        var gps = obj as GPS;
        return gps != null && this.latitude == gps.latitude && this.longitude == gps.longitude;
    }*/

    public double getLatitude()
    {
        return this.latitude;
    }
    public double getLongitude()
    {
        return this.longitude;
    }
    public double Distance(GPS g)
    {
        double theta, dist;
        theta = this.longitude - g.getLongitude();
        dist = Math.Sin(Deg2rad(this.latitude)) * Math.Sin(Deg2rad(g.getLatitude())) +
               Math.Cos(Deg2rad(this.latitude)) * Math.Cos(Deg2rad(g.getLatitude())) * Math.Cos(Deg2rad(theta));
        dist = Math.Acos(dist);
        dist = Rad2deg(dist);
        dist = dist * 60 * 1.1515 * 1.609344;
        dist = dist * 1000; //changing to meter
        return Math.Round(dist, 4);
    }
    private double Deg2rad(double deg)
    {
        return (double)(deg * Math.PI / (double)180d);
    }

    private double Rad2deg(double rad)
    {
        return (double)(rad * (double)180d / Math.PI);
    }
}