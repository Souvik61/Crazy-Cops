using UnityEngine;

public class ObjectPointerScript : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if(target!=null)
        transform.right = target.position - transform.position;
    }
}
