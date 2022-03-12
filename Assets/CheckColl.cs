using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColl : MonoBehaviour
{
    bool collStatus;
    // Update is called once per frame
    void Update()
    {
        collStatus = Physics2D.GetIgnoreLayerCollision(8, 9);
    }

    private void OnGUI()
    {
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = 25;
       // GUI.Label(new Rect(5, 5, 300, 40), "Car/Barrier Collision ignored: " + collStatus, gUIStyle);
    }
}
