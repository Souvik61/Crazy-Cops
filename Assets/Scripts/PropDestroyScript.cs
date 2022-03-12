using System.Collections;
using UnityEngine;

public class PropDestroyScript : MonoBehaviour
{
    public float drag;
    [Range(1000, 5000)]
    public float breakForce = 5000;
    Vector2 velocity;
    public GeneralAudioScript gAudioScript;
    public bool isDestructable;
    bool audioAvail;
    bool isHit = false;

    private void Awake()
    {
        GameObject gm = GameObject.FindWithTag("gen_audio");
        if (gm != null)
        {
            gAudioScript = gm.GetComponent<GeneralAudioScript>();
        }
        audioAvail = (gAudioScript != null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDestructable && !isHit)
        {
            ContactPoint2D con0 = collision.GetContact(0);
            float greatestImpulse = GetGreatestImpulse(collision);
            if (greatestImpulse > breakForce)
            {
                Vector2 force = con0.normal * (greatestImpulse / 100000);
                StartCoroutine("MoveRoutine", force);

                GetComponent<Collider2D>().enabled = false;

                isHit = true;
                //if audio source available play sound
                if (audioAvail)
                {
                    gAudioScript.PlayCarCrashSfx();
                }
            }
        }
    }

    IEnumerator MoveRoutine(Vector2 target_velocity)
    {
        velocity = target_velocity;
        while (velocity.magnitude > 0.001f)
        {
            transform.Translate(velocity, Space.World);
            velocity *= drag;//Add drag
            yield return null;
        }
        velocity = Vector2.zero;
        StartCoroutine(nameof(BlinkAndSelfDestructRoutine));
    }

    IEnumerator BlinkAndSelfDestructRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    //Find the greatest impulse in contacts
    float GetGreatestImpulse(Collision2D collision)
    {
        float greatestImp = collision.GetContact(0).normalImpulse;

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normalImpulse > greatestImp)
            {
                greatestImp = collision.GetContact(i).normalImpulse;
            }
        }

        return greatestImp;
    }
}
