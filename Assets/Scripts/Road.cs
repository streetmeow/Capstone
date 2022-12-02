using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road // 두 노드와 그 노드 사이의 GPS점을 가지고 있는 클래스. 이 클래스 이용하여 경로 내 GPS 점 리턴
{
    private GPS[] route; //route: path gps data
    private int[] pnt = new int[2]; //pnt: two end points(nodes)
    private double distance = 0; //distance of route

    public Road(GPS[] rt, int pnt1, int pnt2)
    {
        route = new GPS[rt.Length];
        for (int i = 0; i < rt.Length; i++)
        {
            route[i] = rt[i];
            if (i != 0)
            {
                distance += rt[i].Distance(rt[i - 1]);
            }
        }
        pnt[0] = pnt1;
        pnt[1] = pnt2;
    }

    public int[] getPnt()
    {
        return pnt;
    }

    public double getDist()
    {
        return distance;
    }

    public GPS[] getRoute()
    {
        return route;
    }
}