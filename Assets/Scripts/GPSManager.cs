using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ARLocation;
using UnityEngine;

public class Road
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
    /*public bool chkGPS(GPS gps)
    {
        foreach(GPS g in route)
        {
            if (g.Equals(gps))
            {
                return true;
            }
        }
        return false;
    }*/
}
public class GPS
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
    /*private double Distance(double lat1, double lon1, double lat2, double lon2)
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
    }*/
    private double Deg2rad(double deg)
    {
        return (double)(deg * Math.PI / (double)180d);
    }

    private double Rad2deg(double rad)
    {
        return (double)(rad * (double)180d / Math.PI);
    }
}


public class GPSManager
{
    public int[] nodesNum = //BLDGSeq의 노드 번호
    {
        1, 5, 3, 4, 8, 9, 14, 7, 16, 22, 21, 18, 35, 34, 38, 27, 28, 30, 32, 29, 20, 40, 39, 33
    };
    public GPS[] nodes = //길찾기에 사용되는 노드
    {
        new GPS(37.506735, 126.958585), //1(정문)
        new GPS(37.506634, 126.957785), //2
        new GPS(37.506108, 126.958421), //3(102)
        new GPS(37.505932, 126.958916), //4(103)
        new GPS(37.506118, 126.957963), //5(101)
        new GPS(37.506447, 126.957057), //6
        new GPS(37.506108, 126.957641), //7(107)
        new GPS(37.505843, 126.958441), //8(104)
        new GPS(37.505493, 126.958567), //9(105)
        new GPS(37.505859, 126.957122), //10
        new GPS(37.505736, 126.957398), //11
        new GPS(37.505583, 126.957690), //12
        new GPS(37.505392, 126.958284), //13
        new GPS(37.505214, 126.958594), //14(106)
        new GPS(37.505504, 126.956847), //15
        new GPS(37.505335, 126.957119), //16(201)
        new GPS(37.505176, 126.957375), //17
        new GPS(37.505080, 126.958139), //18(204)
        new GPS(37.504845, 126.958484), //19
        new GPS(37.505837, 126.954364), //20(307)
        new GPS(37.504832, 126.956708), //21(203)
        new GPS(37.504806, 126.957106), //22(202)
        new GPS(37.504475, 126.957334), //23
        new GPS(37.504356, 126.957638), //24
        new GPS(37.504478, 126.956650), //25
        new GPS(37.504429, 126.957261), //26
        new GPS(37.504874, 126.954364), //27(301)
        new GPS(37.504652, 126.954805), //28(302)
        new GPS(37.504561, 126.955009), //29(305)
        new GPS(37.504301, 126.955515), //30(303)
        new GPS(37.504135, 126.955965), //31
        new GPS(37.504308, 126.956164), //32(304)
        new GPS(37.504038, 126.956342), //33(310)
        new GPS(37.503784, 126.957372), //34(208)
        new GPS(37.504120, 126.956436), //35(207)
        new GPS(37.503946, 126.955344), //36
        new GPS(37.502972, 126.957388), //37
        new GPS(37.503267, 126.957767), //38(209)
        new GPS(37.502943, 126.956499), //39(309)
        new GPS(37.502945, 126.956963), //40(308)
        new GPS(37.504744, 126.956610), //41(303 뒤)
    };

    public GPS[] buildings = //빌딩 방문 순서 찾을 때 사용
    {
        new GPS(37.507102, 126.958897), //정문
        new GPS(37.506118, 126.957963), //101
        new GPS(37.506108, 126.958421), //102(E)
        new GPS(37.505932, 126.958916), //103
        new GPS(37.505843, 126.958441), //104(F)
        new GPS(37.505493, 126.958567), //105
        new GPS(37.505214, 126.958594), //106
        new GPS(37.506108, 126.957641), //107(D)
        new GPS(37.505335, 126.957119), //201
        new GPS(37.504806, 126.957106), //202(Y)
        new GPS(37.504832, 126.956708), //203
        new GPS(37.505080, 126.958139), //204(M)
        new GPS(37.504120, 126.956436), //207
        new GPS(37.503784, 126.957372), //208(V)
        new GPS(37.504874, 126.954364), //301(R)
        new GPS(37.504652, 126.954805), //302
        new GPS(37.504301, 126.955515), //303(S)
        new GPS(37.504308, 126.956164), //304
        new GPS(37.504561, 126.955009), //305
        new GPS(37.505837, 126.954364), //307
        new GPS(37.502945, 126.956963), //308
        new GPS(37.502943, 126.956499), //309
        new GPS(37.504038, 126.956342), //310(U)
        new GPS(37.504744, 126.956610), //303(뒤)
        /* new GPS(37.505620, 126.957317),//청룡연못
         new GPS(37.5037, 126.9567),//자이언트구장
         new GPS(37.505896, 126.957493),//빼빼로광장
         new GPS(37.506363, 126.957995),//중앙광장
         new GPS(37.505131, 126.958368),//의혈탑#1#
        new GPS(37.505, 126.9539)//후문*/
    };

