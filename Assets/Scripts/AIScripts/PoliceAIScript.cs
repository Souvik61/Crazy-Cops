using UnityEngine;

public class PoliceAIScript : MonoBehaviour
{
    public Transform targetTransform;
    public Transform selfTransform;
    public float targetDistanceTolerance = 7.0f;
    public float brakeDistance = 7.0f;
    public float brakeVelocity = 7.0f;

    NewCarScript hostCarScript;
    Rigidbody2D targetRBody;
    float distanceToTarget;


    // Start is called before the first frame update
    private void Start()
    {
        targetRBody = targetTransform.GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        hostCarScript = GetComponentInChildren<NewCarScript>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector2.Distance(selfTransform.position, targetTransform.position);

        //If target too far
        if (distanceToTarget > targetDistanceTolerance)
        {
            Vector2 dirToTarget = (targetTransform.position - selfTransform.position).normalized;
            float dotProd = Vector2.Dot(selfTransform.up, dirToTarget);

            //move forward?
            hostCarScript.Accelerate((dotProd > 0));

            //move backward?
            hostCarScript.Reverse((dotProd < 0));

            //Turn logic
            float angleToDir = Vector2.SignedAngle(selfTransform.up, dirToTarget);

            if (angleToDir < 0 && Mathf.Abs(angleToDir) > 10) { hostCarScript.Turn(1); }
            else if (angleToDir > 0 && Mathf.Abs(angleToDir) > 10) { hostCarScript.Turn(-1); }
            else { hostCarScript.Turn(0); }
        }
        else//Reached target
        {
            hostCarScript.Accelerate(false);
            hostCarScript.Reverse(false);
            hostCarScript.Turn(0);
        }

        //Brake
        Vector2 tarVelocity = targetRBody.velocity;
        Vector2 selfVelocity = hostCarScript.rBody.velocity;
        float velo = hostCarScript.rBody.velocity.magnitude;

        Vector2 tmp = selfTransform.position - targetTransform.position;
        float relVelocity = -(Vector2.Dot(selfVelocity - tarVelocity, tmp) / tmp.magnitude);

        //Debug.Log(Mathf.Abs(relVelocity));

        if (distanceToTarget < brakeDistance && velo > brakeVelocity && Mathf.Abs(relVelocity) > 7.0f)
        {
            hostCarScript.Brake(true);
        }
        else { hostCarScript.Brake(false); }

    }
}
