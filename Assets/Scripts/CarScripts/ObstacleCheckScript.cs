using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCheckScript : MonoBehaviour
{

    public Transform castStart;
    public float castDistance = 1;

    public bool isObstAhead;
    public LayerMask ignoreLayerMask;

    GameObject currentObstacle;
    public float currentObstacleClosingSpeed;
    [SerializeField]
    Rigidbody2D rBody;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(CheckObstacle), 1, 0.5f);
    }

    void CheckObstacle()
    {
        LayerMask t_ignLayerMask = ignoreLayerMask;

        Vector2 castStartPt = castStart.position;

        Bounds bound = new Bounds(castStartPt, new Vector2(0.45f,0.45f));

        //Layer mask black magic------>start
        bool ignf = Physics2D.GetIgnoreLayerCollision(29, 29);

        if (!ignf)
        {
            t_ignLayerMask = ignoreLayerMask | (1 << 9);
        }

        t_ignLayerMask = ~t_ignLayerMask;

        //Layer mask black magic------>end


        RaycastHit2D hit = Physics2D.CircleCast(castStartPt, 0.225f, transform.up, castDistance, t_ignLayerMask);

        HelperScript.DrawBoundDebug(bound, Color.green);

        bound.center = new Vector2(castStartPt.x + transform.up.x * castDistance, castStartPt.y + transform.up.y * castDistance);

        HelperScript.DrawBoundDebug(bound,Color.green);
        //HelperScript.DrawPointDebug(castStart, Color.red);

        /*
        {
            Vector2 center = new Vector2(castStart.x + transform.up.x * castDistance, castStart.y + transform.up.y * castDistance);
            Vector2 A = center - new Vector2(0.5f, 0.5f);
            Vector2 B = new Vector2(center.x - 0.5f, center.y + 0.5f);
            Vector2 C = center + new Vector2(0.5f, 0.5f);
            Vector2 D = new Vector2(center.x + 0.5f, center.y - 0.5f);

            Debug.DrawLine(A, B,Color.green,0.25f);
            Debug.DrawLine(B, C,Color.green,0.25f);
            Debug.DrawLine(C, D,Color.green,0.25f);
            Debug.DrawLine(D, A,Color.green, 0.25f);
         }
        */

        if (hit)
        {
            //Debug.Log(hit.collider.gameObject.name);
            currentObstacle = hit.collider.gameObject;
            isObstAhead = true;
        }
        else
        {
            //Debug.Log("None");
            currentObstacle = null;
            isObstAhead = false;
        }

        currentObstacleClosingSpeed = CurrentObstacleClosingSpeed();

    }

    public float CurrentObstacleClosingSpeed()
    {
        if (currentObstacle != null)
        {
            Vector2 tmp = transform.position - currentObstacle.transform.position;

            var velA = rBody.velocity;
            Vector2 velB = new Vector2();
            Rigidbody2D tempBody;

            //if bodyB has a rigidbody
            if (currentObstacle.TryGetComponent<Rigidbody2D>(out tempBody))
            {
                velB = tempBody.velocity;
            }

            float relVel = -(Vector2.Dot((velA - velB), tmp) / tmp.magnitude);
            return (relVel < 0.0001f) ? 0 : relVel;
        }
        return 0.0f;
    }

}
