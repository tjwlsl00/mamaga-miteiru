using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameDirection
    {
        Playing,
        Victroy,
        GameOver
    }
    public GameDirection currentGameDirection;

    #region 내부
    // UI연결
    public TextMeshProUGUI StartSouziText;
    [SerializeField] private float fadeDuration = 2f; //글씨 페이드 아웃 시키는 시간 
    public GameObject VictroyPanel;
    public GameObject GameOverPanel;
    // 오디오 
    private AudioSource audioSource;
    public AudioClip souziClip;
    // 싱글톤 
    public static GameManager Instance;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentGameDirection = GameDirection.Playing;
        VictroyPanel.SetActive(false);
        GameOverPanel.SetActive(false);

        // 글씨 페이드 아웃 
        StartSouziText.color = new Color32(255, 255, 255, 255);
        StartCoroutine(FadeOut());

        // 사운드 
        audioSource.PlayOneShot(souziClip);
    }

    #region 게임 승리 / 오버 
    public void GameVictroy()
    {
        currentGameDirection = GameDirection.Victroy;
        VictroyPanel.SetActive(true);

        // 게임 승리 로직 호출
        Player.Instance.Victroy();
        MomSpawnManager.Instance.MoveToPlayer();
        Mom.Instance.Victroy();
    }

    public void GameOver()
    {
        currentGameDirection = GameDirection.GameOver;
        GameOverPanel.SetActive(true);

        // 게임 오버 로직 호출
        Player.Instance.GameOver();
        MomSpawnManager.Instance.MoveToPlayer();
        Mom.Instance.GameOver();
    }
    #endregion

    // 정리해 ! 텍스트 페이드 아웃 
    IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            byte newAlpha = (byte)Mathf.Lerp(255, 0, timer / fadeDuration);
            StartSouziText.color = new Color32(255, 255, 255, newAlpha);

            yield return null;
        }

        StartSouziText.color = new Color32(255, 255, 255, 0);
    }

    #region 버튼 효과
    // 재시작
    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // 메뉴 화면
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    #endregion

}
