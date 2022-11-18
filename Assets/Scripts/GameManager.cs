using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int[] buttonchange = Enumerable.Repeat<int>(0, 65).ToArray<int>();
    public List<String> chosen = new List<String>();

    //public int checkMap = 0;
    //빌딩 순서
    public String[] BLDGSeq =
    {
        "정문", "101관(영신관)", "102관(약학대학 및 R&D 센터)", "103관(파이퍼홀)", "104관(수림과학관)", "105관(제 1의학관)", "106관(제 2의학관)", "107관(학생회관)", "201관(본관)",
        "202관(전산정보관)", "203관(서라벌홀)", "204관(중앙도서관)", "207관(봅스트홀)", "208관(제 2공학관)", "209관(창업보육관)", "301관(중앙문화예술관)", "302관(대학원)",
        "303관(법학관)", "304관(미디어공연영상관)", "305관(교수연구동 및 체육관)", "307관(글로벌하우스)", "308관(블루미르홀 308관)", "309관(블루미르홀 309관)", "310관(100주년 기념관)",
        "청룡연못", "자이언트구장", "중앙마루", "중앙광장", "의혈탑", "후문"
    };

    public double[] BLDGLat =
    {
        37.507102, 37.505965, 37.506507, 37.505944, 37.505769, 37.5056, 37.505064, 37.506336, 37.505236,
        37.504919, 37.504821, 37.504757, 37.5038, 37.5036, 37.503226, 37.5054, 37.5049,
        37.504648, 37.504502, 37.504382, 37.5058, 37.5028, 37.502, 37.5037,
        37.505620, 37.5037, 37.505896, 37.506363, 37.505131, 37.505
    };
    
    public double[] BLDGLon =
    {
        126.958897, 126.958035, 126.958952, 126.959014, 126.958242, 126.9589, 126.958749, 126.957269, 126.956908,
        126.956923, 126.956451, 126.958134, 126.9577, 126.9571, 126.958030, 126.9544, 126.9551,
        126.955889, 126.956254, 126.954535, 126.9547, 126.9566, 126.9569, 126.9559,
        126.957317, 126.9567, 126.957493, 126.957995, 126.958368, 126.9539
    };
    void Start()
    {
        //checkMap = 0;
    }
    public double Distance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta, dist;
        theta = lon1 - lon2;
        dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) +
               Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
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
    public void GetPath()
    {
        double currLon = BLDGLon[0], currLat = BLDGLat[0];
        for (int i = 0; i < chosen.Count - 1; i++)
        {
            double dist = 100000, tmp;
            int nextInd = i;
            for (int j = i; j < chosen.Count; j++)
            {
                int ind = GetInd(chosen[j]);
                if (ind == -1)
                {
                    Debug.Log(chosen[j]);
                }
                tmp = Distance(currLat, currLon, BLDGLat[ind], BLDGLon[ind]);
                Debug.Log(tmp.ToString());
                if (dist > tmp)
                {
                    dist = tmp;
                    nextInd = j;
                }
            }
            if (i != nextInd)
            {
                String str = chosen[nextInd];
                chosen[nextInd] = chosen[i];
                chosen[i] = str;
            }
            currLon = BLDGLon[nextInd];
            currLat = BLDGLat[nextInd];
        }
    }

    private int GetInd(String str)
    {
        for(int i = 1; i < BLDGSeq.Length - 1; i++)
        {
            Debug.Log(BLDGSeq[i] + " " + str);
            if (str.Contains(BLDGSeq[i]))
            {
                return i;
            }
        }
        return -1;
    }
    public void ClickButton()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        int inx = Int32.Parse(clickObject.name.Substring(6)) - 1;
        //print(inx);
        ColorBlock cb = clickObject.GetComponent<Button>().colors;
        if (buttonchange[inx] == 0)
        {
            buttonchange[inx] = 1;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.pressedColor = Color.red;
            clickObject.GetComponent<Button>().colors = cb;
            chosen.Add(clickObject.GetComponentInChildren<Text>().text.Trim().Split('(')[1]);
        }
        else
        {
            buttonchange[inx] = 0;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white;
            clickObject.GetComponent<Button>().colors = cb;
            chosen.Remove(clickObject.GetComponentInChildren<Text>().text.Trim().Split('(')[1]);
        }
    }

    public void mapChecking()
    {
        //checkMap = 1;
        foreach (String s in BLDGSeq)
        {
            Debug.Log(s);
        }
        GetPath();
        foreach (String s in chosen)
        {
            Debug.Log(s);
        }
    }
}

    

