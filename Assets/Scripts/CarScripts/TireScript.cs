using UnityEngine;

public class OTireScript : MonoBehaviour
{
    //tire class variables
    public float m_maxForwardSpeed;  // 100;
    public float m_maxBackwardSpeed; // -20;
    public float m_maxDriveForce;    // 150;
    public float maxLateralImpulse;    // 150;
 
    Rigidbody2D rBody;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
       // UpdateFriction();
        //UpdateDrive(currentAccState);
        //UpdateTurn(currentTurnState);
    }

    //Other functions

    Vector2 GetForwardVelocity()
    {
        Vector2 currentRightNormal = transform.TransformDirection(new Vector2(0, 1));
        return Vector2.Dot(currentRightNormal, rBody.velocity) * currentRightNormal;
    }

    Vector2 GetLateralVelocity()
    {
        Vector2 currentRightNormal = transform.TransformDirection(new Vector2(1, 0));
        return Vector2.Dot(currentRightNormal, rBody.velocity) * currentRightNormal;
    }

    public void UpdateFriction()
    {
        Vector2 impulse = rBody.mass * -GetLateralVelocity();

        if (impulse.magnitude > maxLateralImpulse) { impulse *= maxLateralImpulse / impulse.magnitude; }

        rBody.AddForce(impulse, ForceMode2D.Impulse);

        //rBody.AddTorque(100.0f * rBody.inertia * -rBody.angularVelocity);

        rBody.AddTorque(0.001f * rBody.inertia * -rBody.angularVelocity, ForceMode2D.Impulse);

        Vector2 currentForwardNormal = GetForwardVelocity();
        float currentForwardSpeed = 1.0f;
        currentForwardNormal.Normalize();
        float dragForceMagnitude = -2 * currentForwardSpeed;
        rBody.AddForce(dragForceMagnitude * currentForwardNormal);
    }

   /* public void UpdateDrive(CarScript.AccState state)
    {
        //find desired speed
        float desiredSpeed = 0;
        switch (state)
        {
            case CarScript.AccState.UP: desiredSpeed = m_maxForwardSpeed; break;
            case CarScript.AccState.DOWN: desiredSpeed = m_maxBackwardSpeed; break;
            default: return;//do nothing
        }

        //find current speed in forward direction
        Vector2 currentForwardNormal = transform.TransformDirection(new Vector2(0, 1));
        float currentSpeed = Vector2.Dot(GetForwardVelocity(), currentForwardNormal);

        //apply necessary force
        float force = 0;
        if (desiredSpeed > currentSpeed)
            force = m_maxDriveForce;
        else if (desiredSpeed < currentSpeed)
            force = -m_maxDriveForce;
        else
            return;
        rBody.AddForce(force * currentForwardNormal);
    }*/

   /* void UpdateTurn(TurnState state)
    {
        float desiredTorque = 0;
        switch (state)
        {
            case TurnState.LEFT: desiredTorque = 15; break;
            case TurnState.RIGHT: desiredTorque = -15; break;
            default: break;
        }
        rBody.AddTorque(desiredTorque);
    }
   */
}
