using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{

    public float mRotateStepSpeed;
    public float mTopLimit;
    public float mBottomLimit;
    public float mCurrentRotation;
    public float mDesiredRotation;
    public float turnCoeff;

    private Rigidbody2D rBody;

    // Start is called before the first frame update
    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    /*
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            mDesiredRotation = mTopLimit;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mDesiredRotation = mBottomLimit;
        }
        else
        {
            mDesiredRotation = 0;
        }

        float toRotate = mDesiredRotation - mCurrentRotation;//Figure out to rotate

        toRotate = Mathf.Clamp(toRotate, -mRotateStepSpeed, mRotateStepSpeed) * 100 * Time.deltaTime;//Clamp rotation

        //Debug.Log(toRotate);
        mCurrentRotation += toRotate;

        //If currentRotation almost equals target rotation snap to target rot. 
        if (HelperScript.IsInRange(mCurrentRotation, mDesiredRotation - 0.0001f, mDesiredRotation + 0.00001f))
        {
            mCurrentRotation = mDesiredRotation;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, mCurrentRotation));


    }
    */

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            mDesiredRotation = mTopLimit;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mDesiredRotation = mBottomLimit;
        }
        else
        {
            mDesiredRotation = 0;
        }

        Debug.Log(rBody.velocity.magnitude);
        float velocity = Mathf.Clamp01(rBody.velocity.magnitude);

        turnCoeff = 1 - velocity;

        mCurrentRotation = RotationTo(mCurrentRotation, mDesiredRotation*turnCoeff, mRotateStepSpeed , mTopLimit, mBottomLimit);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, mCurrentRotation));

    }

    float RotationTo(float currentRotation, float desiredRotation, float stepSpeed, float topLimit, float bottomLimit)
    {

        float toRotate = desiredRotation - currentRotation;//Figure out to rotate

        toRotate = Mathf.Clamp(toRotate, -stepSpeed, stepSpeed) * 100 * Time.deltaTime;//Clamp rotation

        //Debug.Log(toRotate);
        currentRotation += toRotate;

        //If currentRotation almost equals target rotation snap to target rot. 
        if (HelperScript.IsInRange(currentRotation, desiredRotation - 0.0001f, desiredRotation + 0.00001f))
        {
            currentRotation = desiredRotation;
        }

        return currentRotation;
    }


}
