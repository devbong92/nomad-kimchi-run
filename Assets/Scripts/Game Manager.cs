using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Intro,
    Playing,
    Dead
}

/*
    *: Game Manager는 게임 전체를 관리하는 역할을 수행함.
    *: 누구든지 접근할 수 있어야 하고, 오직 단 하나만 존재해야 함!
*/
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State = GameState.Intro;

    public float PlayStartTime;

    public int Lives = 3;

    [Header("References")]
    public GameObject IntroUI;
    public GameObject DeadUI;
    public GameObject EnemySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldenSpawner;

    public Player PlayerScript;
    public TMP_Text scoreText;

    // * Unity에 의해서 자동으로 호출되는 함수로 Start 메서드보다 먼저 실행됨 
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Intro UI 활성화
        IntroUI.SetActive(true);
    }

    float CalculateScore()
    {
        return Time.time - PlayStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore());
        // 컴퓨터 디스크에 데이터를 저장 
        int currentHighScore = PlayerPrefs.GetInt("highScore");

        // 최고 점수 수정 
        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            // * 변경 사항 일괄적으로 저장 
            PlayerPrefs.Save();
        }
    }

    // 최고점수 조회 
    int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }

    // 
    public float CalculateGameSpeed()
    {
        if(State != GameState.Playing)
        {
            return 5f;
        }

        float speed = 8f + (0.5f * Mathf.Floor(CalculateScore() / 10f));
        
        return Mathf.Min(speed, 30f);
    }

    // Update is called once per frame
    void Update()
    {

        // 최고점수 업데이트
        if(State == GameState.Playing)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore());
        }else if (State == GameState.Dead)
        {
            scoreText.text = "High Score: " + GetHighScore();
        }

        // * 플레이어 최초 시작 
        if(State == GameState.Intro && Input.GetKeyDown(KeyCode.Space))
        {
            // 게임 상태 변경 
            State = GameState.Playing;
            // Intro UI 비활성화 
            IntroUI.SetActive(false);

            // 모든 Spawner 활성화 
            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldenSpawner.SetActive(true);

            // 플레이 시작 타임 저장
            PlayStartTime = Time.time;
        }

        // * 플레이어 사망 
        if(State == GameState.Playing && Lives == 0 )
        {
            PlayerScript.KillPlayer();

            // 모든 Spawner 비활성화 
            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpawner.SetActive(false);

            // 플레이어 사망처리 
            State = GameState.Dead;

            DeadUI.SetActive(true);
        }

        // * 플레이어 재시작 
        if(State == GameState.Dead && Input.GetKeyDown(KeyCode.Space))
        {
            // * Scene 재시작
            SceneManager.LoadScene("main");
        }
    }
}