    public Road[] roads =
    {
        new Road(new GPS[] // 1-2
        {
            new GPS(37.506735, 126.958585),
            new GPS(37.506700, 126.958172),
            new GPS(37.506670, 126.957936),
            new GPS(37.506634, 126.957785)
        },1, 2),
        new Road(new GPS[] // 1-3
        {
            new GPS(37.506735, 126.958585),
            new GPS(37.506621, 126.958535),
            new GPS(37.506455, 126.958443),
            new GPS(37.506108, 126.958421)
        },1, 3),
        new Road(new GPS[] // 2-6
        {
            new GPS(37.506634, 126.957785),
            new GPS(37.506642, 126.957682),
            new GPS(37.506631, 126.957469),
            new GPS(37.506554, 126.957352),
            new GPS(37.506471, 126.957180),
            new GPS(37.506447, 126.957057)
        },2, 6),
        
        new Road(new GPS[] // 2-7
        {
            new GPS(37.506634, 126.957785),
            new GPS(37.506457, 126.957799),
            new GPS(37.506430, 126.957795),
            new GPS(37.506233, 126.957679),
            new GPS(37.506108, 126.957641)
        },2, 7),					
        new Road(new GPS[] // 6-20
        {
            new GPS(37.506447, 126.957057),
            new GPS(37.506391, 126.956891),
            new GPS(37.506298, 126.956635),
            new GPS(37.506222, 126.956448),
            new GPS(37.506150, 126.956268),
            new GPS(37.506152, 126.956093),
            new GPS(37.506152, 126.955876),
            new GPS(37.506157, 126.955710),
            new GPS(37.506181, 126.955506),
            new GPS(37.506212, 126.955231),
            new GPS(37.506243, 126.955051),
            new GPS(37.506267, 126.954772),
            new GPS(37.506179, 126.954566),
            new GPS(37.506028, 126.954478),
            new GPS(37.505837, 126.954364)
        },6, 20),		
        new Road(new GPS[] // 20-27
        {
            new GPS(37.505837, 126.954364),
            new GPS(37.505676, 126.954275),
            new GPS(37.505482, 126.954160),
            new GPS(37.505325, 126.954058),
            new GPS(37.505176, 126.953976),
            new GPS(37.505059, 126.953913),
            new GPS(37.505009, 126.954109),
            new GPS(37.504874, 126.954364)
        },20, 27),
        new Road(new GPS[] // 6-10
        {
            new GPS(37.506447, 126.957057),
            new GPS(37.506325, 126.957060),
            new GPS(37.506208, 126.957070),
            new GPS(37.506077, 126.957108),
            new GPS(37.505959, 126.957120),
            new GPS(37.505859, 126.957122)
        },6, 10),
        new Road(new GPS[] // 7-5
        {
            new GPS(37.506108, 126.957641),
            new GPS(37.506103, 126.967768),
            new GPS(37.506118, 126.957963)
        }, 7, 5),
        new Road(new GPS[] // 5-3
        {
            new GPS(37.506118, 126.957963),
            new GPS(37.506124, 126.958190),
            new GPS(37.506108, 126.958421)
        },5, 3),
        new Road(new GPS[] // 7-11
        {
            new GPS(37.506108, 126.957641),
            new GPS(37.506029, 126.957559),
            new GPS(37.505935, 126.957498),
            new GPS(37.505736, 126.957398)
        },7, 11),
        new Road(new GPS[] // 3-4
        {
            new GPS(37.506108, 126.958421),
            new GPS(37.505966, 126.958582),
            new GPS(37.505964, 126.958787),
            new GPS(37.505932, 126.958916)
        },3, 4),
        new Road(new GPS[] // 3-8
        {
            new GPS(37.506108, 126.958421),
            new GPS(37.505948, 126.958394),
            new GPS(37.505843, 126.958441)
        },3, 8),
        new Road(new GPS[] // 8-13
        {
            new GPS(37.505843, 126.958441),
            new GPS(37.505742, 126.958453),
            new GPS(37.505642, 126.958414),
            new GPS(37.505533, 126.958430),
            new GPS(37.505392, 126.958284)
        },8, 13),
        new Road(new GPS[] // 10-11
        {
            new GPS(37.505859, 126.957122),
            new GPS(37.505801, 126.957350),
            new GPS(37.505736, 126.957398)
        },10, 11),
        new Road(new GPS[] // 10-15
        {
            new GPS(37.505859, 126.957122),
            new GPS(37.505793, 126.957040),
            new GPS(37.505725, 126.956953),
            new GPS(37.505586, 126.956993),
            new GPS(37.505504, 126.956847)
        },10, 15),
        new Road(new GPS[] // 11-12
        {
            new GPS(37.505736, 126.957398),
            new GPS(37.505661, 126.957529),
            new GPS(37.505583, 126.957690)
        },11, 12),
        new Road(new GPS[] // 12-13
        {
            new GPS(37.505583, 126.957690),
            new GPS(37.505535, 126.957853),
            new GPS(37.505493, 126.957987),
            new GPS(37.505449, 126.958149),
            new GPS(37.505392, 126.958284)
        },12, 13),
        new Road(new GPS[] // 12-17
        {
            new GPS(37.505583, 126.957690),
            new GPS(37.505486, 126.957631),
            new GPS(37.505387, 126.957550),
            new GPS(37.505275, 126.957561),
            new GPS(37.505176, 126.957375)
        },12, 17),
        new Road(new GPS[] // 13-9
        {
            new GPS(37.505392, 126.958284),
            new GPS(37.505450, 126.958395),
            new GPS(37.505493, 126.958567)
        },13, 9),
        new Road(new GPS[] // 13-18
        {
            new GPS(37.505392, 126.958284),
            new GPS(37.505325, 126.958210),
            new GPS(37.505251, 126.958218),
            new GPS(37.505159, 126.958194),
            new GPS(37.505080, 126.95813)
        },13, 18),
        new Road(new GPS[] // 13-14
        {
            new GPS(37.505392, 126.958284),
            new GPS(37.505320, 126.958498),
            new GPS(37.505214, 126.958594)
        },13, 14),
        new Road(new GPS[] // 14-19
        {
            new GPS(37.505214, 126.958594),
            new GPS(37.505076, 126.958559),
            new GPS(37.504954, 126.958509),
            new GPS(37.504845, 126.958484)
        },14, 19),
        new Road(new GPS[] // 15-41
        {
            new GPS(37.505504, 126.956847),
            new GPS(37.505414, 126.956776),
            new GPS(37.505320, 126.956697),
            new GPS(37.505216, 126.956628),
            new GPS(37.505199, 126.956605),
            new GPS(37.505148, 126.956506),
            new GPS(37.505084, 126.956358),
            new GPS(37.505069, 126.956250),
            new GPS(37.505058, 126.956119),
            new GPS(37.504991, 126.956003),
            new GPS(37.504907, 126.955948),
            new GPS(37.504822, 126.955902),
            new GPS(37.504753, 126.956409),
            new GPS(37.504744, 126.956610)
        },15, 41),
        new Road(new GPS[] // 15-16
        {
            new GPS(37.505504, 126.956847),
            new GPS(37.505438, 126.956963),
            new GPS(37.505335, 126.957119)
        },15, 16),
        new Road(new GPS[] // 16-17
        {
            new GPS(37.505335, 126.957119),
            new GPS(37.505301, 126.957191),
            new GPS(37.505235, 126.957301),
            new GPS(37.505176, 126.957375)
        },16, 17),
        new Road(new GPS[] // 17-22
        {
            new GPS(37.505176, 126.957375),
            new GPS(37.505081, 126.957317),
            new GPS(37.504985, 126.957240),
            new GPS(37.504878, 126.957172),
            new GPS(37.504806, 126.957106)
        },17, 22),
        new Road(new GPS[] // 22-21
        {
            new GPS(37.504806, 126.957106),
            new GPS(37.504728, 126.956966),
            new GPS(37.504774, 126.956845),
            new GPS(37.504832, 126.956708)
        },22, 21),
        new Road(new GPS[] // 22-23
        {
            new GPS(37.504806, 126.957106),
            new GPS(37.504676, 126.957139),
            new GPS(37.504621, 126.957298),
            new GPS(37.504482, 126.957257),
            new GPS(37.504475, 126.957334)
        },22, 23),
        new Road(new GPS[] // 18-23
        {
            new GPS(37.505080, 126.95813),
            new GPS(37.505074, 126.957940),
            new GPS(37.505077, 126.957726),
            new GPS(37.504940, 126.957638),
            new GPS(37.504798, 126.957545),
            new GPS(37.504626, 126.957430),
            new GPS(37.504475, 126.957334)
        },18, 23),
        new Road(new GPS[] // 23-26
        {
            new GPS(37.504475, 126.957334),
            new GPS(37.504429, 126.957261)
        },23, 26),
        new Road(new GPS[] // 18-19
        {
            new GPS(37.505080, 126.95813),
            new GPS(37.505006, 126.958227),
            new GPS(37.504962, 126.958295),
            new GPS(37.504910, 126.958359),
            new GPS(37.504845, 126.958484)
        },18, 19),
        new Road(new GPS[] // 19-24
        {
            new GPS(37.504845, 126.958484),
            new GPS(37.505742, 126.958394),
            new GPS(37.504622, 126.958345),
            new GPS(37.504508, 126.958287),
            new GPS(37.504386, 126.958218),
            new GPS(37.504368, 126.950420),
            new GPS(37.504412, 126.957779),
            new GPS(37.504356, 126.957638)
        },19, 24),
        new Road(new GPS[] // 25-41
        {
            new GPS(37.504478, 126.956650),
            new GPS(37.504577, 126.956392),
            new GPS(37.504647, 126.956241),
            new GPS(37.504699, 126.956120),
            new GPS(37.504744, 126.956610)
        },25, 41),
        new Road(new GPS[] // 25-26
        {
            new GPS(37.504478, 126.956650),
            new GPS(37.504499, 126.956680),
            new GPS(37.504473, 126.956900),
            new GPS(37.504451, 126.957109),
            new GPS(37.504429, 126.957261)
        },25, 26),
        new Road(new GPS[] // 25-33
        {
            new GPS(37.504478, 126.956650),
            new GPS(37.504305, 126.956576),
            new GPS(37.504197, 126.956525),
            new GPS(37.504087, 126.956453),
            new GPS(37.504038, 126.956342)
        },25, 33),
        new Road(new GPS[] // 26-24
        {
            new GPS(37.504429, 126.957261),
            new GPS(37.504392, 126.957469),
            new GPS(37.504356, 126.957638)
        },26, 24),
        new Road(new GPS[] // 24-35
        {
            new GPS(37.504356, 126.957638),
            new GPS(37.504375, 126.956614),
            new GPS(37.504253, 126.956543),
            new GPS(37.504120, 126.956436)
        },24, 35),
        new Road(new GPS[] // 35-34
        {
            new GPS(37.504120, 126.956436),
            new GPS(37.503784, 126.957372)
        },35, 34),			
        new Road(new GPS[] // 27-36
        {
            new GPS(37.504874, 126.954364),
            new GPS(37.504724, 126.954182),
            new GPS(37.504574, 126.954116),
            new GPS(37.504421, 126.954074),
            new GPS(37.504311, 126.954077),
            new GPS(37.504220, 126.954228),
            new GPS(37.504156, 126.954399),
            new GPS(37.504086, 126.954565),
            new GPS(37.504036, 126.954707),
            new GPS(37.503979, 126.954857),
            new GPS(37.503969, 126.955056),
            new GPS(37.503946, 126.955344)
        },27, 36),
        new Road(new GPS[] // 27-28
        {
            new GPS(37.504874, 126.954364),
            new GPS(37.504845, 126.954454),
            new GPS(37.504730, 126.954659),
            new GPS(37.504652, 126.954805)
        },27, 28),
        new Road(new GPS[] // 28-29
        {
            new GPS(37.504652, 126.954805),
            new GPS(37.504561, 126.955009)
        },28, 29),
        new Road(new GPS[] // 29-30
        {
            new GPS(37.504561, 126.955009),
            new GPS(37.504444, 126.955287),
            new GPS(37.504373, 126.955463),
            new GPS(37.504301, 126.955515)
        },29, 30),
        new Road(new GPS[] // 30-31
        {
            new GPS(37.504301, 126.955515),
            new GPS(37.504216, 126.955740),
            new GPS(37.504172, 126.955822),
            new GPS(37.504135, 126.955965)
        },30, 31),
        new Road(new GPS[] // 30-36
        {
            new GPS(37.504301, 126.955515),
            new GPS(37.504210, 126.955503),
            new GPS(37.504049, 126.955418),
            new GPS(37.503946, 126.955344)
        },30, 36),
        new Road(new GPS[] // 31-32
        {
            new GPS(37.504135, 126.955965),
            new GPS(37.504213, 126.956058),
            new GPS(37.504294, 126.956149),
            new GPS(37.504308, 126.956164)
        },31, 32),
        new Road(new GPS[] // 31-33
        {
            new GPS(37.504135, 126.955965),
            new GPS(37.504082, 126.956059),
            new GPS(37.504082, 126.956216),
            new GPS(37.504038, 126.956342)
        },31, 33),	
        new Road(new GPS[] // 33-34
        {
            new GPS(37.504038, 126.956342),
            new GPS(37.504060, 126.956460),
            new GPS(37.504012, 126.956611),
            new GPS(37.503995, 126.956762),
            new GPS(37.503888, 126.956883),
            new GPS(37.503788, 126.957031),
            new GPS(37.503775, 126.957223),
            new GPS(37.503784, 126.957372)
        },33, 34),
        new Road(new GPS[] // 34-37
        {
            new GPS(37.503784, 126.957372),
            new GPS(37.503663, 126.957388),
            new GPS(37.503533, 126.957385),
            new GPS(37.503387, 126.957391),
            new GPS(37.503241, 126.957388),
            new GPS(37.503117, 126.957391),
            new GPS(37.502972, 126.957388)
        },34, 37),
        new Road(new GPS[] // 36-39
        {
            new GPS(37.503946, 126.955344),
            new GPS(37.503880, 126.955523),
            new GPS(37.503720, 126.955641),
            new GPS(37.503574, 126.955644),
            new GPS(37.503407, 126.955650),
            new GPS(37.503237, 126.955656),
            new GPS(37.503139, 126.955812),
            new GPS(37.503029, 126.956011),
            new GPS(37.502952, 126.956144),
            new GPS(37.502948, 126.956300),
            new GPS(37.502943, 126.956499)
        },36, 39),				
        new Road(new GPS[] // 39-40
        {
            new GPS(37.502943, 126.956499),
            new GPS(37.502943, 126.956746),
            new GPS(37.502945, 126.956963)
        },39, 40),				
        new Road(new GPS[] // 40-37
        {
            new GPS(37.502945, 126.956963),
            new GPS(37.502933, 126.957159),
            new GPS(37.502948, 126.957325),
            new GPS(37.502972, 126.957388)
        },40, 37),
        new Road(new GPS[] // 37-38
        {
            new GPS(37.502972, 126.957388),
            new GPS(37.502985, 126.957473),
            new GPS(37.502987, 126.957607),
            new GPS(37.503019, 126.957726),
            new GPS(37.503716, 126.957775),
            new GPS(37.503267, 126.957767)
        },37, 38)
    };
    
