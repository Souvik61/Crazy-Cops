using UnityEngine;

public class HumanInputController : MonoBehaviour
{

   private NewCarScript carScript;

    bool isAccelerate;
    bool isBrake;
    bool isReverse;
    int turnDir;

    private void Awake()
    {
        carScript = GetComponent<NewCarScript>();
    }

    // Update is called once per frame
    void Update()
    {
        isAccelerate = isBrake = isReverse = false;
        turnDir = 0;

        //Accelerate
        if (Input.GetKey(KeyCode.W)) { isAccelerate = true; }
        //Reverse
        if (Input.GetKey(KeyCode.S)) { isReverse = true; }
        //Brake
        if (Input.GetKey(KeyCode.Space)) { isBrake = true; }

        //Turn
        if (Input.GetKey(KeyCode.A)) { turnDir = -1; }
        else if (Input.GetKey(KeyCode.D)) { turnDir = 1; }
        else { turnDir = 0; }

        //Pass values to car
        carScript.Accelerate(isAccelerate);
        carScript.Reverse(isReverse);
        carScript.Brake(isBrake);
        carScript.Turn(turnDir);

    }
}
