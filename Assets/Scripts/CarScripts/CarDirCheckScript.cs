using UnityEngine;

public class CarDirCheckScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        float dotProd = Vector2.Dot(transform.up, Vector2.down);
        if (dotProd > 0.9)
        {
            // ObjA is looking mostly towards ObjB
            Debug.Log("Car is facing down");
        }
    }
}
