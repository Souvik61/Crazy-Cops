using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawnerScript : MonoBehaviour
{
    public GameObject roadPrefab_t1;
    public GameObject roadPrefab_t2;
    [SerializeField]
    private Transform rdParent;//Use as a parent transform for all roads for cleaner hierarchy
    public GameObject initialRoad;
    public GameObject Car;

    public Transform carUpLimit;
    public Transform carDownLimit;

    GameObject currFocusRoad;
    public List<GameObject> roads = new List<GameObject>();

    Vector2 roadBoundSize = new Vector2(20, 10);

    // Start is called before the first frame update
    void Start()
    {
        currFocusRoad = initialRoad;
        roads.Add(currFocusRoad);

        InvokeRepeating(nameof(RemoveExtraRoads), 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawnForUpLimit();
        CheckSpawnForDownLimit();
        //RemoveExtraRoads();
    }

    private void RemoveExtraRoads()
    {
        List<GameObject> rdsToDest = new List<GameObject>();
        foreach (var road in roads)
        {
            if (road.transform.position.y < carDownLimit.transform.position.y-10 || road.transform.position.y > carUpLimit.transform.position.y+10)
            {
                rdsToDest.Add(road);
            }
        }

        foreach (var item in rdsToDest)
        {
            roads.Remove(item);
            Destroy(item);
        }
    }

    void CheckSpawnForUpLimit()
    {
        bool inABox = false;
        //Check if a uplimit is in a box
        foreach (GameObject road in roads)
        {
            Vector2 rPos = road.transform.position;
            Bounds currentRdBound = new Bounds(rPos, roadBoundSize);

            //Check if up limit is in bound
            if (currentRdBound.Contains((Vector2)carUpLimit.position))
            {
                inABox = true;
            }
            // Or Check if up limit is in a city
            if (CheckIfCity(carUpLimit.position))
            {
                inABox = true;
            }
        }

        if (!inABox)//Have to spawn a road
        {
            if (((int)(carUpLimit.position.y % 10.0f)) != 0)
            {
                //Check spawn position
                var c = Mathf.Ceil(carUpLimit.position.y / 10.0f);
                GameObject road;

                //Spawn road as location type
                if (Mathf.Abs(c) % 2 == 0)//If c even
                { road = CreateRoadwType(Vector2.zero, 0); }
                else//If c odd
                { road = CreateRoadwType(Vector2.zero, 1); }

                //position it
                float posY = 10.0f * c;
                road.transform.position = new Vector2(0, posY);

                roads.Add(road);//Add to list
            }
        }
    }

    void CheckSpawnForDownLimit()
    {
        bool inABox = false;

        //Check if a downlimit is in a box
        foreach (GameObject road in roads)
        {
            Vector2 rPos = road.transform.position;
            Bounds currentRdBound = new Bounds(rPos, roadBoundSize);
            
            //Check if down limit is in bound
            if (currentRdBound.Contains((Vector2)carDownLimit.position))
            {
                inABox = true;
            }
            // Or Check if up limit is in a city
            if (CheckIfCity(carDownLimit.position))
            {
                inABox = true;
            }
        }

        if (!inABox)//Have to spawn a road
        {
            if (((int)(carDownLimit.position.y % 10.0f)) != 0)
            {
                //Check spawn position
                var c = Mathf.Floor(carDownLimit.position.y / 10.0f);
                GameObject road;

                //Spawn road as location type
                if (Mathf.Abs(c) % 2 == 0)//If c even
                { road = CreateRoadwType(Vector2.zero, 0); }
                else//If c odd
                { road = CreateRoadwType(Vector2.zero, 1); }

                //position it
                float posY = 10.0f * c;
                road.transform.position = new Vector2(0, posY);

                roads.Add(road);//Add to list
            }
        }
    }

    GameObject CreateRoadwType(Vector2 position, uint type)
    {
        GameObject road;

        if (type == 0)//Road type 1
        { road = Instantiate(roadPrefab_t1, rdParent); }
        else//Road type 2
        { road = Instantiate(roadPrefab_t2, rdParent); }

        road.transform.position = position;
        return road;
    }

    bool CheckIfCity(Vector2 position)
    {
        Collider2D coll = Physics2D.OverlapPoint(position, 1<<13);

        if (coll != null)
        {
            return coll.CompareTag("city_mask");
        }
        return false;
    }
}
