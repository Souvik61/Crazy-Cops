using System.Collections;
using UnityEngine;

public class NewPoliceAIScript : MonoBehaviour
{
    public Transform targetTransform;
    public Transform selfTransform;
    public float targetDistanceTolerance = 7.0f;
    public float brakeDistance = 7.0f;
    public float brakeVelocity = 7.0f;
    public float topVelocity;//Top velocity at which it will apply brakes
    public float arrestDistance;

    public CarBaseScript hostCarScript;
    Rigidbody2D targetRBody;
    NewHumanInputController targetCarController;
    float distanceToTarget;
    Vector2 dirToTarget;
    bool targetAvailable = false;

    bool isNear = false;
    bool isArrestIniting = false;
    bool isArresting = false;
    bool isArrestSuccess = false;

    public float arrestMeter = -1;


    // Start is called before the first frame update
    private void Start()
    {
        if (targetTransform != null)
        {
            targetRBody = targetTransform.GetComponent<Rigidbody2D>();
            targetCarController = targetTransform.GetComponent<NewHumanInputController>();
            targetAvailable = true;
        }
    }

    void Awake()
    {
        hostCarScript = GetComponentInChildren<CarBaseScript>();//Find car controller
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (targetAvailable)
        {
            distanceToTarget = Vector2.Distance(selfTransform.position, targetTransform.position);

            //If target too far
            if (distanceToTarget > targetDistanceTolerance)
            {
                Vector2 dirToTarget = (targetTransform.position - selfTransform.position).normalized;
                float dotProd = Vector2.Dot(selfTransform.up, dirToTarget);

                //move forward?
                if (dotProd > 0)
                {
                    hostCarScript.PAccelerate();
                }
                //move backward?
                else
                {
                    if (distanceToTarget > 10f)
                    {
                        //Too far to reverse
                        hostCarScript.PAccelerate();
                    }
                    else
                    {
                        hostCarScript.PReverse();
                    }
                }


                //Turn logic
                float angleToDir = Vector2.SignedAngle(selfTransform.up, dirToTarget);

                if (angleToDir < 0 && Mathf.Abs(angleToDir) > 10) { hostCarScript.PTurnRight(); }
                else if (angleToDir > 0 && Mathf.Abs(angleToDir) > 10) { hostCarScript.PTurnLeft(); }
                else { }
            }
            else//Reached target
            {
                //hostCarScript.Accelerate(false);
                //hostCarScript.Reverse(false);
                if (hostCarScript.SpeedKilometersPerHour > 2)
                    hostCarScript.PBrake();
            }

            /*
            //Brake
            Vector2 tarVelocity = new Vector2();
            if (targetRBody != null)//Check if target rigidbody available
            { tarVelocity = targetRBody.velocity; }
            Vector2 selfVelocity = hostCarScript.rBody.velocity;
            float velo = hostCarScript.rBody.velocity.magnitude;

            Vector2 tmp = selfTransform.position - targetTransform.position;
            float relVelocity = -(Vector2.Dot(selfVelocity - tarVelocity, tmp) / tmp.magnitude);

            //Debug.Log(Mathf.Abs(relVelocity));

            if (distanceToTarget < brakeDistance && velo > brakeVelocity && Mathf.Abs(relVelocity) > 7.0f)
            {
                hostCarScript.PBrake();
            }
            else
            { //hostCarScript.Brake(false); }
            }
            
        }
    }
*/

    void Update()
    {
        if (targetAvailable)
        {
            if (AccelerateLogic())
            {
                TurnLogic();
            }
            BrakeLogic();

            ArrestLogic();
        }
    }

    //-------------------------
    //Update logic functions
    //-------------------------

    bool AccelerateLogic()
    {
        bool isMoving = false;
        distanceToTarget = Vector2.Distance(selfTransform.position, targetTransform.position);

        //If target too far
        if (distanceToTarget > targetDistanceTolerance)
        {
            dirToTarget = (targetTransform.position - selfTransform.position).normalized;
            float dotProd = Vector2.Dot(selfTransform.up, dirToTarget);

            //move forward?
            if (dotProd > 0)
            {
                hostCarScript.PAccelerate();
            }
            //move backward?
            else
            {
                if (distanceToTarget > 2.5f)
                {
                    //Too far to reverse
                    hostCarScript.PAccelerate();
                }
                else
                {
                    hostCarScript.PReverse();
                }
            }

            isMoving = true;
        }
        else//Reached target
        {
            //hostCarScript.Accelerate(false);
            //hostCarScript.Reverse(false);
            if (hostCarScript.SpeedKilometersPerHour > 2)
                hostCarScript.PBrake();
        }

        return isMoving;
    }

    void TurnLogic()
    {
        //Turn logic
        float angleToDir = Vector2.SignedAngle(selfTransform.up, dirToTarget);

        if (angleToDir < 0 && Mathf.Abs(angleToDir) > 10) { hostCarScript.PTurnRight(); }
        else if (angleToDir > 0 && Mathf.Abs(angleToDir) > 10) { hostCarScript.PTurnLeft(); }
        else { }
    }

    void BrakeLogic()
    {
        //Brake
        Vector2 tarVelocity = new Vector2(0, 0);
        if (targetRBody != null)//Check if target rigidbody available
        { tarVelocity = targetRBody.velocity; }
        Vector2 selfVelocity = hostCarScript.rBody.velocity;
        float velo = hostCarScript.rBody.velocity.magnitude;

        Vector2 tmp = selfTransform.position - targetTransform.position;
        float relVelocity = -(Vector2.Dot(selfVelocity - tarVelocity, tmp) / tmp.magnitude);

        if (distanceToTarget < brakeDistance && Mathf.Abs(relVelocity) > brakeVelocity)
        {
            hostCarScript.PBrake();
        }
        if (velo > topVelocity)
        {
            hostCarScript.PBrake();
        }
        
    }

    void ArrestLogic()
    {
        distanceToTarget = Vector2.Distance(selfTransform.position, targetTransform.position);

        //If target near
        isNear = (distanceToTarget < arrestDistance);

        if (isNear && !isArrestIniting && !isArresting && !isArrestSuccess)
        {
            if (!targetCarController.isArrested && !targetCarController.isBeingArrested)
            {
                StartCoroutine(nameof(ArrestInitiateRoutine));
            }
        }
    }

    //Events
    void OnArrestInit()
    {
        targetTransform.GetComponent<NewHumanInputController>().isBeingArrested = true;
        //Trigger event when arresting
        AllEventsScript.OnArrestInitiated?.Invoke(this);
    }

    void OnArrestSuccess()
    {
        isArrestSuccess = true;

        targetTransform.GetComponent<NewHumanInputController>().isArrested = true;//Set isArrested status of the car

        AllEventsScript.OnArrestComplete?.Invoke();//Trigger arrest complete event

        //Debug.Log("Arrest complete");
    }

    void OnArrestFailed()
    {
        arrestMeter = 0;
        targetTransform.GetComponent<NewHumanInputController>().isBeingArrested = false;
        if (AllEventsScript.OnArrestFailed != null)
            AllEventsScript.OnArrestFailed(this);
    }

    //--------------------------
    //Routines
    //--------------------------

    IEnumerator ArrestInitiateRoutine()
    {
        //Debug.Log("Arrest Init");
        isArrestIniting = true;
        float timer = 0;
        for (; ; )
        {
            timer += Time.deltaTime;
            yield return null;
            if (timer > 2.0f || !isNear)//If timer over or car out of reach
            { break; }
        }
        isArrestIniting = false;
        if (isNear && !targetCarController.isBeingArrested && !targetCarController.isArrested)
        { StartCoroutine(nameof(ArrestRoutine)); }
       // Debug.Log("Arrest Init complete");
    }

    IEnumerator ArrestRoutine()
    {
        //Debug.Log("Arrest Routine");
        isArresting = true;
        float timer = 0;

        OnArrestInit();

        for (; ; )
        {
           // Debug.Log(timer);
            timer += Time.deltaTime;

            arrestMeter = timer / 3.0f;//Set arrest amount normalized

            yield return null;
            if (timer > 3.0f || !isNear)
            { break; }
        }
        isArresting = false;
        //arrestMeter = -1;
        if (isNear)
        {
            OnArrestSuccess();
        }
        else
        {
            OnArrestFailed();
        }
    }


}
