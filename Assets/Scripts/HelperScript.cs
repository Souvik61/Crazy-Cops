using UnityEngine;

public class HelperScript
{
    public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public static bool IsInRange(float input, float min, float max)
    {
        return (input > min && input < max) ? true : false;
    }

    public static float RotationTo(float currentRotation, float desiredRotation, float stepSpeed, float topLimit, float bottomLimit)
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

    //To draw debug bounding box
    public static void DrawBoundDebug(Bounds bound, Color color, float time = 0.25f)
    {
        Vector2 A = bound.min;
        Vector2 B = new Vector2(bound.min.x, bound.max.y);
        Vector2 C = bound.max;
        Vector2 D = new Vector2(bound.max.x, bound.min.y);
        Debug.DrawLine(A, B, color, time);
        Debug.DrawLine(B, C, color, time);
        Debug.DrawLine(C, D, color, time);
        Debug.DrawLine(D, A, color, time);
    }

    public static void DrawPointDebug(Vector2 position, Color color, float time = 0.25f)
    {
        Vector2 v = new Vector2(0.1f, 0.1f);
        Vector2 A = position - v;
        Vector2 B = position + v;
        Vector2 C = position + new Vector2(0.1f, -0.1f);
        Vector2 D = position + new Vector2(-0.1f, +0.1f);
        Debug.DrawLine(A, B, color, time);
        Debug.DrawLine(C, D, color, time);
    }

}