    public static int MAX_N = 50; 
    double[,] map = new double[MAX_N,MAX_N];
    private double Min_Sum = 600;
    bool[] nodeVisit = new bool[MAX_N];
    private int[] Min_Path = new int[MAX_N];
    
    public List<String> chosen = new List<String>();
    public List<bool> chosen_chk = new List<bool>();

    public String[] BLDGSeq =
    {
        "정문", "101관(영신관)", "102관(약학대학 및 R&D 센터)", "103관(파이퍼홀)", "104관(수림과학관)", "105관(제 1의학관)", "106관(제 2의학관)", "107관(학생회관)", "201관(본관)",
        "202관(전산정보관)", "203관(서라벌홀)", "204관(중앙도서관)", "207관(봅스트홀)", "208관(제 2공학관)", "209관(창업보육관)", "301관(중앙문화예술관)", "302관(대학원)",
        "303관(법학관)", "304관(미디어공연영상관)", "305관(교수연구동 및 체육관)", "307관(글로벌하우스)", "308관(블루미르홀 308관)", "309관(블루미르홀 309관)", "310관(100주년 기념관)",
        "청룡연못", "자이언트구장", "중앙마루", "중앙광장", "의혈탑", "후문"
    };

    /*public double[] BLDGLat =
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
    };*/

