using UnityEditor;
using System.Collections;
using UnityEngine;

public class CarSpawnerScript : MonoBehaviour
{
    public float carSpawnEveryNSeconds;
    public CarTypesScrObj carTypes;
    public int carTypesRange;
    public GameObject car;
    public float[] xRanges_UpLane;
    public float[] xRanges_DwnLane;
    
    bool isPrevSpwnUpLane;
    uint prevSubLaneX_Up;
    uint prevSubLaneX_Down;

    CarTypesScrObj.VehicleTypes[] latestSpwnCarTypeInLane = { CarTypesScrObj.VehicleTypes.NONE, CarTypesScrObj.VehicleTypes.NONE, CarTypesScrObj.VehicleTypes.NONE, CarTypesScrObj.VehicleTypes.NONE };

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating(nameof(SpawnACar), 3, 3);
        StartCoroutine(CarSpawnCoroutine());
    }

    /*
    void SpawnACar()
    {
        bool isCurrSpwnUpLane = !isPrevSpwnUpLane;
        isPrevSpwnUpLane = isCurrSpwnUpLane;
        //Spawn in Up lane
        if (isCurrSpwnUpLane)
        {
            uint laneNum = (uint)Random.Range(2, 4);
            float laneXPos = xRanges[laneNum];
            Vector2 overlapPoint = new Vector2(laneXPos, car.transform.position.y + 20);

            Vector2 debugBoxSize = new Vector2(2.0f, 5);

            //If no collider present at point
            if (!IsOverlapingAnyCollider(overlapPoint, debugBoxSize))
            {
                //Spawn
                // Debug.Log("Spawn");
                SpawnCarAtLane(laneNum);
            }
            else
            {
                //Debug.Log("Car at lane");
            }

        }
        else //Spawn down lane
        {
            uint laneNum = (uint)Random.Range(0, 2);
            float laneXPos = xRanges[laneNum];
            Vector2 overlapPoint = new Vector2(laneXPos, car.transform.position.y + 20);

            Vector2 debugBoxSize = new Vector2(2.0f, 5);

            var collArr = Physics2D.OverlapBoxAll(overlapPoint, debugBoxSize, 0);


            //If no collider present at point
            if (!IsOverlapingAnyCollider(overlapPoint, debugBoxSize))
            {
                //Spawn
                //Debug.Log("Spawn");
                SpawnCarAtLane(laneNum);

            }
            else
            {
                //Debug.Log("Car at lane");
            }
        }

    }
    */

    IEnumerator CarSpawnCoroutine()
    {
        for ( ; ; )
        {
            //Spawn a car and wait
            SpawnACar();
            yield return new WaitForSeconds(carSpawnEveryNSeconds);
        }

    }

    void SpawnACar()
    {
        if (isPrevSpwnUpLane)//If previously spawned up lane then spawn down lane
        {
            //Down lane
            CarSpawnDownLane();
            isPrevSpwnUpLane = false;
        }
        else//If previously spawned down lane then spawn up lane
        {
            //Up lane
            CarSpawnUpLane();
            isPrevSpwnUpLane = true;
        }
    }

    void CarSpawnUpLane()
    {
        uint a = (ushort)Random.Range(0, 2);//Random between 0,1
        uint b = a + 2;//Index offset

        float laneXPos = xRanges_UpLane[a];

        Vector2 overlapPoint = new Vector2(laneXPos, car.transform.position.y + 20);

        Vector2 debugBoxSize = new Vector2(2.0f, 5);

        //If no collider present at point
        if (!IsOverlapingAnyCollider(overlapPoint, debugBoxSize))
        {
            //Spawn
            // Debug.Log("Spawn");
            SpawnCarAtLane(b, overlapPoint);
        }
    }
    void CarSpawnDownLane()
    {
        uint a = (ushort)Random.Range(0, 2);//Random between 0,1
        uint b = a;//Index offset

        float laneXPos = xRanges_DwnLane[a];
        Vector2 overlapPoint = new Vector2(laneXPos, car.transform.position.y + 20);
        Vector2 debugBoxSize = new Vector2(2.0f, 5);
        //If no collider present at point
        if (!IsOverlapingAnyCollider(overlapPoint, debugBoxSize))
        {
            //Spawn
            // Debug.Log("Spawn");
            SpawnCarAtLane(b, overlapPoint);
        }
    }

    /*
    //Spawn a car at lane
    void SpawnCarAtLane(uint lane)
    {
        //previous car in this lane
        var prevSpawnedCarInThisLane = latestSpwnCarTypeInLane[lane];

        var carToBeSpawned = (CarTypesScrObj.VehicleTypes)Random.Range(0, 4);

        while (prevSpawnedCarInThisLane == carToBeSpawned)
        {
            carToBeSpawned = (CarTypesScrObj.VehicleTypes)Random.Range(0, 4);
        }

        latestSpwnCarTypeInLane[lane] = carToBeSpawned;

        var carObjToSpawn = carTypes.GetCarAtIndex((uint)carToBeSpawned);
        carObjToSpawn = Instantiate(carObjToSpawn);

        //If down lane
        if (lane < 2)
        {
            //carObjToSpawn.transform.position = new Vector2(xRanges[Random.Range(0, 2)], car.transform.position.y + 20.0f);
        }
        else //Else up lane
        {
            carObjToSpawn.transform.rotation = Quaternion.Euler(0, 0, 0);
            carObjToSpawn.transform.position = new Vector2(xRanges_UpLane[Random.Range(2, 4)], car.transform.position.y + 20.0f);
        }


    }
   */

    //Spawn a car at lane
    void SpawnCarAtLane(uint lane, Vector2 spwnPos)
    {
        //previous car in this lane
        var prevSpawnedCarInThisLane = latestSpwnCarTypeInLane[lane];

        var carToBeSpawned = (CarTypesScrObj.VehicleTypes)Random.Range(0, carTypesRange);

        while (prevSpawnedCarInThisLane == carToBeSpawned)
        {
            carToBeSpawned = (CarTypesScrObj.VehicleTypes)Random.Range(0, carTypesRange);
        }

        latestSpwnCarTypeInLane[lane] = carToBeSpawned;

        var carObjToSpawn = carTypes.GetCarAtIndex((uint)carToBeSpawned);
        carObjToSpawn = Instantiate(carObjToSpawn);

        //Set spawned veihcle position

        //If down lane
        if (lane < 2)
        {
            carObjToSpawn.transform.rotation = Quaternion.Euler(0, 0, 180);
            carObjToSpawn.transform.position = spwnPos;
        }
        else //Else up lane
        {
            carObjToSpawn.transform.position = spwnPos;
        }

    }

    bool IsOverlapingAnyCollider(Vector2 midPoint, Vector2 size)
    {

        Vector2 boxSize = size;

        var arr = Physics2D.OverlapBoxAll(midPoint, boxSize, 0);

        Vector2 v1 = new Vector2(midPoint.x - boxSize.x / 2, midPoint.y - boxSize.y / 2);
        Vector2 v2 = new Vector2(midPoint.x + boxSize.x / 2, midPoint.y - boxSize.y / 2);
        Vector2 v3 = new Vector2(midPoint.x + boxSize.x / 2, midPoint.y + boxSize.y / 2);
        Vector2 v4 = new Vector2(midPoint.x - boxSize.x / 2, midPoint.y + boxSize.y / 2);

        Debug.DrawLine(v1, v2, Color.green, 0.5f);
        Debug.DrawLine(v2, v3, Color.green, 0.5f);
        Debug.DrawLine(v3, v4, Color.green, 0.5f);
        Debug.DrawLine(v4, v1, Color.green, 0.5f);

        //return if array length is not zero
        return (arr.Length != 0);
    }

}
