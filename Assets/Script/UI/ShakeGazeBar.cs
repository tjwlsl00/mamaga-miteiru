using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShakeGazeBar : MonoBehaviour
{
    // 이미지
    public Image MomFaceImage;
    public Sprite[] sprites;
    // 게이지 
    public RectTransform uiRectTransform;
    public static ShakeGazeBar Instance;

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

        if (uiRectTransform == null)
        {
            uiRectTransform = GetComponent<RectTransform>();
        }

    }

    void Start()
    {
        DefaultFace();
    }

    #region 엄마 프로필 사진 변경
    private void ChangeFace(int index)
    {
        MomFaceImage.sprite = sprites[index];
    }

    // 초기 
    public void DefaultFace()
    {
        ChangeFace(0);
    }
    
    public void AngryFace()
    {
        ChangeFace(1);
    }
    #endregion

    #region 게이지 바 흔들기 
    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = uiRectTransform.anchoredPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            uiRectTransform.anchoredPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        uiRectTransform.anchoredPosition = originalPos;
    }
    #endregion
}