    public void init(List<String> chosen)
    {
        this.chosen = chosen.ToList();
        for (int i = 0; i < chosen.Count; i++)
        {
            chosen_chk.Add(false);
        }

        for (int i = 0; i < roads.Length; i++)
        {
            int[] tmp = roads[i].getPnt();
            map[tmp[0],tmp[1]] = roads[i].getDist();
        }
        GetPath();
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
        //double currLon = BLDGLon[0], currLat = BLDGLat[0];
        double currLon = buildings[0].getLatitude(), currLat = buildings[0].getLongitude();
        for (int i = 0; i < chosen.Count - 1; i++)
        {
            double dist = 100000, tmp;
            int nextInd = i;
            for (int j = i; j < chosen.Count; j++)
            {
                int ind = GetInd(chosen[j]);
                tmp = Distance(currLat, currLon, buildings[ind].getLatitude(), buildings[ind].getLongitude());
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
            currLon = buildings[nextInd].getLatitude();
            currLat = buildings[nextInd].getLongitude();
        }
        pathInfo();
    }

    private int GetInd(String str)
    {
        for(int i = 1; i < BLDGSeq.Length - 1; i++)
        {
            if (str.Contains(BLDGSeq[i]))
            {
                return i;
            }
        }
        return -1;
    }
    public int getLastGone()
    {
        int ind = 0;
        for (int i = 0; i < chosen_chk.Count; i++)
        {
            ind = chosen_chk[i] ? i : ind;
        }
        return ind;
    }
    public int getFirstNotGone()
    {
        for (int i = 0; i < chosen_chk.Count; i++)
        {
            if (!chosen_chk[i])
            {
                return i;
            }
        }
        return -1;
    }
    public String nextPos()
    {
        for (int i = 0; i < chosen_chk.Count; i++)
        {
            if (!chosen_chk[i])
            {
                return chosen[i];
            }
        }

        return "ALL_WENT";
    }

    public void DFS(int start, int end, int[] path, int no, double sum)
    {
        int i, j;
        if (sum > Min_Sum)
            return;
        nodeVisit[start] = true;
        path[no] = start;
        for (i = 0; i < 41; i++) //노드 개수
        {
            if (map[start, i] > 0 && !nodeVisit[i])
            {
                if (i == end)
                {
                    if ((sum + map[start, i]) < Min_Sum)
                    {
                        Min_Sum = sum + map[start, i];
                        for (j = 0; j <= no; j++)
                        {
                            Min_Path[j] = path[j];
                        }

                        Min_Path[j] = i;
                        Min_Path[j + 1] = -1;
                    }
                    return;
                }
                DFS(i, end, path, no + 1, sum + map[start,i]);
            }
        }
    }
    public List<String> pathInfo()
    {
        List<String> gpsPath = new List<String>();
        int[] path = new int[MAX_N];
        if (getFirstNotGone() == 0) //아무것도 가지 않은 상태(정문-첫 목적지)
        {
            DFS(1, nodesNum[GetInd(chosen[getFirstNotGone()])], path, 0, 0);
        }
        else
        {
            DFS(nodesNum[GetInd(chosen[getLastGone()])], nodesNum[GetInd(chosen[getFirstNotGone()])], path, 0, 0);
        }

        for (int i = 1; i < Min_Path.Length; i++)
        {
            foreach (Road r in roads)
            {
                int[] tmp = r.getPnt();
                if (tmp[0] == Min_Path[i - 1] && tmp[1] == Min_Path[i])
                {
                    foreach(GPS g in r.getRoute())
                    {
                        gpsPath.Add(g.getLatitude() + ", " + g.getLongitude());
                    }
                }
                else if (tmp[0] == Min_Path[i] && tmp[1] == Min_Path[i - 1])
                {
                    GPS[] g = r.getRoute();
                    for (int j = g.Length - 1; j >= 0; j--)
                    {
                        gpsPath.Add(g[j].getLatitude() + ", " + g[j].getLongitude());
                    }
                }
            }
        }
        foreach (int i in Min_Path)
        {
            Debug.Log(i);
        }
        Debug.Log("\n");
        foreach (String i in gpsPath)
        {
            Debug.Log(i);
        }
        //길 정보 받아오기 chosen[getLastGone()] - chosen[getFirstNotGone()] 경로
        return gpsPath;
    }

    public void hasArrived(String none) //??
    {
        if (none.Equals("None"))
        {
            chosen_chk[getFirstNotGone()] = true;
        }
        else
        {
            for (int i = 0; i < chosen.Count; i++)
            {
                if (chosen[i].Equals(none))
                {
                    chosen_chk[i] = true;
                    break;
                }
            }
        }
    }
}
