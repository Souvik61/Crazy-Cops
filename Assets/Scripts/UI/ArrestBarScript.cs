using UnityEngine;
using UnityEngine.UI;

public class ArrestBarScript : MonoBehaviour
{
    public Image barImg;
    public GameObject[] uiElements;


    bool isBarVisible;

    NewPoliceAIScript currPoliceAI;
    private void OnEnable()
    {
        AllEventsScript.OnArrestInitiated += OnArrestInitated;
        AllEventsScript.OnArrestFailed += OnArrestFailed;
    }

    private void OnDisable()
    {
        AllEventsScript.OnArrestInitiated -= OnArrestInitated;
        AllEventsScript.OnArrestFailed -= OnArrestFailed;
    }

    private void Awake()
    {
        isBarVisible = false;
    }

    private void Update()
    {
        if (isBarVisible && currPoliceAI != null)
        {
            barImg.fillAmount = currPoliceAI.arrestMeter;
        }
    }

    void EnableBar(bool value)
    {
        foreach (var gm in uiElements)
        {
            gm.SetActive(value);
        }
        isBarVisible = value;
    }

    void OnArrestInitated(NewPoliceAIScript policeAI)
    {
        currPoliceAI = policeAI;
        EnableBar(true);
    
    }

    void OnArrestFailed(NewPoliceAIScript policeAI)
    {
        currPoliceAI = null;
        EnableBar(false);
    }


}
