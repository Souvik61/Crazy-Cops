using UnityEngine;

public class CastCheckScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("lamp_post"))
        {
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
