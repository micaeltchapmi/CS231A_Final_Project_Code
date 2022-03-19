using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnablePrefab;

    Camera arCam;
    GameObject spawnedObject;

    float initialdist;
    Vector3 initialscale;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
          
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;                        
                    }
                    else
                    {
                        spawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                if (Input.touchCount == 2)
                {
                    Touch touch0 = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);
                    // scale the object
                    if (touch0.phase == TouchPhase.Ended || touch0.phase == TouchPhase.Canceled ||
                        touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled)
                    {
                        return;
                    }

                    if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                    {
                        initialdist = Vector2.Distance(touch0.position, touch1.position);
                        initialscale = spawnedObject.transform.localScale;
                    }
                    else // if touch is moved
                    {
                        float currentDistance = Vector2.Distance(touch0.position, touch1.position);

                        //if accidentally touched or pinch movement is very small, ignore
                        if (Mathf.Approximately(initialdist, 0))
                        {
                            return; //ignore pinch movement
                        }
                        float factor = currentDistance / initialdist;
                        spawnedObject.transform.localScale = initialscale * factor;
                    }
                }
                else
                {
                    spawnedObject.transform.position = m_Hits[0].pose.position;
                }
                
            }


            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }
        }
    }

    private void spawnPrefab(Vector3 spawnPosition)
    {
        Vector3 cameraForward = arCam.transform.forward;
        Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.LookRotation(cameraBearing));
    }
}
