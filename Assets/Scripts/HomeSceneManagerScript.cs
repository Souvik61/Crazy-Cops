using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

}
