using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetTestScript : MonoBehaviour
{
    Vector2 velocity;
    public float drag;

    private void OnEnable()
    {
        StartCoroutine("MoveRoutine", new Vector2(0, 1));
    }

    IEnumerator MoveRoutine(Vector2 target_velocity)
    {
        velocity = target_velocity;
        while (velocity.magnitude > 0.001f)
        {
            transform.Translate(velocity, Space.World);
            velocity *= drag;//Add drag
            yield return null;
        }
        velocity = Vector2.zero;
    }
}
