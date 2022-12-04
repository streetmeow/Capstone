using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ARLocation;
using UnityEngine;
public class GPSManager : MonoBehaviour 
{
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GPS");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public int[] nodesNum =                         //BLDGSeq의 노드 번호
    {
        1, 5, 3, 4, 8, 9, 14, 7, 16, 22, 21, 18, 35, 34, 38, 27, 28, 30, 32, 29, 20, 40, 39, 33, 11, 42, 43, 44, 45, 46
    };
    public GPS[] nodes =                            //길찾기에 사용되는 노드
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
        new GPS(37.505736, 126.957398), //11(청룡연못)
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
        new GPS(37.504012, 126.956611), //42(자이언트구장)
        new GPS(37.506700, 126.958172), //43(중앙마루)
        new GPS(37.505935, 126.957498), //44(빼빼로광장)
        new GPS(37.504962, 126.958295), //45(의혈탑)
        new GPS(37.505059, 126.953913), //46(후문)
    };

    public GPS[] buildings =                        //빌딩 방문 순서 찾을 때 사용
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
        new GPS(37.505736, 126.957398), //청룡연못
        new GPS(37.504012, 126.956611), //자이언트구장
        new GPS(37.506700, 126.958172), //중앙마루
        new GPS(37.505935, 126.957498), //빼빼로광장
        new GPS(37.504962, 126.958295), //의혈탑
        new GPS(37.505059, 126.953913)  //후문
    };

    public String[] infoStrings = 
    {
        "흑석역 등으로 통학하는 학생들이 주로 이용한다.",
        "설립자 임영신 박사의 이름을 따서 세워진 최초의 석조건물로, 중앙대에서 가장 오랜 역사를 지니고 있고 1960년대까지 대학 본부로 사용되었다.",
        "총 면적만 약 12000평으로 지하 4층부터 지상 11층 규모로 구성되어 있으며 연구실, 산학협력단 등이 들어서 있다. 지하에 기념품점이 있다.",
        "1956년에 완공된 파이퍼홀은 미국 파이퍼 여사의 유지에 따라 기증한 돈으로 신축한 건물로 그 해 낙성식에 당시 이승만 대통령 부부를 비롯하여 수많은 귀빈이 참석했다. 현재 적십자 간호대학이 사용중이다.",
        "자연과학대학이 사용중인 건물로 지상 7층 규모로 물리학과, 수학과, 화학과, 생명과학과로 구성되어있다.",
        "1974년에 준공된 건물로 지상 6층으로 의과대학이 사용 중이다.",
        "2005년에 건축된 지하 1층, 지상 5층의 규모이다. 의학전문대학원 및 행정실, 암센터 연구실이 있다.",
        "1962년에 준공된 건물로 본래 이름은 ‘4월 학생관’으로 당시 임영신 총장이 4.19혁명 학생 의거를 기념하기 위해 신축했다. 과거 교양수업을 진행했지만 현재 학생회관으로 이용중이다.",
        "지하 1층, 지상 3층의 건물로 총장실, 이사장실을 비롯한 행정기관, 서류발급을 위한 민원 사무실이 있다.",
        "학교 전산을 총괄하며 지하 1층, 지상 5층의 규모로 IT 인프라팀과 IT개발팀이 있다.",
        "인문대학, 사범대학, 사회과학대학이 사용하는 건물로 1972년에 준공되었고 지상 8층 규모이다.",
        "2009년에 리모델링, 증축을 완료한 도서관으로 다양한 학습시설, 편의시설로 구성되어 있으며 1949년 목조 2층 건물에서의 개관을 시작으로 1958년 도서관 신축공사를 거쳤다. 당시 이승만 대통령의 아호 ‘우남’을 따서 우남도서관이라 명명하기도 하였다.",
        "1969년에 준공된 건물로 지하 1층, 지상 7층의 규모이다. 사회기반시스템 공학부, 화학신소재 공학부, 기계공학부, 전자전기 공학부가 사용하고 있으며 각종 연구실 및 실험실이 위치하고 있다.",
        "지하 1층, 지상 6층의 규모로 건축학부, 컴퓨터공학부, 사회기반시스템 공학부, 융합공학부가 사용하고 있다.",
        "지하 2층, 지상 8층의 규모로 지식재산권의 취득 및 관리, 기술이전 및 사업화 촉진, 창업에 대한 지원 등을 담당하고 있다.",
        "1999년에 개관한 첨단문화예술 공간으로 예술대학원, 첨단영상대학원, 491석 규모의 중앙아트홀, 전시실, 멀티미디어 강의실로 구성되어 있다.",
        "지하 3층, 지상 5층의 규모로 1991년에 준공된 건물로 일반대학원, 전문대학원, 특수대학원으로 구성되어 있다.",
        "2007년에 준공된 건물로 지하 3층, 지상 14층으로 법학전문대학원과 법학도서관, 사회과학대학 등이 위치해 있다. 모의법정실, 스터디룸, 인문계열 교학행정실, 사회과학대학 행정실, 특수대학원 행정실이 있다.",
        "연기 실습과 영상 제작에 관련된 실습의 용도로 사용되고 있으며 영상 시사실, 모델 제작실 등이 있다.",
        "교수연구실로 이루어진 공간으로, 지하 3층, 지상 10층으로 구성되어 있다. 체육관, 교수연구실, 체력단련실 등이 있다. ",
        "1995년에 지하 2층, 지상 5층으로 준공된 건물로 2010년 블루미르홀 308관 완공 후 고시반 학생들의 기숙사 및 숙소로 사용되고 있다.",
        "지하 2층, 지상 15층의 건물로 2010년에 완공되었다. 2인실, 4인실, 장애학생실 등 대략 900명을 수용할 수 있으며, 세미나실, 세탁실, 카페, 편의점, 식당 등 각종 편의시설을 갖추고 있다. 또한 단기 방문자나 외국인 교원을 위한 게스트 하우스가 있다.",
        "지하 1층, 지상 14층의 건물로 2015년에 완공되었다. 2인실, 4인실, 장애학생실, 게스트 하우스 등 대략 1400명의 학생을 수용할 수 있으며, 다양한 편의시설을 갖추고 있다.",
        "지하 6층, 지상 12층의 규모로 국내 대학 중 단일건물 면적으로 단연 최대 규모이다. 건물 내 엘리베이터 13대, 에스컬레이터도 건물 내부에 6대, 건물 외부에 4대가 각각 설치됐다. 100주년기념사업단 및 경영, 경제, 공과, 창의 ICT 공과 대학이 사용 중이다.",
        "중앙마루를 지나면 나오는 연못으로 가운데 큰 청룡상과 지구본이 연못 한 가운데 있다.",
        "농구장과 족구장이 갖추어진 제2운동장으로 정규 코트외 4개의 농구대가 각각 비치되어 있다.",
        "흔히 빼빼로 광장이라고 학생들 사이에서 불리며 2011년 1월 완공되어 청룡연못과 중앙광장을 잇는 계단으로 계단 좌우로 쉼터가 마련되어있고 야간에는 조명 불빛이 운치 있다. ",
        "정문과 영신관, 102관(약학대학 및 R&D 센터)사이에 위치한 녹지공간이다.",
        "1960년 4월 19일 민주화와 정치발전을 외치며 산화한 6인열사를 추모하기 위해 건립된 탑이다.",
        "상도역으로 통학하는 학생들이 주로 사용하는 문이다."
    };

    public Road[] roads =                           //두개의 노드로부터 만들어지는 길
    {
        new Road(new GPS[] // 1-43
        {
            new GPS(37.506735, 126.958585),
            new GPS(37.506700, 126.958172)
        },1, 43),
        new Road(new GPS[] // 43-2
        {
            new GPS(37.506700, 126.958172),
            new GPS(37.506670, 126.957936),
            new GPS(37.506634, 126.957785)
        },43, 2),
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
        new Road(new GPS[] // 20-46
        {
            new GPS(37.505837, 126.954364),
            new GPS(37.505676, 126.954275),
            new GPS(37.505482, 126.954160),
            new GPS(37.505325, 126.954058),
            new GPS(37.505176, 126.953976),
            new GPS(37.505059, 126.953913)
        },20, 46),   
        new Road(new GPS[] // 46-27
        {
            new GPS(37.505059, 126.953913),
            new GPS(37.505009, 126.954109),
            new GPS(37.504874, 126.954364)
        },46, 27),
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
        new Road(new GPS[] // 7-44
        {
            new GPS(37.506108, 126.957641),
            new GPS(37.506029, 126.957559),
            new GPS(37.505935, 126.957498)
        },7, 44),
        new Road(new GPS[] // 44-11
        {
            new GPS(37.505935, 126.957498),
            new GPS(37.505736, 126.957398)
        },44, 11),
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
        new Road(new GPS[] // 18-45
        {
            new GPS(37.505080, 126.95813),
            new GPS(37.505006, 126.958227),
            new GPS(37.504962, 126.958295)
        },18, 45),
        new Road(new GPS[] // 45-19
        {
            new GPS(37.504962, 126.958295),
            new GPS(37.504910, 126.958359),
            new GPS(37.504845, 126.958484)
        },45, 19),
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
        new Road(new GPS[] // 33-42
        {
            new GPS(37.504038, 126.956342),
            new GPS(37.504060, 126.956460),
            new GPS(37.504012, 126.956611)
        },33, 42),
        new Road(new GPS[] // 42-34
        {
            new GPS(37.504012, 126.956611),
            new GPS(37.503995, 126.956762),
            new GPS(37.503888, 126.956883),
            new GPS(37.503788, 126.957031),
            new GPS(37.503775, 126.957223),
            new GPS(37.503784, 126.957372)
        },42, 34),
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
    
    public static int MAX_N = 50;                   //최대 노드 개수. 배열 개수 잡는데 사용 
    public static int NODE_N = 47;                  //현재 노드 개수. 반복문에 사용
    public static double MAX_D = 6781;              //현재 모든 경로의 합. 초기화에 사용
    double[,] map = new double[MAX_N,MAX_N];        //경로 계산 위한 2차원 배열
    private double Min_Sum;                         //경로 계산 시 최소 길이 경로 찾기 위한 총 길이
    //bool[] nodeVisit = new bool[MAX_N];
    private double[] nodeDst = new double[MAX_N];   //경로 계산 시 각 노드의 최소 경로 담고 있음
    private int[] Min_Path = new int[MAX_N];        //경로의 노드 순서를 담고 있음
    
    public List<String> chosen = new List<String>();//유저가 가고 싶은 빌딩을 넘겨 받음
    public List<bool> chosen_chk = new List<bool>();//유저가 가고 싶은 빌딩을 갔는지 확인

    public String[] BLDGSeq =                       //학교 내 빌딩, 주요 장소 이름 순서
    {
        "정문", "101관(영신관)", "102관(약학대학 및 R&D 센터)", "103관(파이퍼홀)", "104관(수림과학관)", "105관(제 1의학관)", "106관(제 2의학관)", "107관(학생회관)", "201관(본관)",
        "202관(전산정보관)", "203관(서라벌홀)", "204관(중앙도서관)", "207관(봅스트홀)", "208관(제 2공학관)", "209관(창업보육관)", "301관(중앙문화예술관)", "302관(대학원)",
        "303관(법학관)", "304관(미디어공연영상관)", "305관(교수연구동 및 체육관)", "307관(글로벌하우스)", "308관(블루미르홀 308관)", "309관(블루미르홀 309관)", "310관(100주년 기념관)",
        "청룡연못", "자이언트구장", "중앙마루", "중앙광장", "의혈탑", "후문"
    };
    
    public void init(List<String> chosen) //초기화 함수
    {
        this.chosen = chosen.Distinct().ToList(); // UI로부터 chosen 받아와서 중복 제거
        for (int i = 0; i < chosen.Count; i++) //가야하는 전체 경로의 bool[] false로 초기화
        {
            chosen_chk.Add(false);
        }
        for(int i = 0; i < map.GetLength(0); i++) 
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = 0.0; //map 초기화
            }
        }
        for (int i = 0; i < roads.Length; i++) // roads 클래스 내 distance 계산, map은 경로 탐색위해 값 넣음.
        {
            int[] tmp = roads[i].getPnt();
            map[tmp[0],tmp[1]] = roads[i].getDist();
            map[tmp[1],tmp[0]] = roads[i].getDist();
        }

        foreach (String s in this.chosen)
        {
            Debug.Log(s);
        }
        Input.location.Start(); //유저 GPS 시작
        GetPath(); //테스트 위해 일단 함수를 줄줄이 부르는 형태
    }
    public double Distance(double lat1, double lon1, double lat2, double lon2) // 두 GPS간 거리 계산
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

    private double Deg2rad(double deg) //거리 계산 위한 단위 변환(도->라디안)
    {
        return (double)(deg * Math.PI / (double)180d);
    }

    private double Rad2deg(double rad) //거리 계산 위한 단위 변환(라디안->도)
    {
        return (double)(rad * (double)180d / Math.PI);
    }
    public void GetPath() // 유저로부터 넘겨받은 chosen을 거리에 따라 경로 변경
    {
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
        chosen.Add("정문"); //마지막 위치까지 간 후 다시 정문으로 돌아오기 위함
        //pathInfo(); //테스트 위해 일단 함수를 줄줄이 부르는 형태
    }

    private int GetInd(String str) //string의 index검색
    {
        for(int i = 0; i < BLDGSeq.Length; i++)
        {
            if (str.Contains(BLDGSeq[i]))
            {
                return i;
            }
        }
        return -1;
    }
    public int getLastGone() //마지막 도착 위치 리턴
    {
        int ind = 0;
        for (int i = 0; i < chosen_chk.Count; i++)
        {
            ind = chosen_chk[i] ? i : ind;
        }
        return ind;
    }
    public int getFirstNotGone() // 다음 가야할 위치의 인텍스 리턴
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
    
    public void DFS(int start, int end, int[] path, int no, double sum) // 경로 탐색 위한 깊이 우선 탐색 함수
    {
        int i, j;
        if (sum > Min_Sum)
            return;
        //nodeVisit[start] = true;
        nodeDst[start] = sum; 
        path[no] = start;
        for (i = 1; i < NODE_N; i++) //노드 개수
        {
            //if (i != start && map[start, i] > 0 && !nodeVisit[i])
            if (i != start && map[start, i] > 0 && sum + map[start, i] < nodeDst[i])
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
                        //Min_Path[j + 1] = -1;
                    }
                    return;
                }
                DFS(i, end, path, no + 1, sum + map[start,i]);
            }
        }
    }
    public List<String> GetWay(int start, int end) // 두 지점 사이의 경로를 계산, 리턴
    {
        List<String> gpsWay = new List<String>();
        
        //경로 생성 위한 초기화
        int[] path = new int[MAX_N];
        for (int k = 0; k < MAX_N; k++)
        {
            Min_Path[k] = 0;
            path[k] = 0;
            //nodeVisit[k] = false;
            nodeDst[k] = MAX_D;
        }
        Min_Sum = MAX_D;
        //경로 생성
        DFS(start, end, path, 0, 0);
        for (int i = 0; i < Min_Path.Length; i++) // 경로 정제
        {
            if (Min_Path[i] == end)
            {
                for (int j = i + 1; j < Min_Path.Length; j++)
                {
                    Min_Path[j] = 0;
                }
                break;
            }
        }
        
        for (int i = 1; i < Min_Path.Length; i++) //path에 들어있는 노드 순서를 통해 찍어놓은 GPS값을 받아 옴.
        {
            foreach (Road r in roads)
            {
                int[] tmp = r.getPnt();
                if (tmp[0] == Min_Path[i - 1] && tmp[1] == Min_Path[i]) //노드 순서를 확인 후 순서에 맞게 GPS 값을 넣음
                {
                    foreach(GPS g in r.getRoute())
                    {
                        gpsWay.Add(g.getLatitude() + ", " + g.getLongitude());
                    }

                    break;
                }
                else if (tmp[0] == Min_Path[i] && tmp[1] == Min_Path[i - 1])
                {
                    GPS[] g = r.getRoute();
                    for (int j = g.Length - 1; j >= 0; j--)
                    {
                        gpsWay.Add(g[j].getLatitude() + ", " + g[j].getLongitude());
                    }
                    break;
                }
            }
        }
        
        return gpsWay;
    }

    //GetWay()로 대체
    /*public List<String> pathInfo() //두 노드 간 경로를 받아옴. 처음에는 정문 - chosen[0]으로 시작해서 마지막 도착 건물 - 첫 도착안한 건물 경로 리턴
    {
        List<String> gpsPath = new List<String>();
        int[] path = new int[MAX_N];
        for (int k = 0; k < MAX_N; k++)
        {
            Min_Path[k] = 0;
            path[k] = 0;
            //nodeVisit[k] = false;
            nodeDst[k] = MAX_D;
        }
        Min_Sum = MAX_D;
        /*for (int i = 0; i < Min_Path.Length; i++) //경로의 노드 순서를 담고 있는 Min_Path 0으로 초기화(노드는 1번 부터 시작함)
        {
            Min_Path[i] = 0;
        }
        int[] path = new int[MAX_N];#1#
        if (getFirstNotGone() == 0) //아무것도 가지 않은 상태(정문->첫 목적지)
        {
            DFS(1, nodesNum[GetInd(chosen[getFirstNotGone()])], path, 0, 0);
        }
        else // 중간 경로 받아올 때
        {
            DFS(nodesNum[GetInd(chosen[getLastGone()])], nodesNum[GetInd(chosen[getFirstNotGone()])], path, 0, 0);
        }

        for (int i = 1; i < Min_Path.Length; i++) //path에 들어있는 노드 순서를 통해 찍어놓은 GPS값을 받아 옴.
        {
            foreach (Road r in roads)
            {
                int[] tmp = r.getPnt();
                if (tmp[0] == Min_Path[i - 1] && tmp[1] == Min_Path[i]) //노드 순서를 확인 후 순서에 맞게 GPS 값을 넣음
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
            if (Min_Path[i] == nodesNum[GetInd(chosen[getFirstNotGone()])])
            {
                break;
            }
        }
        /*foreach (int i in Min_Path)
        {
            Debug.Log(i);
        }
        Debug.Log("\n");
        foreach (String i in gpsPath)
        {
            Debug.Log(i);
        }#1#
        return gpsPath;
    }*/

    public void hasArrived(String none) //넘겨받은 string의 bool[] true로 함.
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

    private GPS UpdateGPSData() //유저 현재 데이터 리턴
    {
        return new GPS(Input.location.lastData.latitude, Input.location.lastData.longitude);
    }

    public List<String> OutOfPath() //경로 밖으로 나갈 때 유저 위치에서 부터 다음 위치까지의 경로
    {
        List<String> newPath = new List<String>();
        GPS[] arr_tmp;
        double dist_tmp = 0;
        //GPS userLoc = UpdateGPSData(); 테스트 위해 주석 처리
        GPS userLoc = new GPS(37.506484, 126.958078);
        double dist = Distance(userLoc.getLatitude(), userLoc.getLongitude(), nodes[0].getLatitude(),
            nodes[0].getLongitude());
        int ind = 0, finind1 = 0, finind2 = 0;
        for (int i = 1; i < nodes.Length; i++) //현재 위치에서 가장 가까운 노드와 인덱스 탐색
        {
            dist_tmp = Distance(userLoc.getLatitude(), userLoc.getLongitude(), nodes[i].getLatitude(),
                nodes[i].getLongitude());
            if (dist > dist_tmp)
            {
                dist = dist_tmp;
                ind = i;
            }
        }
        /*for (int i = 0; i < roads.Length; i++)
        {
            if (roads[i].getPnt()[0] == ind || roads[i].getPnt()[1] == ind) //road 끝 점에 위 노드가 있는 road만 검색
            {
                arr_tmp = roads[i].getRoute();
                for(int j = 0; j < arr_tmp.Length; j++) //노드가 포함되어 있는 road에서 제일 가까운 GPS 점 탐색
                {
                    dist_tmp = Distance(userLoc.getLatitude(), userLoc.getLongitude(), arr_tmp[j].getLatitude(),
                        arr_tmp[j].getLongitude());
                    if (dist > dist_tmp)
                    {
                        dist = dist_tmp;
                        finind1 = i;
                        finind2 = j;
                    }
                }
            }
        }*/
        
        //현재 위치에서 가장 가까운 노드까지 몇번 쪼개는 작업이 필요함.
        int latTimes = (int)((userLoc.getLatitude() - nodes[ind].getLatitude()) / 0.00005);
        int lonTimes = (int) ((userLoc.getLongitude() - nodes[ind].getLongitude()) / 0.00005);
        int times = latTimes > lonTimes ? latTimes : lonTimes;
        for (int i = 0; i < times; i++)
        {
            newPath.Add((userLoc.getLatitude() + (userLoc.getLatitude() - nodes[ind].getLatitude()) * (i + 1) / times) + ", " +
                        (userLoc.getLongitude() + (userLoc.getLongitude() - nodes[ind].getLongitude()) * (i + 1) / times));
        }
        //가장 가까운 점이 있는 도로에서 가장 가까운 노드가 있는 방향 탐색
        /*newPath.Add(userLoc.getLatitude() + ", " + userLoc.getLongitude()); //유저 현재 위치 추가
        arr_tmp = roads[finind1].getRoute();
        if (roads[finind1].getPnt()[0] == ind) //역방향
        {
            for (int i = finind2; i >= 0; i--)
            {
                newPath.Add(arr_tmp[i].getLatitude() + ", " + arr_tmp[i].getLongitude());
            }
        }
        else //정방향
        {
            for (int i = finind2; i < arr_tmp.Length; i++)
            {
                newPath.Add(arr_tmp[i].getLatitude() + ", " + arr_tmp[i].getLongitude());
            }
        }*/

        foreach (String s in GetWay(ind, nodesNum[GetInd(chosen[getFirstNotGone()])]))
        {
            newPath.Add(s);
        }
        
        
        /*//가장 가까운 노드에서부터 다음 목적지까지의 경로 탐색 후 newPath에 추가
        int[] path = new int[MAX_N];
        for (int i = 0; i < Min_Path.Length; i++) //경로의 노드 순서를 담고 있는 Min_Path 0으로 초기화(노드는 1번 부터 시작함)
        {
            Min_Path[i] = 0;
        }
        DFS(ind, nodesNum[GetInd(chosen[getFirstNotGone()])], path, 0, 0);
        for (int i = 1; i < Min_Path.Length; i++) //path에 들어있는 노드 순서를 통해 찍어놓은 GPS값을 받아 옴.
        {
            foreach (Road r in roads)
            {
                int[] tmp = r.getPnt();
                if (tmp[0] == Min_Path[i - 1] && tmp[1] == Min_Path[i]) //노드 순서를 확인 후 순서에 맞게 GPS 값을 넣음
                {
                    foreach(GPS g in r.getRoute())
                    {
                        newPath.Add(g.getLatitude() + ", " + g.getLongitude());
                    }
                }
                else if (tmp[0] == Min_Path[i] && tmp[1] == Min_Path[i - 1])
                {
                    GPS[] g = r.getRoute();
                    for (int j = g.Length - 1; j >= 0; j--)
                    {
                        newPath.Add(g[j].getLatitude() + ", " + g[j].getLongitude());
                    }
                }
            }
        }*/
        return newPath;
    }
    public List<String> QuestPath(String questLoc) // 마지막 도착 건물 - 퀘스트 위치
    {
        return GetWay(nodesNum[GetInd(chosen[getLastGone()])], nodesNum[GetInd(questLoc)]);

        /*List<String> qstPath = new List<String>();
        //가장 가까운 노드에서부터 다음 목적지까지의 경로 탐색 후 newPath에 추가
        int[] path = new int[MAX_N];
        for (int k = 0; k < MAX_N; k++)
        {
            Min_Path[k] = 0;
            path[k] = 0;
            //nodeVisit[k] = false;
            nodeDst[k] = MAX_D;
        }
        Min_Sum = MAX_D;
        DFS(nodesNum[GetInd(chosen[getLastGone()])], nodesNum[GetInd(questLoc)], path, 0, 0);
        for (int i = 1; i < Min_Path.Length; i++) //path에 들어있는 노드 순서를 통해 찍어놓은 GPS값을 받아 옴.
        {
            foreach (Road r in roads)
            {
                int[] tmp = r.getPnt();
                if (tmp[0] == Min_Path[i - 1] && tmp[1] == Min_Path[i]) //노드 순서를 확인 후 순서에 맞게 GPS 값을 넣음
                {
                    foreach(GPS g in r.getRoute())
                    {
                        qstPath.Add(g.getLatitude() + ", " + g.getLongitude());
                    }
                }
                else if (tmp[0] == Min_Path[i] && tmp[1] == Min_Path[i - 1])
                {
                    GPS[] g = r.getRoute();
                    for (int j = g.Length - 1; j >= 0; j--)
                    {
                        qstPath.Add(g[j].getLatitude() + ", " + g[j].getLongitude());
                    }
                }
            }
        }
        return qstPath;*/
    }
}
