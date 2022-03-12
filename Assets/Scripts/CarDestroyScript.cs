using UnityEngine;

public class CarDestroyScript : MonoBehaviour
{
    public LayerMask objectDestroyMask;
    public string objectNotDestroyTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != objectNotDestroyTag)//If collision tag is not Nottodestroytag
        {
            Destroy(collision.gameObject);
        }
    }

}
