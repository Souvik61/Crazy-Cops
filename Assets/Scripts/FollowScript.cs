using UnityEngine;

[ExecuteInEditMode]
public class FollowScript : MonoBehaviour
{
    public Transform toFollow;
    public Vector3 offset;

    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;


    // Update is called once per frame
    void Update()
    {
        if (toFollow != null)
        {
            float nX = offset.x + toFollow.position.x * (freezeX ? 0 : 1);
            float nY = offset.y + toFollow.position.y * (freezeY ? 0 : 1);
            float nZ = offset.z + toFollow.position.z * (freezeZ ? 0 : 1);

            transform.position = new Vector3(nX, nY, nZ);
        }
    }
}
