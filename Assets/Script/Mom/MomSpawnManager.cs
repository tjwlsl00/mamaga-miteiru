using UnityEngine;

public class MomSpawnManager : MonoBehaviour
{
    #region 변수
    [SerializeField] private float spawnDistance; //소환 거리
    // 플레이어 위치 저장 변수 
    private GameObject playerGameObject;
    private Transform playerTransform;
    // 싱글톤
    public static MomSpawnManager Instance;
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

        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        // 플레이어 위치 받아오기 
        playerTransform = playerGameObject.transform;
    }


    // 플레이어 앞으로 순간이동 
    public void MoveToPlayer()
    {
        if (Mom.Instance == null || playerTransform == null) return;

        Vector3 spawnPosition = playerTransform.position + (playerTransform.forward * spawnDistance);
        Mom.Instance.transform.position = spawnPosition; //엄마 오브젝트 순간이동
        Mom.Instance.transform.LookAt(playerTransform); //플레이어 바라보기 
    }
}