using System.Collections;
using UnityEngine;

public class DiscreteCarSpawnerScript : MonoBehaviour
{
    public bool spawnActive = false;
    public float spawnCarEveryNSecs;
    [SerializeField]
    Transform[] carSpawnPositions;
    [SerializeField]
    CarTypesScrObj carTypes;
    [SerializeField]
    Transform carParentTransf;

    private void OnEnable()
    {
        AllEventsScript.OnCarEnterMask += OnCarEnterMask;
        AllEventsScript.OnCarExitMask += OnCarExitMask;
    }

    private void OnDisable()
    {
        AllEventsScript.OnCarEnterMask -= OnCarEnterMask;
        AllEventsScript.OnCarExitMask -= OnCarExitMask;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(SpawnCarRoutine));
    }

    IEnumerator SpawnCarRoutine()
    {
        for (; ; )
        {
            SpawnCar();
            yield return new WaitForSeconds(spawnCarEveryNSecs);
        }
    }

    void SpawnCar()
    {
        if (spawnActive)
        {
            foreach (Transform t in carSpawnPositions)
            {
                int a = Random.Range(0, 5);
                GameObject car = Instantiate(carTypes.GetCarAtIndex((uint)a), t);
                //Set spawned car rotation
                car.transform.position = t.position;
                car.transform.rotation = t.rotation;
            }
        }
    }

    void OnCarEnterMask()
    {
        spawnActive = true;
    }

    void OnCarExitMask()
    {
        spawnActive = false;
    }

}
