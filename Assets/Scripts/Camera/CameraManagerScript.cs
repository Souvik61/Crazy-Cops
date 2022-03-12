using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManagerScript : MonoBehaviour
{
    [SerializeField]
    Transform car;
    [SerializeField]
    CinemachineVirtualCamera mainFollowCam;

    CinemachineVirtualCamera currentlyActiveCam;

    const int activeCamPriority = 12;

    private void OnEnable()
    {
        //Sub to car entering city event
        AllEventsScript.OnCarEnterMaskWObj += OnCarEnterACity;
        AllEventsScript.OnCarExitMaskWObj += OnCarExitACity;
    }

    private void OnDisable()
    {
        //UnSub to car entering city event
        AllEventsScript.OnCarEnterMaskWObj -= OnCarEnterACity;
        AllEventsScript.OnCarExitMaskWObj -= OnCarExitACity;
    }

    private void Start()
    {
        currentlyActiveCam = mainFollowCam;
    }

    void OnCarEnterACity(GameObject caller)
    {
        GameObject cityCam = caller.transform.parent.Find("CityCamera").gameObject;
        currentlyActiveCam = cityCam.GetComponent<CinemachineVirtualCamera>();
        currentlyActiveCam.Priority = activeCamPriority;//transition to city cam

        currentlyActiveCam.Follow = car;//Set vcam follow to car

    }

    void OnCarExitACity(GameObject caller)
    {
      
        currentlyActiveCam.Priority = -1;//transition to mainCam
        currentlyActiveCam.Follow = null;//Remove vcam follow target
        currentlyActiveCam = mainFollowCam;
        currentlyActiveCam.transform.position = new Vector2(0, currentlyActiveCam.transform.position.y);//Set main follow cam position to screen middle

    }


}
