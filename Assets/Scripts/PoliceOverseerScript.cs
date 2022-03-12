using System.Collections.Generic;
using UnityEngine;

public class PoliceOverseerScript : MonoBehaviour
{
    public GameObject policeCarPrefab;
    public Transform target;

    public Transform upLimit;
    public Transform downLimit;

    public const int preferedNoOfPolice = 5;

    public List<GameObject> currentPoliceCars;

    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        SetPaused(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            CheckToSpawnACar();
            RemoveExtaPoliceCars();
        }
    }

    public void SetPaused(bool enable)
    {
        isPaused = enable;
    }

    private void CheckToSpawnACar()
    {
        if (currentPoliceCars.Count < preferedNoOfPolice)
        {
            GameObject pCar = Instantiate(policeCarPrefab);
            pCar.transform.position = new Vector2(target.position.x + Random.Range(-2.0f, 2), target.position.y + Random.Range(-8.0f, -10));

            pCar.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-20, 20));//Set a random rotation for police cars
            pCar.GetComponent<NewPoliceAIScript>().targetTransform = target;//Set spawned cars target
            currentPoliceCars.Add(pCar);
        }
    }

    private void RemoveExtaPoliceCars()
    {
        List<GameObject> carsToDest = new List<GameObject>();

        //Iterate through all active police cars
        foreach (GameObject car in currentPoliceCars)
        {
            float cPosY = car.transform.position.y;

            if (cPosY > upLimit.position.y || cPosY < downLimit.position.y)
            {
                carsToDest.Add(car);
            }

        }
        foreach (var item in carsToDest)
        {
            currentPoliceCars.Remove(item);
            Destroy(item);
        }
    }

}
