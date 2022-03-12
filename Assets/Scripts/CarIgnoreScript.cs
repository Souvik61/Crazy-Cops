using UnityEngine;

public class CarIgnoreScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AllEventsScript.OnCarEnterMask();
            AllEventsScript.OnCarEnterMaskWObj(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("lamp_post"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AllEventsScript.OnCarExitMask();
            AllEventsScript.OnCarExitMaskWObj(gameObject);
        }
    }
}
