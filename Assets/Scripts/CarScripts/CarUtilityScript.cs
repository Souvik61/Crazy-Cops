using UnityEngine;

public class CarUtilityScript : MonoBehaviour
{
    public GameObject headLights;
    public AudioSource audioSource;

    //light flags
    bool isHeadlightOn;
    bool isHonking;

    // Update is called once per frame
    void Update()
    {
        //Set headlight
        if (Input.GetKeyDown(KeyCode.L))
        {
            isHeadlightOn = !isHeadlightOn;
            headLights.SetActive(isHeadlightOn);
        }

        //horn
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHonking = true;
            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            audioSource.Stop();
            isHonking = false;
        }
    }
}
