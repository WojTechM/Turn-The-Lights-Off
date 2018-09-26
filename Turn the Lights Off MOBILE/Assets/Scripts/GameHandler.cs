using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [Header("Lightwaste'o meter")] public Slider BulbOverload;
    private int _bulbPoints = 1;
    private const int MaxBulbPoints = 100;

    [Header("Main canvases")] public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject InGameCanvas;
    public GameObject EndGameCanvas;
    public Text FinalScoreDisplay;
    public Text IngameScoreDisplay;

    [Header("HowToPlay canvases")] public GameObject[] HowToPlayCanvases;
    private int _canvasPointer;

    [Header("Gameplay objects & variables")]
    private bool _isPaused;

    public GameObject Player1;
    private float _score;
    private bool _isGameOver;

    private void Start()
    {
        _score = 0;
        _isGameOver = false;
        _isPaused = false;
        Time.timeScale = 0;
        PauseMenu.SetActive(false);
        InGameCanvas.SetActive(false);
        EndGameCanvas.SetActive(false);
        foreach (var canvas in HowToPlayCanvases)
        {
            canvas.SetActive(false);
        }
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!_isGameOver)
        {
            UpdateDisplay();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }

            if (_bulbPoints >= MaxBulbPoints)
            {
                _isGameOver = true;
                ShowGameEndingScreen();
            }
        }

        if (_isGameOver || _isPaused) return;
        _score += Time.deltaTime;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        IngameScoreDisplay.text = "Score: " + Mathf.RoundToInt(_score);
    }

    private void ShowGameEndingScreen()
    {
        FinalScoreDisplay.text = "" + Mathf.RoundToInt(_score);
        EndGameCanvas.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0;
        InGameCanvas.SetActive(false);
    }

    public void BulbPoint()
    {
        _bulbPoints++;
    }

    private void UpdateDisplay()
    {
        BulbOverload.value = _bulbPoints;
    }

    private void TogglePause()
    {
        if (_isPaused)
        {
            _isPaused = false;
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            InGameCanvas.SetActive(true);
        }
        else
        {
            _isPaused = true;
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            InGameCanvas.SetActive(false);
        }
    }

    //Accessed via button
    public void StartGame()
    {
        Time.timeScale = 1;
        MainMenu.SetActive(false);
        InGameCanvas.SetActive(true);
        EndGameCanvas.SetActive(false);
        var c = Player1.transform.GetChild(0).GetComponent<Camera>();
        c.rect = new Rect(0, 0, 1, 1);
        foreach (var canvas in HowToPlayCanvases)
        {
            canvas.SetActive(false);
        }
    }

    //Accessed via button
    public void ExitGame()
    {
        Application.Quit();
    }

    //Accessed via button
    public void RestartGame()
    {
        EndGameCanvas.SetActive(false);
        Player1.transform.position = GameObject.Find("P1SpawnPoint").transform.position;
        _score = 0;
        var kidSpawns = GameObject.FindGameObjectsWithTag("KidSpawn");
        var kids = GameObject.FindGameObjectsWithTag("Kid");
        for (var i = 0; i < kids.Length; i++)
        {
            kids[i].GetComponent<KidController>().ResetMoveVector();
            kids[i].transform.position = kidSpawns[i].transform.position;
        }

        GameObject[] lightSources = GameObject.FindGameObjectsWithTag("LightSource");
        foreach (var lightSource in lightSources)
        {
            lightSource.GetComponent<LightBulbBehaviour>().TurnOff();
        }

        _bulbPoints = 0;
        _isGameOver = false;
        TogglePause();
    }

    public void ShowHowToPlay()
    {
        MainMenu.SetActive(false);
        InGameCanvas.SetActive(false);
        EndGameCanvas.SetActive(false);
        _canvasPointer = 0;
        UpdateCanvasDisplay();
    }

    public void NextHowToPlayCanvas()
    {
        _canvasPointer++;
        UpdateCanvasDisplay();
    }

    private void UpdateCanvasDisplay()
    {
        if (_canvasPointer < HowToPlayCanvases.Length)
        {
            for (var i = 0; i < HowToPlayCanvases.Length; i++)
            {
                HowToPlayCanvases[i].SetActive(i == _canvasPointer);
            }
        }
        else
        {
            MainMenu.SetActive(true);
        }
    }

    public void UpdateScore(int points)
    {
        _score += points;
    }
}