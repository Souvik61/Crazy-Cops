using System.Collections.Generic;
using UnityEngine;

//Car class
public class NewCarScript : MonoBehaviour
{
    //Inspector

    public float clampMovePos = 1;
    public float turnSpeedOut;
    public float turnSpeedIn;
    public float desiredPos;

    public Rigidbody2D rBody;
    public GameObject smokePrefab;
    public GameObject brakeLights;//Brake lights
    public GameObject headLights;//Brake lights

    //Four Wheels
    public SliderJoint2D axleSlider;
    public NewTireScript rLTire;
    public NewTireScript rRTire;
    //List of all tires
    public List<NewTireScript> tireScripts;

    //Internal vars
    public enum AccState { NONE, UP, DOWN };
    public enum TurnState { NONE, RIGHT, LEFT };
    AccState currentAccState = AccState.NONE;
    TurnState currentTurnState = TurnState.NONE;
    int driveDir = 0;
    uint lives;
    bool isCarOk;
    bool isCarPaused;

    //Ext. control
    bool accelActive;
    bool brakeActive;
    bool reverseActive;
    int  turnDir;

    private void Awake()
    {
        lives = 3;
        isCarOk = true;
        isCarPaused = false;
    }

    // Update is called once per frame
    /* void Update()
     {
         if (isCarOk && !isCarPaused)
         {
             //Accelerator
             if (Input.GetKey(KeyCode.W)) { currentAccState = AccState.UP; driveDir = 1; }
             else if (Input.GetKey(KeyCode.S)) { currentAccState = AccState.DOWN; driveDir = -1; }
             else { currentAccState = AccState.NONE; driveDir = 0; }

             //Steering
             if (Input.GetKey(KeyCode.A)) { currentTurnState = TurnState.LEFT; }
             else if (Input.GetKey(KeyCode.D)) { currentTurnState = TurnState.RIGHT; }
             else { currentTurnState = TurnState.NONE; }
         }
         else
         {
             currentAccState = AccState.NONE;
             driveDir = 0;
             currentTurnState = TurnState.NONE;
         }

     }*/

    void Update()
    {
        //Handle input
        if (isCarOk && !isCarPaused)
        {
            //Accelerator
            if (accelActive) { currentAccState = AccState.UP; driveDir = 1; }

            //Reverse
            if (reverseActive) { currentAccState = AccState.DOWN; driveDir = -1; }

            if (accelActive == false && reverseActive == false) { currentAccState = AccState.NONE; driveDir = 0; }

            //Steering
            if (turnDir < 0) { currentTurnState = TurnState.LEFT; }
            else if (turnDir > 0) { currentTurnState = TurnState.RIGHT; }
            else { currentTurnState = TurnState.NONE; }
        }
        else
        {
            currentAccState = AccState.NONE;
            driveDir = 0;
            currentTurnState = TurnState.NONE;
        }

        //Set brake lights
        if (brakeLights != null)
            brakeLights.SetActive(brakeActive);

    }

    private void FixedUpdate()
    {
        foreach (NewTireScript tireScript in tireScripts)
        {
            tireScript.UpdateFriction();
            tireScript.UpdateBrake(brakeActive);
        }

        rLTire.UpdateDrive(driveDir);
        rRTire.UpdateDrive(driveDir);

        UpdateTurn_2();

    }
    
    //Collision events
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("oth_cars"))
        {
            //Collision with a car with force
            if (collision.GetContact(0).normalImpulse > 4.0f)
            {
                // Debug.Log("Collided with  a car with force : " + collision.GetContact(0).normalImpulse.ToString());
                DecreaseLife();
            }
        }

    }

    private void DecreaseLife()
    {
        if (lives > 0)
        {
            lives--;
            //fire life decrease event
           // AllEventsScript.OnCarLifeDecrease.Invoke(lives);

            if (lives == 0 && isCarOk)
            {
                OnCarDestroyed();
            }
        }
    }

    void OnCarDestroyed()
    {
        isCarOk = false;
        //Add smoke to bonnet
        var smk = Instantiate(smokePrefab, transform);
        smk.transform.localPosition = new Vector2(0, 0.232f);

       // AllEventsScript.OnCarDestroyed.Invoke();
    }

    /*
    void UpdateTurn()
    {
        switch (currentTurnState)
        {
            case TurnState.LEFT: desiredAngle = -lockAngle; break;
            case TurnState.RIGHT: desiredAngle = lockAngle; break;
            default: desiredAngle = 0;
                break;//nothing
        };

        float angleNow = fLJoint.jointAngle;
        float angleToTurn = desiredAngle - angleNow;
        angleToTurn = Mathf.Clamp(angleToTurn, -turnPerTimeStep, turnPerTimeStep);
        float newAngle = angleNow + angleToTurn;

        JointAngleLimits2D lim = new JointAngleLimits2D() { max = newAngle, min = newAngle };
        fLJoint.limits = lim;
        fRJoint.limits = lim;
      
    }
    */

    void UpdateTurn_2()
    {
        float motorSpeed = 0;
        switch (currentTurnState)
        {
            case TurnState.LEFT: desiredPos = clampMovePos; break;
            case TurnState.RIGHT: desiredPos = -clampMovePos; break;
            case TurnState.NONE: desiredPos = 0; break;
            default: break;//nothing
        };

        //If axle to center
        if (desiredPos == 0)
        {
            //If axle pos is almost 0
            if (Mathf.Abs(axleSlider.jointTranslation) > 0.0005f)
            {
                float dir = -axleSlider.jointTranslation;
                motorSpeed = (dir < 0) ? -turnSpeedIn : turnSpeedIn;
            }
            else if (Mathf.Abs(axleSlider.jointTranslation) < 0.0005f)
            {
            }
            else { motorSpeed = 0; }
        }
        else //If axle to any direction
        {
            float dir = axleSlider.jointTranslation - desiredPos;
            motorSpeed = (dir < 0) ? turnSpeedOut : -turnSpeedOut;
        }

       // Debug.Log(axleSlider.jointTranslation);

        JointMotor2D motor = new JointMotor2D() { motorSpeed = motorSpeed, maxMotorTorque = 50 };
        axleSlider.motor = motor;

    }

    //Methods to be called from controller.
    public void Accelerate(bool isTrue)
    {
        accelActive = isTrue;
    }

    public void Reverse(bool isTrue)
    {
        reverseActive = isTrue;
    }

    public void Brake(bool isTrue)
    {
        brakeActive = isTrue;
    }

    //Call this to turn car, dir=0=noturn, dir=1=turnright, dir=-1=turnleft
    public void Turn(int dir)
    {
        turnDir = dir;
    }

    //

    public void PauseCar()
    {
        isCarPaused = true;
    }

    public void ResumeCar()
    {
        isCarPaused = false;
    }

}

