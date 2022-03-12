using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject gameOverText;
    [SerializeField]
    UILivesBarScript livesBarScript;
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    NewHumanInputController carScript;
    [SerializeField]
    CountdownScript countScript;
    [SerializeField]
    PoliceOverseerScript policeOverseerScr;


    //Internal vars

    uint carScore;

    public enum GameState {NONE,READY,RUNNING,OVER };

    public GameState _gameState;

    private void Awake()
    {
        carScore = 0;
    }

    private void OnEnable()
    {
        AllEventsScript.OnArrestComplete += OnCarArrestComplete;
        AllEventsScript.OnCarLifeDecrease += OnCarHPDecrease;
        AllEventsScript.OnCountdownOver += OnCountdownOver;
    }

    private void OnDisable()
    {
        AllEventsScript.OnArrestComplete -= OnCarArrestComplete;
        AllEventsScript.OnCarLifeDecrease -= OnCarHPDecrease;
        AllEventsScript.OnCountdownOver -= OnCountdownOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        // OnGameStart();
        countScript.StartTimer();//Start count down
    }

    private void Update()
    {
        carScore = (uint)(carScript.distanceTravelled * 0.1f);
        scoreText.text = "Score : " + carScore.ToString("00000");
    }

    //Game Events
    private void OnGameStart()
    {
        //Unpause all
        policeOverseerScr.SetPaused(false);
        carScript.SetPaused(false);
        _gameState = GameState.RUNNING;
    }

    private void OnGameOver()
    {
        _gameState = GameState.OVER;
        gameOverText.SetActive(true);
    }

    //Events
    void OnCarArrestComplete()
    {
        if (_gameState != GameState.OVER)
            OnGameOver();
    }
    void OnCarHPDecrease(uint hp)
    {
        livesBarScript.SetHP(hp);
    }
    void OnCountdownOver()
    {
        OnGameStart();
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(1);
    }
   public  void OnBackButtonPressed()
    {
        SceneManager.LoadScene(0);
    }


}
