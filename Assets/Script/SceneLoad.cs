using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    #region 내부
    public GameObject explainPanelParent;
    public GameObject[] explainPanels;
    [SerializeField] private int currentIndex;
    // 사운드 
    private AudioSource audioSource;
    public AudioClip buttonClickClip;
    #endregion

    void Awake()
    {
        explainPanelParent.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentIndex = 0;
    }

    public void GameSceneLoad()
    {
        SceneManager.LoadScene("GameScene");
    }

    #region 패널
    public void OpenPanel()
    {
        if (explainPanelParent != null)
        {
            explainPanelParent.gameObject.SetActive(true);
            ShowPanelAtIndex(currentIndex);
        }

        audioSource.clip = buttonClickClip;
        audioSource.Play();
    }

    public void ClosePanel()
    {
        if (explainPanelParent != null)
        {
            explainPanelParent.gameObject.SetActive(false);
            currentIndex = 0;
        }

        audioSource.clip = buttonClickClip;
        audioSource.Play();
    }

    public void ShowPanelAtIndex(int index)
    {
        for (int i = 0; i < explainPanels.Length; i++)
        {
            explainPanels[i].SetActive(i == index);
        }
    }

    public void ShowNextPanel()
    {
        currentIndex++;

        if (currentIndex >= explainPanels.Length)
        {
            currentIndex = 0;
        }

        ShowPanelAtIndex(currentIndex);
        audioSource.clip = buttonClickClip;
        audioSource.Play();
    }
    #endregion

}
