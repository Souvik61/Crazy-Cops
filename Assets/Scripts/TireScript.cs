using UnityEngine;

public class TireScript : MonoBehaviour
{

    public float maxLateralImpulse;

    public float m_maxForwardSpeed;  // 100;
    public float m_maxBackwardSpeed; // -20;
    public float m_maxDriveForce;    // 150;

    Rigidbody2D rBody;

    enum ControlState { NONE, UP, DOWN };
    ControlState controlState;

    enum TurnState { NONE, LEFT, RIGHT };
    TurnState turnState;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Set control state
        if (Input.GetKey(KeyCode.W))
        { controlState = ControlState.UP; }
        else if (Input.GetKey(KeyCode.S))
        { controlState = ControlState.DOWN; }
        else
        { controlState = ControlState.NONE; }

        //Set turn state
        if (Input.GetKey(KeyCode.A))
        { turnState = TurnState.LEFT; }
        else if (Input.GetKey(KeyCode.D))
        { turnState = TurnState.RIGHT; }
        else
        { turnState = TurnState.NONE; }
    }

    void FixedUpdate()
    {
        UpdateFriction();
        UpdateDrive();
       // UpdateTurn();
    }

    Vector2 GetLateralVelocity()
    {
        Vector2 currentRightNormal = transform.TransformDirection(new Vector2(1, 0));
        return Vector2.Dot(currentRightNormal, rBody.velocity) * currentRightNormal;
    }

    Vector2 GetForwardVelocity()
    {
        Vector2 currentRightNormal = transform.TransformDirection(new Vector2(0, 1));
        return Vector2.Dot(currentRightNormal, rBody.velocity) * currentRightNormal;
    }

    /*void UpdateFriction()
    {
        //lateral linear velocity
        Vector2 impulse = rBody.mass * -GetLateralVelocity();

        rBody.AddForce(impulse, ForceMode2D.Impulse);

        //angular velocity
        rBody.AddTorque(0.1f * rBody.inertia * -rBody.angularVelocity);

        //forward linear velocity
        Vector2 currentForwardNormal = GetForwardVelocity();
        float currentForwardSpeed = currentForwardNormal.magnitude;
        currentForwardNormal.Normalize();
        float dragForceMagnitude = -2 * currentForwardSpeed;
        rBody.AddForce( dragForceMagnitude * currentForwardNormal);
    }
    */

    void UpdateFriction()
    {
        Vector2 impulse = rBody.mass * -GetLateralVelocity();
        if (impulse.magnitude > maxLateralImpulse)
        { impulse *= maxLateralImpulse / impulse.magnitude; }
        rBody.AddForce(impulse, ForceMode2D.Impulse);

        rBody.AddTorque(0.1f * rBody.inertia * -rBody.angularVelocity);

    }

    void UpdateDrive()
    {
        //find desired speed
        float desiredSpeed = 0;
        switch (controlState)
        {
            case ControlState.UP: desiredSpeed = m_maxForwardSpeed; break;
            case ControlState.DOWN: desiredSpeed = m_maxBackwardSpeed; break;
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
    }

    void UpdateTurn()
    {
        float desiredTorque = 0;
        switch (turnState)
        {
            case TurnState.LEFT: desiredTorque = 0.2f; break;
            case TurnState.RIGHT: desiredTorque = -0.2f; break;
            default:break;//nothing
        }
        rBody.AddTorque(desiredTorque);
    }
}
