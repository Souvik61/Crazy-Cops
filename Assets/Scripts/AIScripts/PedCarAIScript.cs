using UnityEngine;

public class PedCarAIScript : MonoBehaviour
{
    public float targetSpeed;
    public float brakeClosingSpeed;

    [SerializeField]
    private ObstacleCheckScript obsCheck;
    [SerializeField]
    private CarBaseScript carScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (carScript.SpeedKilometersPerHour < targetSpeed - 1.0f)
        {
            if (!obsCheck.isObstAhead)//If no obstacle ahead
            {
                carScript.PAccelerate();
               // Debug.Log("Accelerate");
            }
        }

        if (obsCheck.currentObstacleClosingSpeed > brakeClosingSpeed)
        {
            carScript.PBrake();
            //Debug.Log("Brake");
        
        }

       // Debug.Log(obsCheck.ObstacleClosingSpeed());

    }
}
