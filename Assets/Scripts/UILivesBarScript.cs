using UnityEngine;
using UnityEngine.UI;

public class UILivesBarScript : MonoBehaviour
{
    //Sprites to display lives
    [SerializeField]
    Image live1_sp;
    [SerializeField]
    Image live2_sp;
    [SerializeField]
    Image live3_sp;

    //Set how many lives left parameters can be 0,1,2,3
    public void SetHP(uint count)
    {
        switch (count)
        {
            case 0:
                {
                    live1_sp.enabled = false;
                    live2_sp.enabled = false;
                    live3_sp.enabled = false;
                }
                break;
            case 1:
                {
                    live1_sp.enabled = true;
                    live2_sp.enabled = false;
                    live3_sp.enabled = false;
                }
                break;
            case 2:
                {
                    live1_sp.enabled = true;
                    live2_sp.enabled = true;
                    live3_sp.enabled = false;
                }
                break;
            case 3:
                {
                    live1_sp.enabled = true;
                    live2_sp.enabled = true;
                    live3_sp.enabled = true;
                }
                break;
            default:
                break;
        }
    }
}
