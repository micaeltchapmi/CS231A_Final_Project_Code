using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject indicator;
    public GameObject cubePrefab;

    private Touch touch;
    private GameObject activeobj;

    Camera arcam;
    Vector3 pos2d;

    Vector3 screenCenter;

    // Start is called before the first frame update
    void Start()
    {
        cubePrefab.SetActive(false);
        arcam = GameObject.Find("AR Camera").GetComponent<Camera>();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            
            touch = Input.GetTouch(0); // 0 first finger, 1 second, etc

            if (touch.phase == TouchPhase.Began) {
                //if (indicator.activeInHierarchy)
                //{
                
                Instantiate(cubePrefab, touch.position, indicator.transform.rotation);
                //}

            }
        }
    }
}
