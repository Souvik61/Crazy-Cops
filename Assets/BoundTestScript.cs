using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class BoundTestScript : MonoBehaviour
{
    public Grid grid;

    float minX = 0, minY = 0;
    float maxX = 0, maxY = 0;

    private void OnEnable()
    {
        minX = minY = maxX = maxY = 0;
        Tilemap[] tilemapArray = GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tmp in tilemapArray)
        {
            tmp.CompressBounds();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Tilemap[] tilemapArray = GetComponentsInChildren<Tilemap>();


        for (int i = 0; i < tilemapArray.Length; i++)
        {
            Tilemap tmp = tilemapArray[i];

            Bounds bound = tmp.localBounds;

            Vector2 A = tmp.transform.TransformPoint(bound.min);
            Vector2 B = tmp.transform.TransformPoint(new Vector2(bound.min.x, bound.max.y));
            Vector2 C = tmp.transform.TransformPoint(bound.max);
            Vector2 D = tmp.transform.TransformPoint(new Vector2(bound.max.x, bound.min.y));

            Vector2 bcenter = tmp.transform.TransformPoint(bound.center);
            Vector2 bsize = bound.size;

            Bounds bound1 = new Bounds(bcenter, bsize);

            HelperScript.DrawBoundDebug(bound1, Color.green);

            
            if (i == 0)//First loop iteration
            {
                minX = A.x; minY = A.y;
                maxX = C.x; maxY = C.y;
            }
            else
            {
                //Calculate min/max point
                //min point
                if (A.x < minX) { minX = A.x; }
                if (A.y < minY) { minY = A.y; }
                //max point
                if (C.x > maxX) { maxX = C.x; }
                if (C.y > maxY) { maxY = C.y; }
            }

        }

        Vector2 size = new Vector2(maxX - minX, maxY - minY);
        Vector2 center = new Vector2(minX, minY) + size / 2;
        var b = new Bounds(center, size);
        //Draw total bound
        HelperScript.DrawBoundDebug(b, Color.red);
    }
}
