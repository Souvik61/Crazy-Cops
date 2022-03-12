using System;
using UnityEngine;

public class NewHumanInputController : MonoBehaviour
{
    [SerializeField]
    CarBaseScript carScript;

    [SerializeField]
    GameObject smokePrefab;

    public bool isBeingArrested;
    public bool isArrested;

    //My vars --start
    public float distanceTravelled;//Distance travelled in unity units.

    uint hp;
    bool isCarOk;
    bool isPaused;

    //My vars --end

    void Awake()
    {
        isCarOk = true;
        hp = 3;
        isBeingArrested = false;
        isArrested = false;
        carScript = GetComponent<CarBaseScript>();
    }

    private void Start()
    {
        SetPaused(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isArrested && isCarOk && !isPaused)
        {
            //Drive
            if (Input.GetKey(KeyCode.W)) { carScript.PAccelerate(); }
            else if (Input.GetKey(KeyCode.S)) { carScript.PReverse(); }
            else if (Input.GetKey(KeyCode.Space)) { carScript.PBrake(); }

            //Steer
            if (Input.GetKey(KeyCode.A)) { carScript.PTurnLeft(); }
            else if (Input.GetKey(KeyCode.D)) { carScript.PTurnRight(); }
        }

        CalculateDistanceTravelled();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collision with a car with force
        if (collision.GetContact(0).normalImpulse > 7000.0f)
        {
            // Debug.Log("Collided with  a car with force : " + collision.GetContact(0).normalImpulse.ToString());
            OnDecreaseLife();
        }
    }

    public void SetPaused(bool enable)
    {
        isPaused = enable;
    }

    private void OnDecreaseLife()
    {
        if (hp > 0)
        {
            hp--;
            //fire life decrease event
            AllEventsScript.OnCarLifeDecrease.Invoke(hp);

            if (hp == 0 && isCarOk)
            {
                OnCarDestroyed();
            }
        }
    }

    void OnCarDestroyed()
    {
        isCarOk = false;
        //Add smoke to bonnet
        var smk = Instantiate(smokePrefab, transform);
        smk.transform.localPosition = new Vector2(0, 0.744f);

        AllEventsScript.OnCarDestroyed?.Invoke();
    }

    private void CalculateDistanceTravelled()
    {
        var vel = GetComponent<Rigidbody2D>().velocity;
        float dot = Vector2.Dot(vel.normalized, Vector2.up);
        //If car facing up count distance traveled
        if (dot > 0.9f)
        {
            distanceTravelled += vel.magnitude * Time.deltaTime;
        }
    }

    

}
