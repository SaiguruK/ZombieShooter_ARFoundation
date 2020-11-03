using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GroundPlaneManager : MonoBehaviour
{
    public GameObject groundPlane;
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;

    public bool isGroundPlaneReady;

    // Start is called before the first frame update
    void Start()
    {
        if (groundPlane != null) groundPlane.SetActive(false);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;//To make sure screen does not sleep
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTouches();
    }

    private void ProcessTouches()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.touchCount != 0)
        {
            Touch touchZero = Input.GetTouch(0);
            if(touchZero.phase == TouchPhase.Began)
            {
                var hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touchZero.position, hits, TrackableType.Planes))//trackable plane is plane because if we want raycast agaibst planes
                {
                    if(groundPlane != null && isGroundPlaneReady == false)
                    {
                        //gets top most plane
                        Pose pose = hits[0].pose;// pose = new structure to store position and rotation
                        groundPlane.transform.position = pose.position;
                        groundPlane.SetActive(true);
                        isGroundPlaneReady = true;
                        planeManager.enabled = false;

                        ARPlane[] planes;
                        planes = GameObject.FindObjectsOfType<ARPlane>();
                        foreach(var item in planes)
                        {
                            item.gameObject.SetActive(false);
                        }


                    }
                }
            }
        }
    }
}
