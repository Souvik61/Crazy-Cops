using System.Collections.Generic;
using UnityEngine;


public class CitySpawnerScript : MonoBehaviour
{

    public float roadLength;
    public uint spawnRdPerUnits;

    public Transform carTransform;
    public Transform upLimit;
    public GameObject cityPrefab;
    public GameObject cityPrefab1;

    uint prevSpwnCityID = 1;

    Dictionary<int, bool> waypointSpawnedChecklist;

    private void Awake()
    {
        waypointSpawnedChecklist = new Dictionary<int, bool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnCityRoutine), 1, 1);
    }

    void SpawnCityRoutine()
    {
        float nWP = FindNearestWayPoint(upLimit.position.y);

        if (Mathf.Abs(nWP - upLimit.position.y) < 50)
        {
            //If waypoint already not spawned
            if (!waypointSpawnedChecklist.ContainsKey((int)nWP))
            {
                //Randomize spawn
                uint spId = GenerateCityId(prevSpwnCityID);

                var gm = CreateCity(spId);
                gm.transform.position = new Vector2(0, nWP);

                prevSpwnCityID = spId;
                waypointSpawnedChecklist.Add((int)nWP, true);
            }
        }

    }

    bool CheckPositionInRange(Vector2 pos)
    {
        if (pos.y > 200)
        { return (pos.y % 300) < 50; }
        return false;
    }

    bool IsAWayPoint(float point)
    {
        if (point > 200)
        {if (point > 200)
            {
                return (point % 300 == 0);
            }
            return false;
        }
        return false;
    }

    float FindNearestWayPoint(float point)
    {
        if (point > 200)
        {
            return 300.0f * Mathf.CeilToInt(point / 300);
        }
        return 300;
    }

    uint GenerateCityId(uint prevId)
    {
        return (uint)(prevId == 0 ? 1 : 0);
    }

    GameObject CreateCity(uint id)
    {
        GameObject gm;

        if (id == 0)
        {
            gm = Instantiate(cityPrefab);
        }
        else
        {
            gm = Instantiate(cityPrefab1);
        }
        return gm;
    }


}
