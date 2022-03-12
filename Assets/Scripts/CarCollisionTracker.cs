using UnityEngine;

public class CarCollisionTracker : MonoBehaviour
{
    public static bool carInMaskCollider = false;

    private void OnEnable()
    {
        AllEventsScript.OnCarEnterMask += EventOnCarEnterMask;
        AllEventsScript.OnCarExitMask += EventOnCarExitMask;
    }

    private void OnDisable()
    {
        AllEventsScript.OnCarEnterMask -= EventOnCarEnterMask;
        AllEventsScript.OnCarExitMask -= EventOnCarExitMask;
    }

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 9, false);//Enable car collision w barriers if game restarts
    }

    void EventOnCarEnterMask()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Physics2D.IgnoreLayerCollision(29, 29, false);//Dummy layer collision
        carInMaskCollider = true;
    }
    void EventOnCarExitMask()
    {
        Physics2D.IgnoreLayerCollision(8, 9, false);
        Physics2D.IgnoreLayerCollision(29, 29, true);//Dummy layer collision
        carInMaskCollider = false;
    }
}
