using UnityEngine;

public class CarBlockScript : MonoBehaviour
{
    public float yOffset;
    public Transform carTransform;

    float prevYPosition;
    // Start is called before the first frame update
    void Start()
    {
        yOffset = transform.position.y - carTransform.position.y;
        prevYPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float a = carTransform.position.y + yOffset;

        Vector2 newPos = new Vector2(carTransform.position.x, a);

        if (a > prevYPosition)
        {
            newPos.y = a;
            prevYPosition = a;
        }
        else
        {
            newPos.y = prevYPosition;
        }
        transform.position = newPos;
    }
}