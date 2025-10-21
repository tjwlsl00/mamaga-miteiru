using UnityEngine;

public class Gomi : MonoBehaviour
{
    #region  내부 
    [SerializeField] private float maxTimer = 10f;
    [SerializeField] private float GazePoint = 5f;
    private float timer;
    // 컴포넌트 연결 
    public AudioClip gomiClip;
    #endregion

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (GameManager.Instance.currentGameDirection == GameManager.GameDirection.Playing)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimer)
            {
                Mom.Instance.IncreaseGaze(GazePoint);
                Destroy(gameObject);
            }
        }
    }

    // 충돌 이벤트 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 사운드 재생 
            AudioSource.PlayClipAtPoint(gomiClip, transform.position);
            Destroy(gameObject);
        }
    }
}