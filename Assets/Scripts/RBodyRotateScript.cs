using UnityEngine;

public class RBodyRotateScript : MonoBehaviour
{

    public float mTopLimit;
    public float mBottomLimit;
    public float mStepSpeed;

    private float mDesiredRotation;
    private float mcurrentRotation;
    private HingeJoint2D mHingeJoint;

    public float turnCoeff;
    private Rigidbody2D rBody;


    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        mHingeJoint = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            mDesiredRotation = mTopLimit;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            mDesiredRotation = mBottomLimit;
        }
        else
        {
            mDesiredRotation = 0;
        }

        //Debug.Log(rBody.velocity.magnitude);

        float velocity = Mathf.Clamp01(rBody.velocity.magnitude);

        turnCoeff = 1 - velocity;

        mcurrentRotation = HelperScript.RotationTo(mcurrentRotation, mDesiredRotation , mStepSpeed, mTopLimit, mBottomLimit);

        JointAngleLimits2D limit = new JointAngleLimits2D();
        limit.max = mcurrentRotation + 0.01f;
        limit.min = mcurrentRotation - 0.01f;

        mHingeJoint.limits = limit;

    }
}
