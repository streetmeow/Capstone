using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ActivateUi : MonoBehaviour
{
    public GameObject ui;
    public TextMeshProUGUI title;
    public TextMeshProUGUI body;
    public TextMeshProUGUI buttonText;
    public ARManager arManager;
    
    [SerializeField]
    private Camera arCamera;

    private Vector2 touchPosition = default;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private ARRaycastManager arRaycastManager;
    // Start is called before the first frame update

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        
    }

    void Update()
    /*
     * raycast 로 충돌 감지 후, Player 태그의 오브젝트일 경우 해당 오브젝트 건물 정보 읽어와
     * UI를 통해 해당 건물의 이름, 설명 띄움
     */
    {
        if (ui.activeSelf)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.CompareTag("Player"))
                    {
                        ObjectInfo oi = hitObject.transform.gameObject.GetComponent<ObjectInfo>();
                        ui.SetActive(true);
                        title.text = oi.Name;
                        body.text = oi.Explanation;
                        buttonText.text = "확인";
                        if (hitObject.transform.gameObject.name == "dest")
                        {
                            arManager.HasArrived();
                        }
                    }
                }
            }
        }
        
    }

    public void ActivateUI()
    {
        ui.SetActive(true);
    }
}
