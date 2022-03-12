using UnityEngine;

[CreateAssetMenu(menuName = "Car Types")]
public class CarTypesScrObj : ScriptableObject
{
    public GameObject truck;
    public GameObject small_truck;
    public GameObject pickup_2;
    public GameObject pickup;
    public GameObject suv;

    public enum VehicleTypes { NONE, TRUCK, SM_TRUCK, PICKUP_2, PICKUP, SUV };
    GameObject[] vehicles = new GameObject[5];


    private void OnEnable()
    {
        vehicles[0] = truck;
        vehicles[1] = small_truck;
        vehicles[2] = pickup_2;
        vehicles[3] = pickup;
        vehicles[4] = suv;
    }

    public GameObject GetCarAtIndex(uint index)
    {
        return vehicles[index];
    }
}
