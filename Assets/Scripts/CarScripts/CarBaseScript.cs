using UnityEngine;

public class CarBaseScript : MonoBehaviour
{
    [SerializeField]
    bool IsPlayerControlled = false;

    [SerializeField]
    bool HUDCarInfo = false;

    [SerializeField]
    [Range(0f, 1f)]
    float CGHeight = 0.55f;

    [SerializeField]
    [Range(0f, 2f)]
    float InertiaScale = 1f;

    [SerializeField]
    float BrakePower = 12000;

    [SerializeField]
    float EBrakePower = 5000;

    [SerializeField]
    [Range(0f, 1f)]
    float WeightTransfer = 0.35f;

    [SerializeField]
    [Range(0f, 1f)]
    float MaxSteerAngle = 0.75f;

    [SerializeField]
    [Range(0f, 20f)]
    float CornerStiffnessFront = 5.0f;

    [SerializeField]
    [Range(0f, 20f)]
    float CornerStiffnessRear = 5.2f;

    [SerializeField]
    [Range(0f, 20f)]
    float AirResistance = 2.5f;

    [SerializeField]
    [Range(0f, 20f)]
    float RollingResistance = 8.0f;

    [SerializeField]
    [Range(0f, 1f)]
    float EBrakeGripRatioFront = 0.9f;

    [SerializeField]
    [Range(0f, 5f)]
    float TotalTireGripFront = 2.5f;

    [SerializeField]
    [Range(0f, 1f)]
    float EBrakeGripRatioRear = 0.4f;

    [SerializeField]
    [Range(0f, 5f)]
    float TotalTireGripRear = 2.5f;

    [SerializeField]
    [Range(0f, 5f)]
    float SteerSpeed = 2.5f;

    [SerializeField]
    [Range(0f, 5f)]
    float SteerAdjustSpeed = 1f;

    [SerializeField]
    [Range(0f, 1000f)]
    float SpeedSteerCorrection = 300f;

    [SerializeField]
    [Range(0f, 20f)]
    float SpeedTurningStability = 10f;

    [SerializeField]
    [Range(0f, 10f)]
    float AxleDistanceCorrection = 2f;

    public float SpeedKilometersPerHour
    {
        get
        {
            return rBody.velocity.magnitude * 18f / 5f;
        }
    }

    // Variables that get initialized via code
    float Inertia = 1;
    float WheelBase = 1;
    float TrackWidth = 1;

    // Private vars
    float HeadingAngle;
    float AbsoluteVelocity;
    float AngularVelocity;
    float SteerDirection;
    float SteerAngle;

    Vector2 Velocity;
    Vector2 Acceleration;
    Vector2 LocalVelocity;
    Vector2 LocalAcceleration;

    float Throttle;
    float Brake;
    float EBrake;

    public Rigidbody2D rBody;

    Axle AxleFront;
    Axle AxleRear;
    Engine Engine;

    GameObject CenterOfGravity;

    //pedal flags
    bool pedal_accel;
    bool pedal_reverse;
    bool pedal_brake;
    //steering flags
    bool str_left;
    bool str_right;

    //
    //Unity methods
    //

    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        CenterOfGravity = transform.Find("CenterOfGravity").gameObject;

        AxleFront = transform.Find("AxleFront").GetComponent<Axle>();
        AxleRear = transform.Find("AxleRear").GetComponent<Axle>();

        Engine = transform.Find("Engine").GetComponent<Engine>();

