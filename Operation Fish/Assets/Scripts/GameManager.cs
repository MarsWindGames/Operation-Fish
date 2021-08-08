using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Background Prefabs")]
    public GameObject[] backgrounds;
    public Vector3 backgroundSpawnPoint;

    //Instances
    public GameObject player;
    Animator animator;
    public Camera cam;

    //Unity
    PlayerMovement playerMovement;
    bool gameStarted = false;
    ObstacleMover[] obstacleMovers;
    public int currentLevel = 1;
    public int coinCount = 0;
    public int starsGained = 1; // all coins are gathered =  3 stars.
    public int playerScore;
    Camera mainCam;
    UIManager uIManager;
    bool gameFinished = false;

    public static GameManager instance;
    //Events
    #region 
    public delegate void OnGameStartedDelegate();
    public event OnGameStartedDelegate OnGameStarted;
    public bool GameStarted
    {
        get { return gameStarted; }
        set
        {
            if (gameStarted == value) return;
            gameStarted = value;
            if (OnGameStarted != null)
                OnGameStarted();
        }
    }
    #endregion

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (instance == null)
            instance = this;

        OnGameStarted += FreezeObjects;
        OnGameStarted += StartGame;

        uIManager = UIManager.instance;

        LoadGame();

        LoadBackground();
    }

    void Start()
    {
        obstacleMovers = GameObject.FindObjectsOfType<ObstacleMover>();
        playerMovement = player.GetComponent<PlayerMovement>();
        animator = player.GetComponent<Animator>();
        mainCam = Camera.main;

        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coinCount++;
        }

        int tutorial = PlayerPrefs.GetInt("playerTutorial", 0);

        if (tutorial != 1)
        {
            uIManager.showTutorialCanvas();
            PlayerPrefs.SetInt("playerTutorial", 1);
        }
    }

    void StartGame()
    {
        GameStarted = true;
        playerMovement.StartMoving();
        animator.SetBool("idle", false);
        UIManager.instance.hideMainCanvas();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    void FreezeObjects()
    {
        // when the game starts objects need to be frozen (prevent moving) will be frozen.
        foreach (ObstacleMover oMover in obstacleMovers)
        {
            oMover.FreezeObject();
        }
    }

    public void finishGame()
    {
        // show finish canvas
        // give player score point

        if (!gameFinished)
        {
            if (coinCount == 0)
            {
                starsGained = 3;
            }
            else if (coinCount > 3)
            {
                starsGained = 1;
            }
            else
            {
                starsGained = 2;
            }
            uIManager.showFinishCanvas();

            SaveManager.instance.SavePlayerInfo(true);
            SaveManager.instance.SaveMapInfo(currentLevel, starsGained);
            animator.SetBool("happy", true);
            gameFinished = true;
        }
    }

    public void failGame()
    {
        uIManager.showFailCanvas();
        animator.SetBool("angry", true);
    }

    private void LoadGame()
    {
        currentLevel = SaveManager.instance.selectedLevelIndex;

        MapSaver mapSaver = FindObjectOfType<MapSaver>();
        if (mapSaver.mapCount() >= currentLevel)
        {
            mapSaver.LoadMap(currentLevel);
        }
        else
        {
            mapSaver.LoadMap(1);
        }
    }

    private void LoadBackground()
    {
        int rnd = UnityEngine.Random.Range(0, backgrounds.Length);
        Instantiate(backgrounds[rnd], backgroundSpawnPoint, Quaternion.identity);
    }
}
