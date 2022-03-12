using UnityEngine;

public class NewTireScript : MonoBehaviour
{

    public float maxDriveForce;
    public float maxForwardSpeed;
    public float maxBackwardSpeed;
    public float maxLateralImpulse;


    bool isBraking;

    Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
       // if (Input.GetKey(KeyCode.W)) { driveDir = 1; }
       // else if (Input.GetKey(KeyCode.S)) { driveDir = -1; }
       // else { driveDir = 0; }

    }

    void FixedUpdate()
    {
       // UpdateFriction();
       // UpdateDrive();
    }

    //Other functions
    Vector2 GetLateralVelocity()
    {
        Vector2 currentRightNormal = transform.TransformDirection(new Vector2(1, 0));
        return Vector2.Dot(currentRightNormal, rBody.velocity) * currentRightNormal;
    }

    Vector2 GetForwardVelocity()
    {
        Vector2 currentForwardNormal = transform.TransformDirection(new Vector2(0, 1));
        return Vector2.Dot(currentForwardNormal, rBody.velocity) * currentForwardNormal;
    }

    public void UpdateFriction()
    {
        Vector2 impulse = rBody.mass * -GetLateralVelocity();

        if (impulse.magnitude > maxLateralImpulse)
            impulse *= maxLateralImpulse / impulse.magnitude;

        rBody.AddForceAtPosition(impulse, rBody.worldCenterOfMass, ForceMode2D.Impulse);

        rBody.AddTorque(0.01f * rBody.inertia * -rBody.angularVelocity, ForceMode2D.Impulse);

        Vector2 currentForwardNormal = GetForwardVelocity();
        float currentForwardSpeed = currentForwardNormal.magnitude;
        currentForwardNormal.Normalize();
        float dragForceMagnitude = -1.0f * currentForwardSpeed;
        rBody.AddForceAtPosition(dragForceMagnitude * currentForwardNormal, rBody.worldCenterOfMass);
    }

    public void UpdateDrive(int driveDir)
    {
        //find desired speed
        if (driveDir != 0 && !isBraking)
        {
            float desiredSpeed = 0;
            if (driveDir > 0) { desiredSpeed = maxForwardSpeed; }
            else if (driveDir < 0) { desiredSpeed = -maxBackwardSpeed; }

            //find current speed in forward direction
            Vector2 currentForwardNormal = transform.TransformDirection(new Vector2(0, 1));
            float currentSpeed = Vector2.Dot(GetForwardVelocity(), currentForwardNormal);

            //apply necessary force
            float force = 0;
            if (desiredSpeed > currentSpeed)
                force = maxDriveForce;
            else if (desiredSpeed < currentSpeed)
                force = -maxDriveForce;
            else
                return;
            rBody.AddForceAtPosition(force * currentForwardNormal, rBody.worldCenterOfMass);
        }
    }

    public void UpdateBrake(bool isTrue)
    {
        isBraking = isTrue;
        if (isTrue)
        { rBody.AddForce(-rBody.velocity.normalized * 10); }
    }
}