        Init();
    }

    void Init()
    {
        Velocity = Vector2.zero;
        AbsoluteVelocity = 0;

        // Dimensions
        AxleFront.DistanceToCG = Vector2.Distance(CenterOfGravity.transform.position, AxleFront.transform.Find("Axle").transform.position);
        AxleRear.DistanceToCG = Vector2.Distance(CenterOfGravity.transform.position, AxleRear.transform.Find("Axle").transform.position);
        // Extend the calculations past actual car dimensions for better simulation
        AxleFront.DistanceToCG *= AxleDistanceCorrection;
        AxleRear.DistanceToCG *= AxleDistanceCorrection;

        WheelBase = AxleFront.DistanceToCG + AxleRear.DistanceToCG;
        Inertia = rBody.mass * InertiaScale;

        // Set starting angle of car
        rBody.rotation = transform.rotation.eulerAngles.z;
        HeadingAngle = (rBody.rotation + 90) * Mathf.Deg2Rad;
    }

    void Start()
    {
        AxleFront.Init(rBody, WheelBase);
        AxleRear.Init(rBody, WheelBase);

        TrackWidth = Vector2.Distance(AxleRear.TireLeft.transform.position, AxleRear.TireRight.transform.position);
    }

    void Update()
    {
        if (IsPlayerControlled)
        {
            // Handle Input
            Throttle = 0;
            Brake = 0;
            EBrake = 0;

            //switcing
            if (pedal_accel && !pedal_brake)
            {
                Throttle = 1;
            }
            else if (pedal_reverse && !pedal_brake)
            {
                //Brake = 1;
                Throttle = -1;
            }
            if (pedal_brake)
            {
                EBrake = 1;
            }

            //reset pedal positions
            pedal_accel = pedal_brake = pedal_reverse = false;

            //Steering
            float steerInput = 0;

            if (str_left) { steerInput = 1; }
            else if (str_right) { steerInput = -1; }

            /*
			if (Input.GetKeyDown(KeyCode.A))
			{
				Engine.ShiftUp();
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				Engine.ShiftDown();
			}
			*/

            // Apply filters to our steer direction
            SteerDirection = SmoothSteering(steerInput);
            SteerDirection = SpeedAdjustedSteering(SteerDirection);

            str_left = str_right = false;//reset steer flags

            // Calculate the current angle the tires are pointing
            SteerAngle = SteerDirection * MaxSteerAngle;

            // Set front axle tires rotation
            AxleFront.TireRight.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * SteerAngle);
            AxleFront.TireLeft.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * SteerAngle);
        }

        // Calculate weight center of four tires
        // This is just to draw that red dot over the car to indicate what tires have the most weight
        Vector2 pos = Vector2.zero;
        if (LocalAcceleration.magnitude > 1f)
        {

            float wfl = Mathf.Max(0, (AxleFront.TireLeft.ActiveWeight - AxleFront.TireLeft.RestingWeight));
            float wfr = Mathf.Max(0, (AxleFront.TireRight.ActiveWeight - AxleFront.TireRight.RestingWeight));
            float wrl = Mathf.Max(0, (AxleRear.TireLeft.ActiveWeight - AxleRear.TireLeft.RestingWeight));
            float wrr = Mathf.Max(0, (AxleRear.TireRight.ActiveWeight - AxleRear.TireRight.RestingWeight));

            pos = (AxleFront.TireLeft.transform.localPosition) * wfl +
                (AxleFront.TireRight.transform.localPosition) * wfr +
                (AxleRear.TireLeft.transform.localPosition) * wrl +
                (AxleRear.TireRight.transform.localPosition) * wrr;

            float weightTotal = wfl + wfr + wrl + wrr;

            if (weightTotal > 0)
            {
                pos /= weightTotal;
                pos.Normalize();
                pos.x = Mathf.Clamp(pos.x, -0.6f, 0.6f);
            }
            else
            {
                pos = Vector2.zero;
            }
        }

        // Update the "Center Of Gravity" dot to indicate the weight shift
        CenterOfGravity.transform.localPosition = Vector2.Lerp(CenterOfGravity.transform.localPosition, pos, 0.1f);

        /*
		// Skidmarks
		if (Mathf.Abs(LocalAcceleration.y) > 18 || EBrake == 1)
		{
			AxleRear.TireRight.SetTrailActive(true);
			AxleRear.TireLeft.SetTrailActive(true);
		}
		else
		{
			AxleRear.TireRight.SetTrailActive(false);
			AxleRear.TireLeft.SetTrailActive(false);
		}
		*/

        // Automatic transmission
        Engine.UpdateAutomaticTransmission(rBody);

    }

    void FixedUpdate()
    {
        // Update from rigidbody to retain collision responses
        Velocity = rBody.velocity;
        HeadingAngle = (rBody.rotation + 90) * Mathf.Deg2Rad;

        float sin = Mathf.Sin(HeadingAngle);
        float cos = Mathf.Cos(HeadingAngle);

        // Get local velocity
        LocalVelocity.x = cos * Velocity.x + sin * Velocity.y;
        LocalVelocity.y = cos * Velocity.y - sin * Velocity.x;

        // Weight transfer
        float transferX = WeightTransfer * LocalAcceleration.x * CGHeight / WheelBase;
        float transferY = WeightTransfer * LocalAcceleration.y * CGHeight / TrackWidth * 20;        //exagerate the weight transfer on the y-axis

        // Weight on each axle
        float weightFront = rBody.mass * (AxleFront.WeightRatio * -Physics2D.gravity.y - transferX);
        float weightRear = rBody.mass * (AxleRear.WeightRatio * -Physics2D.gravity.y + transferX);

        // Weight on each tire
        AxleFront.TireLeft.ActiveWeight = weightFront - transferY;
        AxleFront.TireRight.ActiveWeight = weightFront + transferY;
        AxleRear.TireLeft.ActiveWeight = weightRear - transferY;
        AxleRear.TireRight.ActiveWeight = weightRear + transferY;

        // Velocity of each tire
        AxleFront.TireLeft.AngularVelocity = AxleFront.DistanceToCG * AngularVelocity;
        AxleFront.TireRight.AngularVelocity = AxleFront.DistanceToCG * AngularVelocity;
        AxleRear.TireLeft.AngularVelocity = -AxleRear.DistanceToCG * AngularVelocity;
        AxleRear.TireRight.AngularVelocity = -AxleRear.DistanceToCG * AngularVelocity;

        // Slip angle
        AxleFront.SlipAngle = Mathf.Atan2(LocalVelocity.y + AxleFront.AngularVelocity, Mathf.Abs(LocalVelocity.x)) - Mathf.Sign(LocalVelocity.x) * SteerAngle;
        AxleRear.SlipAngle = Mathf.Atan2(LocalVelocity.y + AxleRear.AngularVelocity, Mathf.Abs(LocalVelocity.x));

        // Brake and Throttle power
        float activeBrake = Mathf.Min(Brake * BrakePower + EBrake * EBrakePower, BrakePower);
        float activeThrottle = (Throttle * Engine.GetTorque(rBody)) * (Engine.GearRatio * Engine.EffectiveGearRatio);

        // Torque of each tire (rear wheel drive)
        AxleRear.TireLeft.Torque = activeThrottle / AxleRear.TireLeft.Radius;
        AxleRear.TireRight.Torque = activeThrottle / AxleRear.TireRight.Radius;

        // Grip and Friction of each tire
        AxleFront.TireLeft.Grip = TotalTireGripFront * (1.0f - EBrake * (1.0f - EBrakeGripRatioFront));
        AxleFront.TireRight.Grip = TotalTireGripFront * (1.0f - EBrake * (1.0f - EBrakeGripRatioFront));
        AxleRear.TireLeft.Grip = TotalTireGripRear * (1.0f - EBrake * (1.0f - EBrakeGripRatioRear));
        AxleRear.TireRight.Grip = TotalTireGripRear * (1.0f - EBrake * (1.0f - EBrakeGripRatioRear));

        AxleFront.TireLeft.FrictionForce = Mathf.Clamp(-CornerStiffnessFront * AxleFront.SlipAngle, -AxleFront.TireLeft.Grip, AxleFront.TireLeft.Grip) * AxleFront.TireLeft.ActiveWeight;
        AxleFront.TireRight.FrictionForce = Mathf.Clamp(-CornerStiffnessFront * AxleFront.SlipAngle, -AxleFront.TireRight.Grip, AxleFront.TireRight.Grip) * AxleFront.TireRight.ActiveWeight;
        AxleRear.TireLeft.FrictionForce = Mathf.Clamp(-CornerStiffnessRear * AxleRear.SlipAngle, -AxleRear.TireLeft.Grip, AxleRear.TireLeft.Grip) * AxleRear.TireLeft.ActiveWeight;
        AxleRear.TireRight.FrictionForce = Mathf.Clamp(-CornerStiffnessRear * AxleRear.SlipAngle, -AxleRear.TireRight.Grip, AxleRear.TireRight.Grip) * AxleRear.TireRight.ActiveWeight;

        // Forces
        float tractionForceX = AxleRear.Torque - activeBrake * Mathf.Sign(LocalVelocity.x);
        float tractionForceY = 0;

        float dragForceX = -RollingResistance * LocalVelocity.x - AirResistance * LocalVelocity.x * Mathf.Abs(LocalVelocity.x);
        float dragForceY = -RollingResistance * LocalVelocity.y - AirResistance * LocalVelocity.y * Mathf.Abs(LocalVelocity.y);

        float totalForceX = dragForceX + tractionForceX;
        float totalForceY = dragForceY + tractionForceY + Mathf.Cos(SteerAngle) * AxleFront.FrictionForce + AxleRear.FrictionForce;

        //adjust Y force so it levels out the car heading at high speeds
        if (AbsoluteVelocity > 10)
        {
            totalForceY *= (AbsoluteVelocity + 1) / (21f - SpeedTurningStability);
        }

        // If we are not pressing gas, add artificial drag - helps with simulation stability
        if (Throttle == 0)
        {
            Velocity = Vector2.Lerp(Velocity, Vector2.zero, 0.005f);
        }

        // Acceleration
        LocalAcceleration.x = totalForceX / rBody.mass;
        LocalAcceleration.y = totalForceY / rBody.mass;

        Acceleration.x = cos * LocalAcceleration.x - sin * LocalAcceleration.y;
        Acceleration.y = sin * LocalAcceleration.x + cos * LocalAcceleration.y;

        // Velocity and speed
        Velocity.x += Acceleration.x * Time.fixedDeltaTime;
        Velocity.y += Acceleration.y * Time.fixedDeltaTime;

        AbsoluteVelocity = Velocity.magnitude;

        // Angular torque of car
        float angularTorque = (AxleFront.FrictionForce * AxleFront.DistanceToCG) - (AxleRear.FrictionForce * AxleRear.DistanceToCG);

        // Car will drift away at low speeds
        if (AbsoluteVelocity < 0.5f && activeThrottle == 0)
        {
            LocalAcceleration = Vector2.zero;
            AbsoluteVelocity = 0;
            Velocity = Vector2.zero;
            angularTorque = 0;
            AngularVelocity = 0;
            Acceleration = Vector2.zero;
            rBody.angularVelocity = 0;
        }

        var angularAcceleration = angularTorque / Inertia;

        // Update 
        AngularVelocity += angularAcceleration * Time.fixedDeltaTime;

        // Simulation likes to calculate high angular velocity at very low speeds - adjust for this
        if (AbsoluteVelocity < 1 && Mathf.Abs(SteerAngle) < 0.05f)
        {
            AngularVelocity = 0;
        }
        else if (SpeedKilometersPerHour < 0.75f)
        {
            AngularVelocity = 0;
        }

        HeadingAngle += AngularVelocity * Time.fixedDeltaTime;
        rBody.velocity = Velocity;

        rBody.MoveRotation(Mathf.Rad2Deg * HeadingAngle - 90);
    }

    float SmoothSteering(float steerInput)
    {

        float steer = 0;

        if (Mathf.Abs(steerInput) > 0.001f)
        {
            steer = Mathf.Clamp(SteerDirection + steerInput * Time.deltaTime * SteerSpeed, -1.0f, 1.0f);
        }
        else
        {
            if (SteerDirection > 0)
            {
                steer = Mathf.Max(SteerDirection - Time.deltaTime * SteerAdjustSpeed, 0);
            }
            else if (SteerDirection < 0)
            {
                steer = Mathf.Min(SteerDirection + Time.deltaTime * SteerAdjustSpeed, 0);
            }
        }

        return steer;
    }

    float SpeedAdjustedSteering(float steerInput)
    {
        float activeVelocity = Mathf.Min(AbsoluteVelocity, 250.0f);
        float steer = steerInput * (1.0f - (AbsoluteVelocity / SpeedSteerCorrection));
        return steer;
    }

    //
    //Car controls
    //

    public void PAccelerate() { pedal_accel = true; }

    public void PReverse() { pedal_reverse = true; }

    public void PBrake() { pedal_brake = true; }

    public void PTurnLeft() { str_left = true; }

    public void PTurnRight() { str_right = true; }


    //
    //Debug
    //

}
