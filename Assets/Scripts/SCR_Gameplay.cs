// class: SCR_Gameplay
using GoogleMobileAds.Api;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Gameplay : MonoBehaviour
{
	public readonly int[] CHARACTER_UNLOCKED_LEVELS = new int[7]
	{
		0,
		5,
		10,
		20,
		30,
		40,
		50
	};

	public const float TIME_SHOW_ADS = 30f;

	public const float FINISH_POINT_OFFSET_X = 10f;

	public const float DISTANCE_ANCHORS = 10f;

	public const float LEVEL_1_WALL_OFFSET_X = -6f;

	public const float LEVEL_1_WALL_OFFSET_Y = -4f;

	public const float ANCHOR_POSITION_Y_MIN = 4f;

	public const float ANCHOR_POSITION_Y_MAX = 6f;

	public const float WALL_POSITION_Y_MIN = -12.5f;

	public const float WALL_POSITION_Y_MAX = -9.5f;

	public static float SCREEN_WIDTH;

	public static float SCREEN_HEIGHT;

	public static SCR_Gameplay instance;

	private static bool skipMenu = false;

	public int character;

	public GameObject PFB_ANCHOR;

	public GameObject PFB_WALL;

	public GameObject PFB_START_POINT;

	public GameObject PFB_FINISH_POINT;

	public RuntimeAnimatorController[] ACL_CHARACTERS;

	public RuntimeAnimatorController[] ACL_MENU_CHARACTERS;

	public GameObject player;

	public GameObject endLevel;

	public GameObject imgWellDone;

	public GameObject imgKeepGoing;

	public GameObject tmpLevelNumber;

	public GameObject btnReplay;

	public GameObject btnNext;

	public GameObject imgExclamation;

	public GameObject mainMenu;

	public GameObject btnPlay;

	public GameObject tmpUnlock;

	public SCR_ProgressBar scrProgressBar;

	public SCR_Background scrBackgroundFront;

	public SCR_Background scrBackgroundMiddle;

	public SCR_Background scrBackgroundBack;

	public GameObject imgCharacter;

	public SCR_Tutorial scrTutorial;

	public bool updateCamera;

	public int currentLevel;

	public GameState state;

	private List<GameObject> anchors = new List<GameObject>();

	private List<GameObject> walls = new List<GameObject>();

	// Điểm bắt đầu khởi tạo player
	private GameObject startPoint;

	// Điểm kết thúc level
	private GameObject finishPoint;

	private float maxProgress;


	private int currentAnchor;
    private int numberCurrent = 1;

	private int recommendedCharacter;

	private InterstitialAd interstitial;

	private static float timeShowAds = 30f;

	//--------------
	public Transform anchorLast;
    public Transform wallLast;
    private List<Transform> wallS;
    private List<Transform> anchorS;

    public Transform anchorPrefab;
    public Transform wallPrefab;
    public Transform enemyPrefab;
    public int number = 5;
    private int numAnchor;
    private int numWall;
    private float num2 = 7.5f;
    public bool checkMode;
    public int score;
    public GameObject txtScore;
    //---------------- Config
    public ConfigLevelRecord cfLevel;

    public void Awake()
	{
		SCREEN_HEIGHT = Camera.main.orthographicSize * 2f;
		SCREEN_WIDTH = SCREEN_HEIGHT * (float)Screen.width / (float)Screen.height;
		instance = this;

        anchorS = new List<Transform>();
        wallS   = new List<Transform>();
	}

    public void AddLastAnchor(Transform anchor)
    {
        anchorS.Add(anchor);
        anchorLast = anchor;
    }
    public void AddLastWall(Transform wall)
    {
        wallS.Add(wall);
        wallLast = wall;
    }

    public void MoveAnchor()
    {
        for (int i = 0; i < anchorS.Count; i++)
        {
            if (anchorS[i].position.x < (player.transform.position.x -10))
            {
                anchorS[i].position = new Vector3(anchorLast.position.x + 10, GetRandomY(), 0);
                AddLastAnchor(anchorS[i]);
            }
        } 
    }
    public void MoveWall()
    {
        for (int i = 0; i < wallS.Count; i++)
        {
            if (wallS[i].position.x < (player.transform.position.x - 10))
            {
                wallS[i].position = new Vector3(wallLast.position.x + 10, GetRandomY2(), 0);
                AddLastWall(wallS[i]);
            }
        }
    }
    public void Start()
	{
        Application.targetFrameRate = 60;
		currentLevel = PlayerPrefs.GetInt("currentLevel", 0);
		character = PlayerPrefs.GetInt("character", 0);
		recommendedCharacter = PlayerPrefs.GetInt("recommendedCharacter", 0);
		if (!skipMenu && recommendedCharacter != 0)
		{
			character = recommendedCharacter;
			PlayerPrefs.SetInt("character", character);
			recommendedCharacter = 0;
			PlayerPrefs.SetInt("recommendedCharacter", recommendedCharacter);
		}
		UpdateCharacter();
		player.SetActive(value: false);
		scrProgressBar.gameObject.SetActive(value: false);
		endLevel.SetActive(value: false);
		mainMenu.SetActive(value: true);
		maxProgress = 0f;
		scrProgressBar.SetProgress(0f);
		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
		updateCamera = true;
		scrTutorial.Hide();
		state = GameState.MENU;
		MobileAds.Initialize("ca-app-pub-0081066185741622~8874259147");
		RequestInterstitial();
        // Button Replay
		if (skipMenu && checkMode == false)
		{
            SwitchState(GameState.READYENDLESS);
        }
        else if (skipMenu && checkMode == true)
        {
            SwitchState(GameState.READYLEVEL);
        }
	}
	private void RequestInterstitial()
	{
		string adUnitId = "ca-app-pub-0081066185741622/9034041242";
		interstitial = new InterstitialAd(adUnitId);
		AdRequest request = new AdRequest.Builder().AddTestDevice("36e6813a9776d338128e27da33a0467f").AddTestDevice("287cea1fe1bd25fe56e59ea1e0566114").Build();
		interstitial.LoadAd(request);
	}

	private void ShowAds()
	{
		interstitial.Show();
		interstitial.Destroy();
		RequestInterstitial();
		timeShowAds = 0f;
	}
	public void Update()
	{
		timeShowAds += Time.unscaledDeltaTime;
		if (player != null && startPoint != null && finishPoint != null)
		{
			float num = player.transform.position.x - startPoint.transform.position.x;
			float num2 = finishPoint.transform.position.x - startPoint.transform.position.x;
			float num3 = num / num2;
			if (maxProgress < num3)
			{
				maxProgress = num3;
				scrProgressBar.SetProgress(maxProgress);
			}
		}
        if (state == GameState.GAME_OVER)
        {
            txtScore.SetActive(value: false);
        }
    }

	public void LateUpdate()
	{
		if (player != null && updateCamera)
		{
			float num = 0.3f * SCREEN_WIDTH;
			Transform transform = Camera.main.transform;
			float x = transform.position.x;
			float num2 = player.transform.position.x + num;
			transform.position = new Vector3(num2, transform.position.y, transform.position.z);
			scrBackgroundFront.Move(num2 - x);
			scrBackgroundMiddle.Move(num2 - x);
			scrBackgroundBack.Move(num2 - x);
		}
	}
    private void ReadyLevel()
    {
        Debug.LogError(currentLevel);
        // Hide mainMenu
        mainMenu.SetActive(value: false);
        
        // if new player show tutorial
        // else show ProgressBar
        if (currentLevel == 0)
        {
            scrTutorial.Show();
            scrProgressBar.gameObject.SetActive(value: false);
        }
        else
        {
            scrProgressBar.SetLevel(currentLevel);
            scrProgressBar.gameObject.SetActive(value: true);
        }
        // Get Anim Player
        // Show Player
        player.GetComponent<Animator>().runtimeAnimatorController = ACL_CHARACTERS[character];
        player.SetActive(value: true);
        // Use Data Config
        ConfiglevelKey key = new ConfiglevelKey();
        key.id = cfLevel.id;
        key.level = currentLevel;
        cfLevel = ConfigManager.instance.configlevel.GetRecordBykeySearch(key);
        Debug.LogError("Level: "+cfLevel.level);
        Debug.LogError(cfLevel.anchor + " anchor");
        Debug.LogError(cfLevel.wall + " wall");
        Debug.LogError(cfLevel.enemy + " enemy");
        
        //--------Create Anchor
        for (int i = 0; i < cfLevel.anchor; i++)
        {
            Transform anchor = CreateAnchor();
            anchor.name = "anchor number " + i;
            anchorS.Add(anchor);
            anchor.position = new Vector3(num2 + (float)i * 10f, GetRandomY(), 0);
            AnchorControl anchorControl = anchor.GetComponent<AnchorControl>();
            anchorControl.Setup(this);
        }
        anchorLast = anchorS[anchorS.Count - 1];

        //-------------Create Wall
        for (int j = 0; j < cfLevel.wall; j++)
        {
            Transform wall = CreateWall();
            wall.name = "wall number" + j;
            wallS.Add(wall);
            wall.position = new Vector3(num2 + (float)j * 10f, GetRandomY2(), 0);
            WallControl wallControl = wall.GetComponent<WallControl>();
            wallControl.Setup(this);

            if (j == wallS.Count - 1)
                wallLast = wall;
        }
        //-------------------Create Enemy
        //OnCreateEnemyLevelFollowPos(anchorS, 0);
        //--------------------------Create StartPoint
        AddStartPoint();
        //-----------------------------------Create Finish Point
        AddFinishPoint();
        finishPoint.transform.position = new Vector3(anchorS[anchorS.Count - 1].transform.position.x + 10f, finishPoint.transform.position.y, finishPoint.transform.position.z);
        //if (currentLevel == 1)
        //{
        //    wallS[0].transform.position = new Vector3(finishPoint.transform.position.x + -6f, finishPoint.transform.position.y + -4f, wallS[0].transform.position.z);
        //}
        //-----------
        currentAnchor = -1;
    }
    private void ReadyEndless()
	{
		mainMenu.SetActive(value: false);
        txtScore.SetActive(value: true);
		
		player.GetComponent<Animator>().runtimeAnimatorController = ACL_CHARACTERS[character];
		player.SetActive(value: true);

        numAnchor = 7;
        numWall = 7;
        AddAnchor(numAnchor);
        AddWall(numWall);
        AddStartPoint();
        // At initialization currentAnchor = -1(not grab any time yet)
        currentAnchor = -1;
    }
    private void AddAnchor(int numAnchor)
    {
        for (int i = 0; i < numAnchor; i++)
        {
            Transform anchor = CreateAnchor();
            anchor.name = "anchor number " + i;
            anchorS.Add(anchor);
            anchor.position = new Vector3(num2 + (float)i * 10f, GetRandomY(), 0);
            AnchorControl anchorControl = anchor.GetComponent<AnchorControl>();
            anchorControl.Setup(this);
        }
        anchorLast = anchorS[anchorS.Count - 1];
    }
    private void AddWall(int numWall)
    {
        for (int j = 0; j < numWall; j++)
        {
            Transform wall = CreateWall();
            wallS.Add(wall);
            wall.position = new Vector3(num2 + (float)j * 10f, GetRandomY2(), 0);
            WallControl wallControl = wall.GetComponent<WallControl>();
            wallControl.Setup(this);
        }
        wallLast = wallS[wallS.Count - 1];
    }
    private void AddStartPoint()
    {
        startPoint = UnityEngine.Object.Instantiate(PFB_START_POINT);
    }
    private void AddFinishPoint()
    {
        finishPoint = UnityEngine.Object.Instantiate(PFB_FINISH_POINT);
    }

	public float GetRandomY()
    {
       return UnityEngine.Random.Range(4f, 6f);
    }
    public float GetRandomY2()
    {
        return UnityEngine.Random.Range(-10f, -13f);
    }
    private Transform CreateAnchor()
    {
        Transform column = Instantiate(anchorPrefab);
        return column;
    }
    private Transform CreateWall()
    {
        Transform wall = Instantiate(wallPrefab);
        return wall;
    }
    private Transform CreateEnemy()
    {
        Transform enemy = Instantiate(enemyPrefab);
        return enemy;
    }
    public void OnCreateEnemyEndless()
    {
        int numberWall = UnityEngine.Random.Range(1, 4);
        //int numberWall = 2;
        if (numberCurrent > numberWall)
        {
            Transform enemy = CreateEnemy();
            //enemy.position = new Vector3(wallLast.position.x,0, 0);
            enemy.position = new Vector3(anchorS[currentAnchor].position.x + 10f, 0, 0);
            numberCurrent = 0;
        }
    }
    public void OnCreateEnemyLevelFollowPos(List<Transform> anchor, int pos)
    {
        Transform enemy = CreateEnemy();
        enemy.position = new Vector3(anchor[pos].position.x, 0, 0);
    }

	public void SwitchState(GameState s)
	{
		if (state == GameState.GAME_OVER || state == GameState.LEVEL_CLEARED)
		{
			return;
		}
		state = s;
		if (state == GameState.READYENDLESS)
		{
			ReadyEndless();
		}
        if (state == GameState.READYLEVEL)
        {
            ReadyLevel();
        }
		else if (state == GameState.PLAY)
		{
			startPoint.GetComponent<Collider2D>().enabled = false;
		}
		else if (state == GameState.GAME_OVER)
		{
			scrProgressBar.gameObject.SetActive(value: false);
			tmpLevelNumber.GetComponent<TextMeshProUGUI>().text = (currentLevel + 1).ToString("D2");
			imgWellDone.SetActive(value: false);
			imgKeepGoing.SetActive(value: true);
			btnNext.SetActive(value: false);
			btnReplay.SetActive(value: true);
			endLevel.SetActive(value: true);
			imgExclamation.SetActive(value: false);
			scrTutorial.Hide();
		}
		else
		{
			if (state != GameState.LEVEL_CLEARED)
			{
				return;
			}
			scrProgressBar.gameObject.SetActive(value: false);
			tmpLevelNumber.GetComponent<TextMeshProUGUI>().text = (currentLevel + 1).ToString("D2");
			imgWellDone.SetActive(value: true);
			imgKeepGoing.SetActive(value: false);
			btnNext.SetActive(value: true);
			btnReplay.SetActive(value: false);
			endLevel.SetActive(value: true);
			scrTutorial.Hide();
			for (int i = 0; i < CHARACTER_UNLOCKED_LEVELS.Length; i++)
			{
				if (currentLevel + 1 == CHARACTER_UNLOCKED_LEVELS[i])
				{
					recommendedCharacter = i;
					PlayerPrefs.SetInt("recommendedCharacter", recommendedCharacter);
					break;
				}
			}
			if (recommendedCharacter != 0)
			{
				imgExclamation.SetActive(value: true);
			}
			else
			{
				imgExclamation.SetActive(value: false);
			}
			currentLevel++;
			PlayerPrefs.SetInt("currentLevel", currentLevel);
		}
	}

    public Transform GetNextAnchor()
    {
        do
        {
            currentAnchor++;
            numberCurrent++;
            score++;
        }
        while (currentAnchor < anchorS.Count && anchorS[currentAnchor].position.x < player.transform.position.x);
        if (currentAnchor < anchorS.Count)
        {
            if (checkMode == false)
            {
                OnCreateEnemyEndless();
            }
            
            return anchorS[currentAnchor];
        }
        return null;
    }
  
    public bool IsLastAnchor(Rigidbody2D rb)
	{
		if (rb.gameObject == anchors[anchors.Count - 1])
		{
			return true;
		}
		return false;
	}
    public bool IsLastAnchor2(Rigidbody2D rb)
    {
        if (rb.gameObject == anchorS[anchorS.Count - 1])
        {
            return true;
        }
        return false;
    }
    public void OnReplay()
	{
		if (timeShowAds >= 30f && interstitial.IsLoaded())
		{
			ShowAds();
		}
		skipMenu = true;
        if (checkMode == false && state == GameState.GAME_OVER)
        {
            SwitchState(GameState.READYENDLESS);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           
        }
        else if (checkMode == true && state == GameState.GAME_OVER || state == GameState.LEVEL_CLEARED)
        {
            SwitchState(GameState.READYLEVEL);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
          
    }

    // Part of mode Level
	public void OnNext()
	{
		if (timeShowAds >= 30f && interstitial.IsLoaded())
		{
			ShowAds();
		}
		skipMenu = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void OnHome()
	{
		if (timeShowAds >= 30f && interstitial.IsLoaded())
		{
			ShowAds();
		}
		skipMenu = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnArrowLeft()
	{
		character--;
		if (character < 0)
		{
			character = ACL_MENU_CHARACTERS.Length - 1;
		}
		UpdateCharacter();
		PlayerPrefs.SetInt("character", character);
	}

	public void OnArrowRight()
	{
		character++;
		if (character >= ACL_MENU_CHARACTERS.Length)
		{
			character = 0;
		}
		UpdateCharacter();
		PlayerPrefs.SetInt("character", character);
	}

	private void UpdateCharacter()
	{
		imgCharacter.GetComponent<Animator>().runtimeAnimatorController = ACL_MENU_CHARACTERS[character];
		if (currentLevel + 1 >= CHARACTER_UNLOCKED_LEVELS[character])
		{
			btnPlay.SetActive(value: true);
			tmpUnlock.transform.parent.gameObject.SetActive(value: false);
		}
		else
		{
			tmpUnlock.GetComponent<TextMeshProUGUI>().text = "UNLOCKED AT LEVEL " + CHARACTER_UNLOCKED_LEVELS[character];
			btnPlay.SetActive(value: false);
			tmpUnlock.transform.parent.gameObject.SetActive(value: true);
		}
	}

	public void OnPlay()
	{
		SwitchState(GameState.READYENDLESS);
        checkMode = false;
	}
    public void OnPlayLevel()
    {
        SwitchState(GameState.READYLEVEL);
        checkMode = true;
    }
}
