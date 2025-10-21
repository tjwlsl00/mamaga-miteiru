using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Mom : MonoBehaviour
{
    #region 내부
    public float currentAngryGaze;
    private float reduceGaze = 10f;
    private float maxAngryGaze = 50f;
    // bool
    private bool hasSoundPlayed = false;
    private bool isIncreaseSoundPlayed = false;
    // 컴포넌트 / UI연결 
    private Animator animator;
    public Image AngryGaze;
    public TextMeshProUGUI GazeText;
    // 하트 오브젝트 
    public GameObject heartPrefab;
    // 사운드 
    private AudioSource audioSource;
    public AudioClip IncreaseGazeClip;
    public AudioClip attackClip;
    public AudioClip happyClip;
    // 싱글톤 
    public static Mom Instance;
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

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // 게이지 초기화 
        currentAngryGaze = 0f;

        // 하트 오브젝트 안보이게 
        heartPrefab.SetActive(false);
    }

    void Update()
    {
        CheckAngryGaze();
    }

    #region 게이지 실시간 확인 / 업데이트
    void CheckAngryGaze()
    {
        AngryGaze.fillAmount = currentAngryGaze / maxAngryGaze;
        GazeText.text = currentAngryGaze.ToString() + "/" + maxAngryGaze.ToString();

        // 게이지가 절반이상일때 엄마 프로필 화난 얼굴로
        if (currentAngryGaze >= maxAngryGaze / 2)
        {
            ShakeGazeBar.Instance.AngryFace();
        }
        else
        {
            ShakeGazeBar.Instance.DefaultFace();
        }

        // 게이지가 max에 도달했을 때 
        if (currentAngryGaze >= maxAngryGaze)
        {
            GameManager.Instance.GameOver();

            currentAngryGaze = 0f; // 분노 게이지 초기화 
        }
    }

    // 게이지 증가 
    public void IncreaseGaze(float amount)
    {
        currentAngryGaze += amount;
        if (!isIncreaseSoundPlayed)
        {
            StartCoroutine(IncreaseEffect());
        }
    }

    IEnumerator IncreaseEffect()
    {
        isIncreaseSoundPlayed = true;
        // UI 흔들림 효과
        ShakeGazeBar.Instance.StartShake(0.5f, 2f);
        // 오디오 
        audioSource.PlayOneShot(IncreaseGazeClip);
        yield return new WaitForSeconds(IncreaseGazeClip.length);
        isIncreaseSoundPlayed = false;
    }
    #endregion

    #region 엄마 게이지 감소
    public void ReduceAngryGaze()
    {
        if (currentAngryGaze <= 5f)
        {
            currentAngryGaze -= 5f;
        }
        else
        {
            currentAngryGaze -= reduceGaze;
        }
    }
    #endregion

    #region 게임 승리 / 오버 
    public void Victroy()
    {
        // 하트 오브젝트 보이게
        heartPrefab.SetActive(true);

        if (!hasSoundPlayed)
        {
            hasSoundPlayed = true;
            HappySoundPlay();
        }
    }

    public void GameOver()
    {
        animator.SetTrigger("isAttack");

        if (!hasSoundPlayed)
        {
            hasSoundPlayed = true;
            AttackSoundPlay();
        }
    }
    #endregion

    #region 사운드 효과 
    void AttackSoundPlay()
    {
        audioSource.clip = attackClip;
        audioSource.Play();
        hasSoundPlayed = true;
    }

    void HappySoundPlay()
    {
        audioSource.clip = happyClip;
        audioSource.Play();
        hasSoundPlayed = true;
    }
    #endregion
}
