using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownScript : MonoBehaviour
{
    public TMP_Text countdownText;

    int startTime = 3;

    public void StartTimer()
    {
        StartCoroutine(nameof(TimerCoroutine));
    }

    IEnumerator TimerCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            countdownText.text = startTime--.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        countdownText.gameObject.SetActive(false);

        AllEventsScript.OnCountdownOver?.Invoke();
    }

}

